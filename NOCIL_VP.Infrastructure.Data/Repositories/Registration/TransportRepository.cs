using AutoMapper;
using Microsoft.Extensions.Configuration;
using NOCIL_VP.Domain.Core.Dtos;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Approval;
using NOCIL_VP.Domain.Core.Entities.Registration.CommonData;
using NOCIL_VP.Domain.Core.Entities.Registration.Transport;
using NOCIL_VP.Infrastructure.Data.Enums;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration;
using NPOI.OpenXmlFormats.Dml.Diagram;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Registration
{
    public class TransportRepository : ITransportRepository
    {
        private VpContext _dbContext;
        private IMapper _mapper;
        private ModelValidator _validator;
        private EmailHelper _emailHelper;

        public TransportRepository(VpContext dbContext, IMapper mapper, EmailHelper email)
        {
            this._dbContext = dbContext;
            _mapper = mapper;

            _validator = new ModelValidator();
            _emailHelper = email;
        }


        public async Task<bool> SaveTransportVendorDetails(TransportForm transportForm, int formId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    this._dbContext.Transport_Vendor_Personal_Data.Add(this._mapper.Map<TransportVendorPersonalData>(transportForm.TransportVendorPersonalData));
                    this._dbContext.Tanker_Details.AddRange(this._mapper.Map<List<TankerDetail>>(transportForm.TankerDetails));
                    this._dbContext.Vehicle_Details.AddRange(this._mapper.Map<List<VehicleDetails>>(transportForm.VehicleDetails));
                    this._dbContext.Bank_Details.Add(this._mapper.Map<Bank_Detail>(transportForm.BankDetail));
                    this._dbContext.Commercial_Profile.Add(this._mapper.Map<CommercialProfile>(transportForm.CommercialProfile));
                    this._dbContext.Addresses.AddRange(this._mapper.Map<List<Address>>(transportForm.Addresses));
                    this._dbContext.Contacts.AddRange(this._mapper.Map<List<Contact>>(transportForm.Contacts));
                    this._dbContext.VendorBranches.AddRange(this._mapper.Map<List<VendorBranch>>(transportForm.VendorBranches));

                    var form = this._dbContext.Forms.Where(f => f.Form_Id == transportForm.TransportVendorPersonalData.Form_Id).FirstOrDefault();
                    var roleId = this._dbContext.User_Role_Mappings.FirstOrDefault(x => x.Employee_Id == form.Created_By).Role_Id;
                    form.Status_Id = (int)FormStatusEnum.Pending;
                    var workFlow = await this.UpdateWorkflow(formId, form.Created_By, roleId);
                    if (workFlow)
                    {
                        await this._dbContext.SaveChangesAsync();
                        var toMail = (from user in _dbContext.Users
                                      where user.Employee_Id == form.Created_By
                                      select user).FirstOrDefault();

                        ApprovalMailInfo approval = new ApprovalMailInfo()
                        {
                            Form_Id = formId,
                            ToEmail = toMail!.Email,
                            Email = form.Vendor_Mail,
                            UserName = $"{toMail.First_Name} {toMail.Middle_Name} {toMail.Last_Name}".TrimEnd(),
                            Mobile_Number = form.Vendor_Mobile
                        };

                        await this._emailHelper.SendApprovalRequestMail(approval);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        transaction.Dispose();
                        throw new Exception("Something went wrong.");
                    }

                    await transaction.CommitAsync();
                    transaction.Dispose();

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }

        public async Task<bool> UpdateTransportVendorDetails(TransportForm transportForm, int formId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    this._dbContext.Transport_Vendor_Personal_Data.Update(this._mapper.Map<TransportVendorPersonalData>(transportForm.TransportVendorPersonalData));
                    this._dbContext.Tanker_Details.UpdateRange(this._mapper.Map<List<TankerDetail>>(transportForm.TankerDetails));
                    this._dbContext.Vehicle_Details.UpdateRange(this._mapper.Map<List<VehicleDetails>>(transportForm.VehicleDetails));
                    this._dbContext.Bank_Details.Update(this._mapper.Map<Bank_Detail>(transportForm.BankDetail));
                    this._dbContext.Commercial_Profile.Update(this._mapper.Map<CommercialProfile>(transportForm.CommercialProfile));
                    this._dbContext.VendorBranches.UpdateRange(this._mapper.Map<List<VendorBranch>>(transportForm.VendorBranches));

                    //var form = this._dbContext.Forms.Where(f => f.Form_Id == domesticForm.DomesticVendorPersonalData.Form_Id).FirstOrDefault();
                    var oldTask = this._dbContext.Tasks.Where(f => f.Form_Id == formId && f.Level == 1).FirstOrDefault();
                    var workFlow = await this.UpdateWorkflow(formId, oldTask.Owner_Id, oldTask.Role_Id, oldTask.Level);
                    var form = _dbContext.Forms.FirstOrDefault(x => x.Form_Id == formId);
                    form.Status_Id = form.Status_Id == (int)FormStatusEnum.Rejected ? (int)FormStatusEnum.Pending : (int)FormStatusEnum.EditApprovalPending;
                    if (workFlow) await this._dbContext.SaveChangesAsync();
                    else
                    {
                        await transaction.RollbackAsync();
                        transaction.Dispose();
                        throw new Exception("Something went wrong.");
                    }

                    await transaction.CommitAsync();
                    transaction.Dispose();

                    return true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }

        private async Task<bool> UpdateWorkflow(int formId, string Employee_Id,int? roleId, int level = 1)
        {
            Tasks task = new Tasks
            {
                Task_Id = 0,
                Form_Id = formId,
                Status = "Active",
                StartDate = DateTime.Now,
                Level = level,
                Owner_Id = Employee_Id,
                Role_Id = roleId
            };
            await _dbContext.AddAsync(task);
            return true;
        }
    }
}
