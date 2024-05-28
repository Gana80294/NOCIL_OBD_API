using AutoMapper;
using Microsoft.Extensions.Configuration;
using NOCIL_VP.Domain.Core.Dtos;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos.Registration.CommonData;
using NOCIL_VP.Domain.Core.Dtos.Registration.Domestic;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Approval;
using NOCIL_VP.Domain.Core.Entities.Registration.CommonData;
using NOCIL_VP.Domain.Core.Entities.Registration.Domestic;
using NOCIL_VP.Infrastructure.Data.Enums;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration.UpdateForm;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Registration
{
    public class DomesticAndImportRepository : IDomesticAndImportRepository
    {
        private VpContext _dbContext;
        private IMapper _mapper;
        private ModelValidator _validator;
        private EmailHelper _emailHelper;
        private IUpdateFormRepository _updateFormRepo;

        public DomesticAndImportRepository(VpContext context, IMapper mapper, EmailHelper mail, IUpdateFormRepository updateFormRepo)
        {
            _dbContext = context;
            _mapper = mapper;
            _validator = new ModelValidator();
            _emailHelper = mail;
            _updateFormRepo = updateFormRepo;
        }

        public async Task<bool> SaveDomesticAndImportVendorDetails(DomesticAndImportForm domesticAndImportForm, int formId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    this._dbContext.Vendor_Personal_Data.Add(this._mapper.Map<VendorPersonalData>(domesticAndImportForm.VendorPersonalData));
                    this._dbContext.Addresses.AddRange(this._mapper.Map<List<Address>>(domesticAndImportForm.Addresses));
                    this._dbContext.Contacts.AddRange(this._mapper.Map<List<Contact>>(domesticAndImportForm.Contacts));
                    this._dbContext.VendorBranches.AddRange(this._mapper.Map<List<VendorBranch>>(domesticAndImportForm.VendorBranches));
                    this._dbContext.Vendor_Organization_Profile.Add(this._mapper.Map<VendorOrganizationProfile>(domesticAndImportForm.VendorOrganizationProfile));
                    if (domesticAndImportForm.VendorOrganizationProfile.RelationToNocil)
                    {
                        this._dbContext.NocilRelatedEmployees.AddRange(this._mapper.Map<List<NocilRelatedEmployee>>(domesticAndImportForm.NocilRelatedEmployees));
                    }
                    this._dbContext.Proprietor_or_Partners.AddRange(this._mapper.Map<List<ProprietorOrPartner>>(domesticAndImportForm.ProprietorOrPartners));
                    this._dbContext.Annual_TurnOver.AddRange(this._mapper.Map<List<AnnualTurnOver>>(domesticAndImportForm.AnnualTurnOvers));
                    this._dbContext.Technical_Profile.Add(this._mapper.Map<TechnicalProfile>(domesticAndImportForm.TechnicalProfile));
                    this._dbContext.Subsideries.AddRange(this._mapper.Map<List<Subsideries>>(domesticAndImportForm.Subsideries));
                    this._dbContext.MajorCustomers.AddRange(this._mapper.Map<List<MajorCustomer>>(domesticAndImportForm.MajorCustomers));
                    this._dbContext.Commercial_Profile.Add(this._mapper.Map<CommercialProfile>(domesticAndImportForm.CommercialProfile));
                    this._dbContext.Bank_Details.Add(this._mapper.Map<Bank_Detail>(domesticAndImportForm.BankDetail));

                    var form = this._dbContext.Forms.Where(f => f.Form_Id == domesticAndImportForm.VendorPersonalData.Form_Id).FirstOrDefault();
                    form.Status_Id = (int)FormStatusEnum.Pending;
                    var workFlow = await this.UpdateWorkflow(formId, form.Created_By);



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
                            UserName = $"{toMail.First_Name} {toMail.Middle_Name} {toMail.Last_Name}".TrimEnd(),
                            Email = form.Vendor_Mail,
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

        public async Task<bool> UpdateDomesticAndImportVendorDetails(DomesticAndImportForm domesticForm, int formId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    this._dbContext.Vendor_Personal_Data.Update(this._mapper.Map<VendorPersonalData>(domesticForm.VendorPersonalData));
                    this._dbContext.Vendor_Organization_Profile.Update(this._mapper.Map<VendorOrganizationProfile>(domesticForm.VendorOrganizationProfile));
                    this._dbContext.Technical_Profile.Update(this._mapper.Map<TechnicalProfile>(domesticForm.TechnicalProfile));
                    this._dbContext.Commercial_Profile.Update(this._mapper.Map<CommercialProfile>(domesticForm.CommercialProfile));
                    this._dbContext.Bank_Details.Update(this._mapper.Map<Bank_Detail>(domesticForm.BankDetail));

                    await _updateFormRepo.UpdateNocilRelatedEmployees(this._mapper.Map<List<NocilRelatedEmployee>>(domesticForm.NocilRelatedEmployees), formId);
                    await _updateFormRepo.UpdateAddress(this._mapper.Map<List<Address>>(domesticForm.Addresses), formId);
                    await _updateFormRepo.UpdateContacts(this._mapper.Map<List<Contact>>(domesticForm.Contacts), formId);
                    await _updateFormRepo.UpdateVendorBranches(this._mapper.Map<List<VendorBranch>>(domesticForm.VendorBranches), formId);
                    await _updateFormRepo.UpdateProprietors(this._mapper.Map<List<ProprietorOrPartner>>(domesticForm.ProprietorOrPartners), formId);
                    await _updateFormRepo.UpdateAnnualTurnOvers(this._mapper.Map<List<AnnualTurnOver>>(domesticForm.AnnualTurnOvers), formId);
                    await _updateFormRepo.UpdateSubsideries(this._mapper.Map<List<Subsideries>>(domesticForm.Subsideries), formId);
                    await _updateFormRepo.UpdateMajorCustomers(this._mapper.Map<List<MajorCustomer>>(domesticForm.MajorCustomers), formId);

                    //var form = this._dbContext.Forms.Where(f => f.Form_Id == domesticForm.DomesticVendorPersonalData.Form_Id).FirstOrDefault();
                    var oldTask = this._dbContext.Tasks.Where(f => f.Form_Id == formId && f.Status == "Rejected").FirstOrDefault();
                    var workFlow = await this.UpdateWorkflow(formId, oldTask.Owner_Id, oldTask.Level);
                    var form = _dbContext.Forms.FirstOrDefault(x => x.Form_Id == formId);
                    form.Status_Id = (int)FormStatusEnum.Pending;
                    if (workFlow)
                    {
                        await this._dbContext.SaveChangesAsync();
                        var toMail = (from user in _dbContext.Users
                                      where user.Employee_Id == oldTask.Owner_Id
                                      select user).FirstOrDefault();

                        ApprovalMailInfo approval = new ApprovalMailInfo()
                        {
                            Form_Id = formId,
                            ToEmail = toMail!.Email,
                            UserName = $"{toMail.First_Name} {toMail.Middle_Name} {toMail.Last_Name}".TrimEnd(),
                            Email = form.Vendor_Mail,
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

        private async Task<bool> UpdateWorkflow(int formId, string Employee_Id, int level = 1)
        {

            Tasks task = new Tasks
            {
                Task_Id = 0,
                Form_Id = formId,
                Status = "Active",
                StartDate = DateTime.Now,
                Level = level,
                Owner_Id = Employee_Id,
            };
            await _dbContext.AddAsync(task);
            return true;
        }

    }
}
