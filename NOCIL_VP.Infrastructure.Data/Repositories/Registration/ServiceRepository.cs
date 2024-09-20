using AutoMapper;
using Microsoft.Extensions.Configuration;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos;
using NOCIL_VP.Domain.Core.Entities.Approval;
using NOCIL_VP.Domain.Core.Entities.Registration.CommonData;
using NOCIL_VP.Domain.Core.Entities.Registration.Domestic;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Infrastructure.Data.Enums;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Registration
{
    public class ServiceRepository : IServiceRepository
    {
        private VpContext _dbContext;
        private IMapper _mapper;
        private ModelValidator _validator;
        private EmailHelper _emailHelper;

        public ServiceRepository(VpContext context, IMapper mapper, EmailHelper email)
        {
            _dbContext = context;
            _mapper = mapper;
            _validator = new ModelValidator();
            _emailHelper = email;
        }

        public async Task<bool> SaveServiceVendorDetails(ServiceForm serviceForm, int formId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    this._dbContext.Vendor_Personal_Data.Add(this._mapper.Map<VendorPersonalData>(serviceForm.VendorPersonalData));
                    this._dbContext.Addresses.AddRange(this._mapper.Map<List<Address>>(serviceForm.Addresses));
                    this._dbContext.Contacts.AddRange(this._mapper.Map<List<Contact>>(serviceForm.Contacts));
                    this._dbContext.Vendor_Organization_Profile.Add(this._mapper.Map<VendorOrganizationProfile>(serviceForm.VendorOrganizationProfile));
                    if (serviceForm.VendorOrganizationProfile.RelationToNocil)
                    {
                        this._dbContext.NocilRelatedEmployees.AddRange(this._mapper.Map<List<NocilRelatedEmployee>>(serviceForm.NocilRelatedEmployees));
                    }
                    this._dbContext.Proprietor_or_Partners.AddRange(this._mapper.Map<List<ProprietorOrPartner>>(serviceForm.ProprietorOrPartners));
                    this._dbContext.Technical_Profile.Add(this._mapper.Map<TechnicalProfile>(serviceForm.TechnicalProfile));
                    this._dbContext.Subsideries.AddRange(this._mapper.Map<List<Subsideries>>(serviceForm.Subsideries));
                    this._dbContext.MajorCustomers.AddRange(this._mapper.Map<List<MajorCustomer>>(serviceForm.MajorCustomers));
                    this._dbContext.Commercial_Profile.Add(this._mapper.Map<CommercialProfile>(serviceForm.CommercialProfile));
                    this._dbContext.Bank_Details.Add(this._mapper.Map<Bank_Detail>(serviceForm.BankDetail));
                    this._dbContext.VendorBranches.AddRange(this._mapper.Map<List<VendorBranch>>(serviceForm.VendorBranches));

                    var form = this._dbContext.Forms.Where(f => f.Form_Id == serviceForm.VendorPersonalData.Form_Id).FirstOrDefault();
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

        public async Task<bool> UpdateServiceVendorDetails(ServiceForm serviceForm, int formId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    this._dbContext.Vendor_Personal_Data.Update(this._mapper.Map<VendorPersonalData>(serviceForm.VendorPersonalData));
                    this._dbContext.Addresses.UpdateRange(this._mapper.Map<List<Address>>(serviceForm.Addresses));
                    this._dbContext.Contacts.UpdateRange(this._mapper.Map<List<Contact>>(serviceForm.Contacts));
                    this._dbContext.Vendor_Organization_Profile.Update(this._mapper.Map<VendorOrganizationProfile>(serviceForm.VendorOrganizationProfile));
                    this._dbContext.Proprietor_or_Partners.UpdateRange(this._mapper.Map<List<ProprietorOrPartner>>(serviceForm.ProprietorOrPartners));
                    this._dbContext.Technical_Profile.Update(this._mapper.Map<TechnicalProfile>(serviceForm.TechnicalProfile));
                    this._dbContext.Commercial_Profile.Update(this._mapper.Map<CommercialProfile>(serviceForm.CommercialProfile));
                    this._dbContext.Bank_Details.Update(this._mapper.Map<Bank_Detail>(serviceForm.BankDetail));

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

        private async Task<bool> UpdateWorkflow(int formId, string Employee_Id, int? roleId, int level = 1)
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
