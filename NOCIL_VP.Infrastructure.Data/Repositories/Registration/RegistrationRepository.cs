﻿using Newtonsoft.Json;
using NOCIL_VP.Domain.Core.Dtos;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos.Response;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Approval;
using NOCIL_VP.Domain.Core.Entities.Registration;
using NOCIL_VP.Infrastructure.Data.Enums;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration;
using NOCIL_VP.Domain.Core.Dtos.Dashboard;
using NOCIL_VP.Domain.Core.Dtos.Registration.Reason;
using NOCIL_VP.Domain.Core.Entities.Registration.CommonData;
using AutoMapper;
using NOCIL_VP.Domain.Core.Entities.Logs;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Registration
{
    public class RegistrationRepository : Repository<Form>, IRegistrationRepository
    {
        private readonly VpContext _dbContext;

        private readonly IDomesticAndImportRepository _domesticRepository;
        private readonly ITransportRepository _transportRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        private readonly EmailHelper _emailHelper;
        private readonly ModelValidator _validator;



        public RegistrationRepository(VpContext context,
            IDomesticAndImportRepository domesticRepository,
            ITransportRepository transportRepository,
            IServiceRepository serviceRepository,
            IMapper mapper,
            EmailHelper email) : base(context)
        {
            this._dbContext = context;
            this._domesticRepository = domesticRepository;
            this._transportRepository = transportRepository;
            this._serviceRepository = serviceRepository;
            this._validator = new ModelValidator();
            this._emailHelper = email;
            this._mapper = mapper;
        }

        public async Task<ResponseMessage> InitiateRegistration(FormDto formDto)
        {
            using (var transaction = this._dbContext.Database.BeginTransaction())
            {
                try
                {
                    var workFlow = (from user in _dbContext.Users
                                    where user.Employee_Id == formDto.CreatedBy
                                    join userRole in _dbContext.User_Role_Mappings on user.Employee_Id equals userRole.Employee_Id
                                    from wFlow in _dbContext.WorkFlows
                                    where wFlow.Vendor_Type_Id == formDto.Vendor_Type_Id && wFlow.Role_Id == userRole.Role_Id
                                    select wFlow).FirstOrDefault();
                    if (workFlow == null || workFlow.Level != 1)
                    {
                        throw new Exception("You do not have the access to initiate this form");
                    }
                    Form form = new Form
                    {
                        Form_Id = 0,
                        Vendor_Type_Id = formDto.Vendor_Type_Id,
                        Status_Id = (int)FormStatusEnum.Initiated,
                        Vendor_Code = formDto.Vendor_Code,
                        Vendor_Name = formDto.Vendor_Name,
                        Vendor_Mail = formDto.Vendor_Mail,
                        Vendor_Mobile = formDto.Vendor_Mobile,
                        Company_Code = formDto.Company_Code,
                        Department_Id = formDto.Department_Id,
                        Created_On = DateTime.Now,
                        Created_By = formDto.CreatedBy
                    };
                    var newForm = Add(form);
                    var saved = Save();

                    // Sending mail to Vendor functionality
                    var sendMail = new SendMail()
                    {
                        ToEmail = formDto.Vendor_Mail,
                        Username = formDto.Vendor_Name,
                        Form_Id = newForm.Form_Id,
                        Vendor_Type_Id = formDto.Vendor_Type_Id
                    };
                    await _emailHelper.SendMailToVendors(sendMail);

                    await transaction.CommitAsync();
                    transaction.Dispose();
                    return ResponseWritter.WriteSuccessResponse("Form initiated successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    transaction.Dispose();
                    throw ex;
                }
            }


        }

        public async Task<ResponseMessage> SubmitForm(FormSubmitTemplate formData)
        {
            var vpId = formData.Vendor_Type_Id;
            bool submitted = false;
            List<string> errors = new List<string>();
            var form = _dbContext.Forms.FirstOrDefault(x => x.Form_Id == formData.Form_Id);
            if (!(form.Status_Id == (int)FormStatusEnum.Initiated || form.Status_Id == (int)FormStatusEnum.Rejected))
            {
                throw new Exception("The form has been already submitted.");
            }

            switch (vpId)
            {
                case (int)VendorTypeEnum.DomesticRM:
                    DomesticAndImportForm domesticRmForm = JsonConvert.DeserializeObject<DomesticAndImportForm>(formData.FormData.ToString());
                    errors = this._validator.ValidateModel(domesticRmForm);
                    if (errors.Count == 0) { submitted = await _domesticRepository.SaveDomesticAndImportVendorDetails(domesticRmForm, formData.Form_Id); }
                    else { throw new ArgumentException(string.Join(", ", errors)); }
                    break;

                case (int)VendorTypeEnum.DomesticEngg:
                    DomesticAndImportForm domesticEnggForm = JsonConvert.DeserializeObject<DomesticAndImportForm>(formData.FormData.ToString());
                    errors = this._validator.ValidateModel(domesticEnggForm);
                    if (errors.Count == 0) { submitted = await _domesticRepository.SaveDomesticAndImportVendorDetails(domesticEnggForm, formData.Form_Id); }
                    else { throw new ArgumentException(string.Join(", ", errors)); }
                    break;

                case (int)VendorTypeEnum.Service:
                    ServiceForm serviceForm = JsonConvert.DeserializeObject<ServiceForm>(formData.FormData.ToString());
                    errors = this._validator.ValidateModel(serviceForm);
                    if (errors.Count == 0) { submitted = await _serviceRepository.SaveServiceVendorDetails(serviceForm, formData.Form_Id); }
                    else { throw new ArgumentException(string.Join(", ", errors)); }
                    break;

                case (int)VendorTypeEnum.Transport:
                    TransportForm transportForm = JsonConvert.DeserializeObject<TransportForm>(formData.FormData.ToString());
                    errors = this._validator.ValidateModel(transportForm);
                    if (errors.Count == 0) { submitted = await _transportRepository.SaveTransportVendorDetails(transportForm, formData.Form_Id); }
                    else { throw new ArgumentException(string.Join(", ", errors)); }
                    break;

                case (int)VendorTypeEnum.Import:
                    DomesticAndImportForm importForm = JsonConvert.DeserializeObject<DomesticAndImportForm>(formData.FormData.ToString());
                    errors = this._validator.ValidateModel(importForm);
                    if (errors.Count == 0) { submitted = await _domesticRepository.SaveDomesticAndImportVendorDetails(importForm, formData.Form_Id); }
                    else { throw new ArgumentException(string.Join(", ", errors)); }
                    break;

                default:
                    break;
            }

            if (submitted) return ResponseWritter.WriteSuccessResponse("Form Submitted Successfully");
            else throw new Exception("Workflow for this vendor type is not defined");
        }

        public async Task<ResponseMessage> ApproveForm(ApprovalDto approvalDto)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var workFlow = (from tb in _dbContext.WorkFlows
                                    where tb.Vendor_Type_Id == approvalDto.VendorTypeId && tb.Role_Id == approvalDto.RoleId
                                    from tb1 in _dbContext.WorkFlows
                                    where tb.GroupId == tb1.GroupId
                                    select tb1).OrderByDescending(x => x.Level).ToList();

                    // Update the old task
                    var oldTask = this._dbContext.Tasks.
                                FirstOrDefault(x => x.Owner_Id == approvalDto.EmployeeId && x.Form_Id == approvalDto.Form_Id && x.Status == "Active");
                    if (oldTask == null)
                    {
                        throw new Exception("Unable to find the task");
                    }

                    var maxLevl = workFlow.Select(x => x.Level).Max();

                    oldTask.Status = "Completed";
                    oldTask.EndDate = DateTime.Now;


                    if (oldTask.Level < maxLevl)
                    {

                        if (oldTask.Level == 1)
                        {
                            if (approvalDto.AdditionalFields == null)
                            {
                                throw new Exception("Additional Fields are required ");
                            }

                            AdditionalFields additionalFields = this._mapper.Map<AdditionalFields>(approvalDto.AdditionalFields);
                            _dbContext.AdditionalFields.Add(additionalFields);
                        }

                        if (workFlow.FirstOrDefault(x => x.Level == oldTask.Level + 1).Role_Id == approvalDto.RmRoleId)
                        {
                            // Create Database entries
                            Tasks task = new Tasks
                            {
                                Task_Id = 0,
                                Form_Id = approvalDto.Form_Id,
                                Owner_Id = approvalDto.RmEmployeeId,
                                Level = oldTask.Level + 1,
                                StartDate = DateTime.Now,
                                Status = "Active",
                                Role_Id = approvalDto.RmRoleId
                            };
                            TransactionHistory history = new TransactionHistory
                            {
                                Log_Id = 0,
                                Form_Id = approvalDto.Form_Id,
                                Logged_Date = DateTime.Now,
                                Message = $"Form {approvalDto.Form_Id} is approved by {approvalDto.EmployeeId}",
                            };

                            // Add to database
                            _dbContext.Tasks.Add(task);
                            _dbContext.TransactionHistories.Add(history);
                            await _dbContext.SaveChangesAsync();


                            // Sending Mails
                            var forms = this._dbContext.Forms.FirstOrDefault(x => x.Form_Id == approvalDto.Form_Id);
                            var toMail = (from users in _dbContext.Users
                                          where users.Employee_Id == approvalDto.EmployeeId
                                          join urMap in _dbContext.User_Role_Mappings on users.Employee_Id equals urMap.Employee_Id
                                          join role in _dbContext.Roles on urMap.Role_Id equals role.Role_Id
                                          from user in _dbContext.Users
                                          where user.Employee_Id == users.Reporting_Manager_EmpId
                                          select new
                                          {
                                              ToEmail = user.Email,
                                              Name = $"{user.First_Name} {user.Middle_Name} {user.Last_Name}".TrimEnd(),
                                              ApprovedBy = role.Role_Name
                                          }).FirstOrDefault();

                            ApprovalMailInfo approval = new ApprovalMailInfo()
                            {
                                Form_Id = approvalDto.Form_Id,
                                ToEmail = toMail!.ToEmail,
                                Email = forms.Vendor_Mail,
                                UserName = toMail.Name,
                                Mobile_Number = forms.Vendor_Mobile
                            };
                            ApprovalInfo approvalInfo = new ApprovalInfo()
                            {
                                UserName = forms.Vendor_Name,
                                ToEmail = forms.Vendor_Mail,
                                ApprovedBy = toMail.ApprovedBy
                            };

                            await this._emailHelper.SendApprovalRequestMail(approval);
                            await this._emailHelper.SendApprovalInfoMail(approvalInfo);

                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            transaction.Dispose();
                            throw new Exception("Reporting manager not mapped correctly");
                        }

                    }
                    else if (oldTask.Level == maxLevl)
                    {
                        // Add and Update entries
                        Tasks task = new Tasks
                        {
                            Task_Id = 0,
                            Form_Id = approvalDto.Form_Id,
                            Owner_Id = null,
                            Level = oldTask.Level + 1,
                            StartDate = DateTime.Now,
                            Status = "SAP"
                        };
                        List<TransactionHistory> histories = new List<TransactionHistory>()
                        {
                            new TransactionHistory{ Log_Id = 0,Form_Id = approvalDto.Form_Id,Logged_Date = DateTime.Now, Message = $"Form {approvalDto.Form_Id} is approved by {approvalDto.EmployeeId}"},
                            new TransactionHistory { Log_Id = 0,Form_Id = approvalDto.Form_Id,Logged_Date = DateTime.Now, Message = $"Form {approvalDto.Form_Id} is moved to SAP"}
                        };
                        var form = this._dbContext.Forms.FirstOrDefault(x => x.Form_Id == approvalDto.Form_Id);
                        form.Status_Id = (int)FormStatusEnum.SAP;

                        // Save Changes
                        _dbContext.Tasks.Add(task);
                        _dbContext.TransactionHistories.AddRange(histories);
                        await _dbContext.SaveChangesAsync();

                        // Send mail
                        var approvedBy = (from user in _dbContext.Users
                                          where user.Employee_Id == approvalDto.EmployeeId
                                          join urMap in _dbContext.User_Role_Mappings on user.Employee_Id equals urMap.Employee_Id
                                          join role in _dbContext.Roles on urMap.Role_Id equals role.Role_Id
                                          select role).FirstOrDefault();

                        var approval = new ApprovalInfo()
                        {
                            ToEmail = form.Vendor_Mail,
                            ApprovedBy = approvedBy.Role_Name,
                            UserName = form.Vendor_Name,
                        };
                        await this._emailHelper.SendApprovalInfoMail(approval);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        transaction.Dispose();
                        throw new Exception("Unable to find the approval matrix");
                    }
                    await transaction.CommitAsync();
                    transaction.Dispose();

                    return ResponseWritter.WriteSuccessResponse($"Form {approvalDto.Form_Id} is approved by {approvalDto.EmployeeId}");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }

        public SAPVendorCreationPayload SAPCreationPayload(int Form_Id)
        {
            var sapPayload = (from form in _dbContext.Forms
                              where form.Form_Id == Form_Id
                              join formId in _dbContext.Vendor_Personal_Data on form.Form_Id equals formId.Form_Id
                              join gstVenCLass in _dbContext.GSTVenClass on formId.GSTVenClass_Id equals gstVenCLass.Id
                              join title in _dbContext.Titles on formId.Title_Id equals title.Id
                              join organization in _dbContext.Vendor_Personal_Data on title.Id equals organization.Title_Id
                              join searchTerm in _dbContext.AdditionalFields on form.Form_Id equals searchTerm.Form_Id
                              join address in _dbContext.Addresses on form.Form_Id equals address.Form_Id
                              join contact in _dbContext.Contacts on form.Form_Id equals contact.Form_Id
                              join panNumber in _dbContext.Commercial_Profile on form.Form_Id equals panNumber.Form_Id
                              join industry in _dbContext.Industry on searchTerm.Industry_Id equals industry.Id
                              join incoterms in _dbContext.Incoterms on searchTerm.Incoterms_Id equals incoterms.Id
                              join reconciliation in _dbContext.ReconciliationAccounts on searchTerm.Reconciliation_Id equals reconciliation.Id
                              join schema in _dbContext.SchemaGroups on searchTerm.Schema_Id equals schema.Id
                              join initiator in _dbContext.Users on form.Created_By equals initiator.Employee_Id
                              select new SAPVendorCreationPayload
                              {
                                  Company_code = form.CompanyCode.Company_Code,
                                  Purchasing_org = searchTerm.PO_Code,
                                  Title = title.Title_Name,
                                  Name = organization.Organization_Name,
                                  Search_term = searchTerm.Search_Term,
                                  Street_House_number = address.House_No,
                                  Street_2 = address.Street_2,
                                  Street_3 = address.Street_3,
                                  Street_4 = address.Street_4,
                                  District = address.District,
                                  Postal_Code = address.Postal_Code,
                                  City = address.City,
                                  Country = address.Country.Code,
                                  Region = address.Region.Code,
                                  Language = searchTerm.Language,
                                  Telephone = address.Tel,
                                  Fax = address.Fax,
                                  Mobile_Phone = contact.Mobile_Number,
                                  E_Mail = contact.Email_Id,
                                  Tax_Number_3 = panNumber.GSTIN,
                                  Industry = industry.Code,
                                  Initiators_name = $"{initiator.First_Name} {initiator.Middle_Name} {initiator.Last_Name}".TrimEnd(),
                                  Pan_Number = panNumber.PAN,
                                  GST_Ven_Class = gstVenCLass.Code,
                                  First_name = $"{initiator.First_Name} {initiator.Middle_Name} {initiator.Last_Name}".TrimEnd(),
                                  Recon_account = reconciliation.Code,
                                  Order_currency = searchTerm.Order_Currency,
                                  Incoterms = incoterms.Code,
                                  Incoterms_Text = incoterms.Description,
                                  Schema_Group_Vendor = schema.Code,
                                  GR_based_Inv_Verif = searchTerm.GrBased,
                                  SRV_based_Inv_Verif = searchTerm.SrvBased

                              }).FirstOrDefault();

            return sapPayload;
        }

        public async Task<ResponseMessage> RejectForm(RejectDto rejectDto)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var oldTask = _dbContext.Tasks.Where(x => x.Form_Id == rejectDto.Form_Id && x.Owner_Id == rejectDto.Employee_Id && x.Status == "Active").FirstOrDefault();
                    if (oldTask != null)
                    {
                        oldTask.Status = "Rejected";
                        oldTask.EndDate = DateTime.Now;
                        oldTask.Message = rejectDto.Reason;
                    }

                    var formData = this._dbContext.Forms.FirstOrDefault(x => x.Form_Id == rejectDto.Form_Id);
                    formData.Status_Id = (int)FormStatusEnum.Rejected;

                    TransactionHistory history = new TransactionHistory
                    {
                        Log_Id = 0,
                        Form_Id = rejectDto.Form_Id,
                        Logged_Date = DateTime.Now,
                        Message = rejectDto.Reason
                    };

                    _dbContext.TransactionHistories.Add(history);


                    DashboardDto formDetails = (from form in _dbContext.Forms
                                                where form.Form_Id == rejectDto.Form_Id
                                                select new DashboardDto
                                                {
                                                    FormId = form.Form_Id,
                                                    VendorTypeId = form.Vendor_Type_Id,
                                                    VendorType = form.VendorType.Vendor_Type,
                                                    Email = form.Vendor_Mail,
                                                    Mobile = form.Vendor_Mobile,
                                                    Name = form.Vendor_Name,
                                                    CreatedOn = form.Created_On,
                                                    Status = form.FormStatus.Status
                                                }).FirstOrDefault();

                    // Send mail to vendor with proper reject reason code
                    var rejectMailDto = new RejectionMailInfo()
                    {
                        Form_Id = rejectDto.Form_Id,
                        Reason = rejectDto.Reason,
                        Vendor_Type_Id = formDetails.VendorTypeId,
                        ToEmail = formDetails.Email,
                        Username = formDetails.Name
                    };
                    await this._emailHelper.SendRejectionInfoMail(rejectMailDto);

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    transaction.Dispose();

                    return ResponseWritter.WriteSuccessResponse("Form Rejected Successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }

        public async Task<ResponseMessage> UpdateForm(FormSubmitTemplate formData)
        {
            var vpId = formData.Vendor_Type_Id;
            bool submitted = false;
            List<string> errors = new List<string>();

            switch (vpId)
            {
                case 1:
                    DomesticAndImportForm domesticForm = JsonConvert.DeserializeObject<DomesticAndImportForm>(formData.FormData.ToString());
                    errors = this._validator.ValidateModel(domesticForm);
                    if (errors.Count == 0) { submitted = await _domesticRepository.UpdateDomesticAndImportVendorDetails(domesticForm, formData.Form_Id); }
                    else { throw new ArgumentException(string.Join(", ", errors)); }
                    break;
                case 2:
                    ServiceForm serviceForm = JsonConvert.DeserializeObject<ServiceForm>(formData.FormData.ToString());
                    errors = this._validator.ValidateModel(serviceForm);
                    if (errors.Count == 0) { submitted = await _serviceRepository.UpdateServiceVendorDetails(serviceForm, formData.Form_Id); }
                    else { throw new ArgumentException(string.Join(", ", errors)); }
                    break;
                case 3:
                    TransportForm transportForm = JsonConvert.DeserializeObject<TransportForm>(formData.FormData.ToString());
                    errors = this._validator.ValidateModel(transportForm);
                    if (errors.Count == 0) { submitted = await _transportRepository.UpdateTransportVendorDetails(transportForm, formData.Form_Id); }
                    else { throw new ArgumentException(string.Join(", ", errors)); }
                    break;
                case 4:
                    DomesticAndImportForm importForm = JsonConvert.DeserializeObject<DomesticAndImportForm>(formData.FormData.ToString());
                    errors = this._validator.ValidateModel(importForm);
                    if (errors.Count == 0) { submitted = await _domesticRepository.UpdateDomesticAndImportVendorDetails(importForm, formData.Form_Id); }
                    else { throw new ArgumentException(string.Join(", ", errors)); }
                    break;
                default:
                    break;
            }

            if (submitted) return ResponseWritter.WriteSuccessResponse("Form Upated Successfully");
            else throw new Exception("Workflow for this vendor type is not defined");
        }

        public DashboardDto GetSingleFormData(int form_Id)
        {
            var res = (from form in _dbContext.Forms
                       where form.Form_Id == form_Id
                       select new DashboardDto
                       {
                           FormId = form.Form_Id,
                           VendorTypeId = form.Vendor_Type_Id,
                           VendorType = form.VendorType.Vendor_Type,
                           Email = form.Vendor_Mail,
                           Mobile = form.Vendor_Mobile,
                           Name = form.Vendor_Name,
                           CreatedOn = form.Created_On,
                           Status = form.FormStatus.Status
                       }).FirstOrDefault();
            return res;
        }

        public ReasonDto GetRejectedReasons(int form_Id)
        {
            ReasonDto response = new ReasonDto();
            var res = (from task in _dbContext.Tasks
                       where task.Form_Id == form_Id && task.Status == "Rejected"
                       join user in _dbContext.Users on task.Owner_Id equals user.Employee_Id
                       orderby task.Task_Id
                       select new ReasonDetailDto
                       {
                           RejectedBy = $"{user.First_Name} {user.Middle_Name} {user.Last_Name}".TrimEnd(),
                           RejectedOn = task.EndDate,
                           Reason = task.Message
                       }).ToList();
            response.Reasons = res;
            response.IsRejected = res.Count > 0 ? true : false;
            return response;
        }
    }
}
