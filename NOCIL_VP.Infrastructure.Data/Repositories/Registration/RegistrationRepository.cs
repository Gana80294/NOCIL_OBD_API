using AutoMapper;
using Newtonsoft.Json;
using NOCIL_VP.Domain.Core.Dtos;
using NOCIL_VP.Domain.Core.Dtos.Dashboard;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos.Registration.Reason;
using NOCIL_VP.Domain.Core.Dtos.Response;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Domain.Core.Entities.Approval;
using NOCIL_VP.Domain.Core.Entities.Logs;
using NOCIL_VP.Domain.Core.Entities.Registration;
using NOCIL_VP.Domain.Core.Entities.Registration.CommonData;
using NOCIL_VP.Infrastructure.Data.Enums;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration;
using Microsoft.EntityFrameworkCore;
using NOCIL_VP.Domain.Core.Configurations;
using Microsoft.Extensions.Options;
using NOCIL_VP.API.Logging;
using System.Threading.Tasks.Dataflow;
using NOCIL_VP.Domain.Core.Dtos.Master;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Master;
using SAP_VENDOR_CREATE_SERVICE;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Registration
{
    public class RegistrationRepository : Repository<Form>, IRegistrationRepository
    {
        private readonly VpContext _dbContext;

        private readonly IDomesticAndImportRepository _domesticRepository;
        private readonly ITransportRepository _transportRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly AppSetting _appSetting;

        private readonly EmailHelper _emailHelper;
        private readonly ModelValidator _validator;
        private readonly VendorService _vendorService;


        public RegistrationRepository(VpContext context,
            IDomesticAndImportRepository domesticRepository,
            ITransportRepository transportRepository,
            IServiceRepository serviceRepository,
            IUserRepository userRepository,
            IMapper mapper,
            EmailHelper email, IOptions<AppSetting> opt, VendorService vendorService) : base(context)
        {
            this._dbContext = context;
            this._domesticRepository = domesticRepository;
            this._transportRepository = transportRepository;
            this._serviceRepository = serviceRepository;
            this._userRepository = userRepository;
            this._validator = new ModelValidator();
            this._emailHelper = email;
            this._mapper = mapper;
            this._appSetting = opt.Value;
            _vendorService = vendorService;
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
                    DomesticAndImportForm domesticForm1 = JsonConvert.DeserializeObject<DomesticAndImportForm>(formData.FormData.ToString());
                    errors = this._validator.ValidateModel(domesticForm1);
                    if (errors.Count == 0) { submitted = await _domesticRepository.UpdateDomesticAndImportVendorDetails(domesticForm1, formData.Form_Id); }
                    else { throw new ArgumentException(string.Join(", ", errors)); }
                    break;
                case 3:
                    ServiceForm serviceForm = JsonConvert.DeserializeObject<ServiceForm>(formData.FormData.ToString());
                    errors = this._validator.ValidateModel(serviceForm);
                    if (errors.Count == 0) { submitted = await _serviceRepository.UpdateServiceVendorDetails(serviceForm, formData.Form_Id); }
                    else { throw new ArgumentException(string.Join(", ", errors)); }
                    break;
                case 4:
                    TransportForm transportForm = JsonConvert.DeserializeObject<TransportForm>(formData.FormData.ToString());
                    errors = this._validator.ValidateModel(transportForm);
                    if (errors.Count == 0) { submitted = await _transportRepository.UpdateTransportVendorDetails(transportForm, formData.Form_Id); }
                    else { throw new ArgumentException(string.Join(", ", errors)); }
                    break;
                case 5:
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
                       join joinuser in _dbContext.Users on task.Owner_Id equals joinuser.Employee_Id into users
                       from user in users.DefaultIfEmpty()
                       orderby task.Task_Id
                       select new ReasonDetailDto
                       {
                           RejectedBy = string.IsNullOrEmpty(task.Owner_Id) ? "SAP" : $"{user.First_Name} {user.Middle_Name} {user.Last_Name}".TrimEnd(),
                           RejectedOn = task.EndDate,
                           Reason = task.Message
                       }).ToList();
            response.Reasons = res;
            response.IsRejected = res.Count > 0 ? true : false;
            return response;
        }


        #region Rejection Region

        public async Task<ResponseMessage> RejectForm(RejectDto rejectDto)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var oldTask = _dbContext.Tasks
                                  .Include(x => x.User) 
                                  .FirstOrDefault(x => x.Form_Id == rejectDto.Form_Id &&
                                    x.Owner_Id == rejectDto.Employee_Id &&
                                    x.Status == "Active");
                    //var oldTask = _dbContext.Tasks.Where(x => x.Form_Id == rejectDto.Form_Id && x.Owner_Id == rejectDto.Employee_Id && x.Status == "Active").FirstOrDefault();
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

                    var rejectToVendor = await GetRejectionMailInfoToVendor(rejectDto);
                    var rejectToBuyer = await GetRejectionMailInfoToBuyer(oldTask, rejectDto);
                    await this._emailHelper.SendRejectionInfoMailToVendor(rejectToVendor);
                    await this._emailHelper.SendRejectionMailInfoToBuyers(rejectToBuyer);

                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    transaction.Dispose();

                    return ResponseWritter.WriteSuccessResponse("Form Rejected Successfully");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    transaction.Dispose();
                    throw;
                }
            }
        }

        public async Task<RejectionMailInfoToVendor> GetRejectionMailInfoToVendor(RejectDto rejectDto)
        {
            DashboardDto formDetails = await (from form in _dbContext.Forms
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
                                              }).FirstOrDefaultAsync();

            var rejectMailDto = new RejectionMailInfoToVendor()
            {
                Form_Id = rejectDto.Form_Id,
                Reason = rejectDto.Reason,
                Vendor_Type_Id = formDetails.VendorTypeId,
                ToEmail = formDetails.Email,
                Username = formDetails.Name
            };

            return rejectMailDto;
        }

        public async Task<List<RejectionMailInfoToBuyer>> GetRejectionMailInfoToBuyer(Tasks oldTask, RejectDto rejectDto)
        {
            var rejectedBy = $"{oldTask.User.First_Name} {oldTask.User.Middle_Name} {oldTask.User.Last_Name}";
            var res1 = await (from form in _dbContext.Forms
                              where form.Form_Id == rejectDto.Form_Id
                              join task in _dbContext.Tasks on form.Form_Id equals task.Form_Id
                              select new
                              {
                                  TaskId = task.Task_Id,
                                  Form_Id=task.Forms.Form_Id,
                                  EmployeeId = task.Owner_Id,
                                  Recepient = task.Owner_Id != null ? task.User.First_Name : "",
                                  RoleId = task.Role_Id,
                                  Level = task.Level,
                                  ToEmail = task.User.Email,
                                  VendorMail = task.Forms.Vendor_Mail,
                                  VendorMobile = task.Forms.Vendor_Mobile,
                                  VendorName = task.Forms.Vendor_Name
                              }).OrderByDescending(x => x.TaskId).ToListAsync();

            var res2 = new List<dynamic>();

            foreach (var r in res1)
            {
                if (r.Level == 1)
                {
                    res2.Add(r);
                    break;
                }
                if (r.Level != oldTask.Level) res2.Add(r);
            }

            var recepients = (from r in res2
                              select new RejectionMailInfoToBuyer
                              {
                                  Form_Id = r.Form_Id,
                                  Reason = rejectDto.Reason,
                                  Recepient = r.Recepient,
                                  RejectedBy = rejectedBy,
                                  ToEmail = r.ToEmail,
                                  VendorMail = r.VendorMail,
                                  VendorMobile = r.VendorMobile,
                                  VendorName = r.VendorName
                              }).Distinct().ToList();

            return recepients;
        }

        #endregion

        #region Approval Region

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
                            //await this._emailHelper.SendApprovalInfoMail(approvalInfo);

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
                        Tasks sapTask = new Tasks
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
                        _dbContext.Tasks.Add(sapTask);
                        _dbContext.TransactionHistories.AddRange(histories);
                        await _dbContext.SaveChangesAsync();
                        try
                        {
                            // Call SAP functions
                            //var sapResponse = await CallSAPCreateVendor(approvalDto.Form_Id);
                            //ZmmRfcVendorCreateResponse sapVendorCodeResponse = _soapParser.ParseSoapResponse(sapResponse);

                            ZMM_RFC_VENDOR_CREATEResponse sapVendorCodeResponse = await GetVendorCreateResponse(approvalDto.Form_Id);

                            if (sapVendorCodeResponse != null && sapVendorCodeResponse.EX_VENDOR_DATA.Length > 0)
                            {
                                if (string.IsNullOrEmpty(sapVendorCodeResponse.EX_VENDOR_DATA[0].VENDOR_CODE))
                                {
                                    throw new Exception("Vendor code not generated in SAP. " + sapVendorCodeResponse.EX_VENDOR_DATA[0].MESSAGE);
                                }
                                else
                                {
                                    form.Vendor_Code = sapVendorCodeResponse.EX_VENDOR_DATA[0].VENDOR_CODE;
                                    form.Status_Id = (int)FormStatusEnum.Approved;

                                    var role = this._dbContext.Roles.FirstOrDefault(x => x.Role_Name.ToLower() == "vendor");
                                    var user = new UserDto()
                                    {
                                        Employee_Id = sapVendorCodeResponse.EX_VENDOR_DATA[0].VENDOR_CODE,
                                        First_Name = sapVendorCodeResponse.EX_VENDOR_DATA[0].NAME1,
                                        Middle_Name = sapVendorCodeResponse.EX_VENDOR_DATA[0].NAME2,
                                        Last_Name = sapVendorCodeResponse.EX_VENDOR_DATA[0].NAME3,
                                        Email = form.Vendor_Mail,
                                        Mobile_No = form.Vendor_Mobile,
                                        IsActive = true,
                                        Role_Id = role != null ? role.Role_Id : 10,
                                        Reporting_Manager_EmpId = null
                                    };
                                    await this._userRepository.AddUser(user);
                                    await _dbContext.SaveChangesAsync();

                                    await SendSuccessVendorCreationMails(form, sapVendorCodeResponse);
                                }
                            }
                            else
                            {
                                throw new Exception("SAP Response is Empty");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error during SAP Vendor creation: " + ex.Message, ex);
                        }


                        // Send mail
                        //var approvedBy = (from user in _dbContext.Users
                        //                  where user.Employee_Id == approvalDto.EmployeeId
                        //                  join urMap in _dbContext.User_Role_Mappings on user.Employee_Id equals urMap.Employee_Id
                        //                  join role in _dbContext.Roles on urMap.Role_Id equals role.Role_Id
                        //                  select role).FirstOrDefault();

                        //var approval = new ApprovalInfo()
                        //{
                        //    ToEmail = form.Vendor_Mail,
                        //    ApprovedBy = approvedBy.Role_Name,
                        //    UserName = form.Vendor_Name,
                        //};
                        //await this._emailHelper.SendApprovalInfoMail(approval);
                    }
                    else
                    {
                        throw new Exception("Unable to find the approval matrix");
                    }
                    await transaction.CommitAsync();
                    transaction.Dispose();

                    return ResponseWritter.WriteSuccessResponse($"Form {approvalDto.Form_Id} is approved by {approvalDto.EmployeeId}");
                }
                catch (Exception)
                {

                    await transaction.RollbackAsync();
                    transaction.Dispose();
                    throw;
                }
            }
        }

        public async Task<string> CallSAPCreateVendor(int formId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, _appSetting.SapVendorCreate);
            request.Headers.Add("SOAPAction", "urn:sap-com:document:sap:soap:functions:mc-style:ZVENDOR_CREATE:ZmmRfcVendorCreateRequest");
            var payload = GetCompleteFormDetailsForSAP(formId);
            var content = new StringContent(GenerateVendorSoapPayload(payload));
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            LogWritter.WriteErrorLog(new string('*', 10) + "XML RESPONSE START" + new string('*', 10));
            LogWritter.WriteErrorLog(responseString);
            LogWritter.WriteErrorLog(new string('*', 10) + "XML RESPONSE END" + new string('*', 10));
            return responseString;

        }
        public SAPVendorCreationPayload GetCompleteFormDetailsForSAP(int Form_Id)
        {
            var formData = this._dbContext.Forms.FirstOrDefault(x => x.Form_Id == Form_Id);
            SAPVendorCreationPayload payload = new SAPVendorCreationPayload();
            switch (formData.Vendor_Type_Id)
            {
                case 1:
                    payload = GetNonTransporter(Form_Id);
                    break;
                case 2:
                    payload = GetNonTransporter(Form_Id);
                    break;
                case 3:
                    payload = GetNonTransporter(Form_Id);
                    break;
                case 4:
                    payload = GetTransportVendor(Form_Id);
                    break;
                case 5:
                    payload = GetNonTransporter(Form_Id);
                    break;
                default:
                    break;
            }
            return payload;

        }
        public SAPVendorCreationPayload GetNonTransporter(int Form_Id)
        {
            var sapPayloadDto = _dbContext.Forms
                .Where(f => f.Form_Id == Form_Id)
                .Include(c => c.CompanyCode)
                .Select(f => new
                {
                    Form = f,
                    VendorPersonalData = _dbContext.Vendor_Personal_Data.Where(vpd => vpd.Form_Id == f.Form_Id)
                    .Include(x => x.GSTVenClass)
                    .Include(x => x.Title).FirstOrDefault(),
                    Addresses = _dbContext.Addresses.Where(x => x.Form_Id == f.Form_Id)
                    .Include(x => x.AddressTypes)
                    .Include(x => x.Country)
                    .Include(x => x.Region).ToList(),
                    Contacts = _dbContext.Contacts.Where(x => x.Form_Id == f.Form_Id)
                    .Include(x => x.ContactTypes).ToList(),
                    CommercialProfile = _dbContext.Commercial_Profile.FirstOrDefault(x => x.Form_Id == f.Form_Id),
                    AdditionalFields = _dbContext.AdditionalFields.Where(x => x.Form_Id == f.Form_Id)
                    .Include(x => x.Incoterms)
                    .Include(x => x.Industry)
                    .Include(x => x.VendorAccountGroup)
                    .Include(x => x.ReconciliationAccount)
                    .Include(x => x.PurchaseOrganization)
                    .Include(x => x.SchemaGroup).FirstOrDefault(),
                    Initiator = _dbContext.Users.FirstOrDefault(x => x.Employee_Id == f.Created_By)
                })
                .AsEnumerable()
                .Select(x => new SAPVendorCreationDto
                {
                    Company_code = x.Form.CompanyCode.Company_Code,
                    Purchasing_org = x.AdditionalFields.PO_Code,
                    Account_grp = x.AdditionalFields.VendorAccountGroup.Code,
                    Title = x.VendorPersonalData.Title.Title_Name,
                    Name1 = x.VendorPersonalData.Organization_Name,
                    Name2 = "",
                    Name3 = "",
                    Search_term = x.AdditionalFields.Search_Term,
                    Addresses = x.Addresses,
                    Language = x.AdditionalFields.Language,
                    Contacts = x.Contacts,
                    Tax_Number_3 = x.CommercialProfile.GSTIN,
                    Industry = x.AdditionalFields.Industry.Code,
                    Initiators_name = $"{x.Initiator.First_Name} {x.Initiator.Middle_Name} {x.Initiator.Last_Name}".TrimEnd(),
                    Pan_Number = x.CommercialProfile.PAN,
                    GST_Ven_Class = x.VendorPersonalData.GSTVenClass.Code,
                    Recon_account = x.AdditionalFields.ReconciliationAccount.Code,
                    Order_currency = x.AdditionalFields.Order_Currency,
                    Incoterms = x.AdditionalFields.Incoterms.Code,
                    Incoterms_Text = "",
                    Schema_Group_Vendor = x.AdditionalFields.SchemaGroup.Code,
                    GR_based_Inv_Verif = x.AdditionalFields.GrBased,
                    SRV_based_Inv_Verif = x.AdditionalFields.SrvBased
                }).FirstOrDefault();


            var singleContact = GetContactDetail(sapPayloadDto.Contacts);
            var singleAddress = GetAddressDetail(sapPayloadDto.Addresses);

            var sapPayload = new SAPVendorCreationPayload()
            {
                Company_code = sapPayloadDto.Company_code,
                Purchasing_org = sapPayloadDto.Purchasing_org,
                Title = sapPayloadDto.Title,
                Account_grp = sapPayloadDto.Account_grp,
                Name1 = sapPayloadDto.Name1,
                Name2 = "",
                Name3 = "",
                Search_term = sapPayloadDto.Search_term,
                Street_House_number = singleAddress.House_No,
                Street_2 = singleAddress.Street_2,
                Street_3 = singleAddress.Street_3,
                Street_4 = singleAddress.Street_4,
                District = singleAddress.District,
                Postal_Code = singleAddress.Postal_Code,
                City = singleAddress.City,
                Country = singleAddress.Country.Code,
                Region = singleAddress.Region.Name,
                Telephone = singleAddress.Tel,
                Fax = singleAddress.Fax,
                Mobile_Phone = singleContact.Mobile_Number,
                E_Mail = singleContact.Email_Id,
                Contact_person = singleContact.Name,
                First_name = singleContact.Name,
                Language = sapPayloadDto.Language,
                Tax_Number_3 = sapPayloadDto.Tax_Number_3,
                Industry = sapPayloadDto.Industry,
                Initiators_name = sapPayloadDto.Initiators_name,
                Pan_Number = sapPayloadDto.Pan_Number,
                GST_Ven_Class = sapPayloadDto.GST_Ven_Class,
                Recon_account = sapPayloadDto.Recon_account,
                Order_currency = sapPayloadDto.Order_currency,
                Incoterms = sapPayloadDto.Incoterms,
                Incoterms_Text = singleAddress.City,
                Schema_Group_Vendor = sapPayloadDto.Schema_Group_Vendor,
                GR_based_Inv_Verif = sapPayloadDto.GR_based_Inv_Verif,
                SRV_based_Inv_Verif = sapPayloadDto.SRV_based_Inv_Verif

            };

            OrganizationNameForSAPPayload(sapPayload);
            return sapPayload;
        }
        public SAPVendorCreationPayload GetTransportVendor(int Form_Id)
        {
            var sapPayloadDto = _dbContext.Forms
                .Where(f => f.Form_Id == Form_Id)
                .Include(c => c.CompanyCode)
                .Select(f => new
                {
                    Form = f,
                    VendorPersonalData = _dbContext.Transport_Vendor_Personal_Data.Where(vpd => vpd.Form_Id == f.Form_Id)
                    .Include(x => x.GSTVenClass)
                    .Include(x => x.Title).FirstOrDefault(),
                    Addresses = _dbContext.Addresses.Where(x => x.Form_Id == f.Form_Id)
                    .Include(x => x.AddressTypes)
                    .Include(x => x.Country)
                    .Include(x => x.Region).ToList(),
                    Contacts = _dbContext.Contacts.Where(x => x.Form_Id == f.Form_Id)
                    .Include(x => x.ContactTypes).ToList(),
                    CommercialProfile = _dbContext.Commercial_Profile.FirstOrDefault(x => x.Form_Id == f.Form_Id),
                    AdditionalFields = _dbContext.AdditionalFields.Where(x => x.Form_Id == f.Form_Id)
                    .Include(x => x.Incoterms)
                    .Include(x => x.Industry)
                    .Include(x => x.VendorAccountGroup)
                    .Include(x => x.ReconciliationAccount)
                    .Include(x => x.PurchaseOrganization)
                    .Include(x => x.SchemaGroup).FirstOrDefault(),
                    Initiator = _dbContext.Users.FirstOrDefault(x => x.Employee_Id == f.Created_By)
                })
                .AsEnumerable()
                .Select(x => new SAPVendorCreationDto
                {
                    Company_code = x.Form.CompanyCode.Company_Code,
                    Purchasing_org = x.AdditionalFields.PO_Code,
                    Account_grp = x.AdditionalFields.VendorAccountGroup.Code,
                    Title = x.VendorPersonalData.Title.Title_Name,
                    Name1 = x.VendorPersonalData.Name_of_Transporter,
                    Name2 = "",
                    Name3 = "",
                    Search_term = x.AdditionalFields.Search_Term,
                    Addresses = x.Addresses,
                    Language = x.AdditionalFields.Language,
                    Contacts = x.Contacts,
                    Tax_Number_3 = x.CommercialProfile.GSTIN,
                    Industry = x.AdditionalFields.Industry.Code,
                    Initiators_name = $"{x.Initiator.First_Name} {x.Initiator.Middle_Name} {x.Initiator.Last_Name}".TrimEnd(),
                    Pan_Number = x.CommercialProfile.PAN,
                    GST_Ven_Class = x.VendorPersonalData.GSTVenClass.Code,
                    Recon_account = x.AdditionalFields.ReconciliationAccount.Code,
                    Order_currency = x.AdditionalFields.Order_Currency,
                    Incoterms = x.AdditionalFields.Incoterms.Code,
                    Incoterms_Text = "",
                    Schema_Group_Vendor = x.AdditionalFields.SchemaGroup.Code,
                    GR_based_Inv_Verif = x.AdditionalFields.GrBased,
                    SRV_based_Inv_Verif = x.AdditionalFields.SrvBased
                }).FirstOrDefault();


            var singleContact = GetContactDetail(sapPayloadDto.Contacts);
            var singleAddress = GetAddressDetail(sapPayloadDto.Addresses);

            var sapPayload = new SAPVendorCreationPayload()
            {
                Company_code = sapPayloadDto.Company_code,
                Purchasing_org = sapPayloadDto.Purchasing_org,
                Title = sapPayloadDto.Title,
                Account_grp = sapPayloadDto.Account_grp,
                Name1 = sapPayloadDto.Name1,
                Name2 = "",
                Name3 = "",
                Search_term = sapPayloadDto.Search_term,
                Street_House_number = singleAddress.House_No,
                Street_2 = singleAddress.Street_2,
                Street_3 = singleAddress.Street_3,
                Street_4 = singleAddress.Street_4,
                District = singleAddress.District,
                Postal_Code = singleAddress.Postal_Code,
                City = singleAddress.City,
                Country = singleAddress.Country.Code,
                Region = singleAddress.Region.Name,
                Telephone = singleAddress.Tel,
                Fax = singleAddress.Fax,
                Mobile_Phone = singleContact.Mobile_Number,
                E_Mail = singleContact.Email_Id,
                Contact_person = singleContact.Name,
                First_name = singleContact.Name,
                Language = sapPayloadDto.Language,
                Tax_Number_3 = sapPayloadDto.Tax_Number_3,
                Industry = sapPayloadDto.Industry,
                Initiators_name = sapPayloadDto.Initiators_name,
                Pan_Number = sapPayloadDto.Pan_Number,
                GST_Ven_Class = sapPayloadDto.GST_Ven_Class,
                Recon_account = sapPayloadDto.Recon_account,
                Order_currency = sapPayloadDto.Order_currency,
                Incoterms = sapPayloadDto.Incoterms,
                Incoterms_Text = singleAddress.City,
                Schema_Group_Vendor = sapPayloadDto.Schema_Group_Vendor,
                GR_based_Inv_Verif = sapPayloadDto.GR_based_Inv_Verif,
                SRV_based_Inv_Verif = sapPayloadDto.SRV_based_Inv_Verif

            };

            OrganizationNameForSAPPayload(sapPayload);
            return sapPayload;
        }
        public Contact GetContactDetail(List<Contact> contacts)
        {
            var contact = contacts.Where(x => x.Contact_Type_Id == 1).FirstOrDefault();
            if (contact == null)
            {
                contact = contacts.FirstOrDefault();
            }
            return contact;
        }
        public Address GetAddressDetail(List<Address> addresses)
        {
            var address = addresses.Where(x => x.Address_Type_Id == 1).FirstOrDefault();
            if (address == null)
            {
                address = addresses.FirstOrDefault();
            }
            return address;
        }
        public void OrganizationNameForSAPPayload(SAPVendorCreationPayload sapPayload)
        {
            string OrgName = sapPayload.Name1;
            sapPayload.Name1 = OrgName;
            sapPayload.Name2 = string.Empty;
            sapPayload.Name3 = string.Empty;
            if (!string.IsNullOrEmpty(OrgName))
            {
                int maxlength = 40;
                int length = OrgName.Length;
                if (length > maxlength)
                {
                    sapPayload.Name1 = OrgName.Substring(0, maxlength);
                    if (length > 40)
                    {
                        sapPayload.Name2 = OrgName.Substring(maxlength, Math.Min(length - maxlength, maxlength) + maxlength);
                    }
                    if (length > 80)
                    {
                        sapPayload.Name3 = OrgName.Substring(maxlength * 2, Math.Min(length - maxlength, maxlength) + maxlength * 2);
                    }
                }
            }
        }
        private string GenerateVendorSoapPayload(SAPVendorCreationPayload request)
        {
            var soapRequestXml = @$"
           <?xml version=""1.0"" encoding=""utf-8""?>
           <soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
           <soap:Header>
           <wsse:Security soap:mustUnderstand = ""1""
           xmlns: wsse = ""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" />
           </soap:Header>
            <soap:Body>
                <ZmmRfcVendorCreate xmlns: tns =""urn:sap-com:document:sap:soap:functions:mc-style"">
                 <ImVendorData>
                   <item>
                    <CompanyCode> {request.Company_code} </CompanyCode>
                    <PurchasingOrg> {request.Purchasing_org} </PurchasingOrg>
                    <AccountGrp> {request.Account_grp} </AccountGrp>
                    <Name1> {request.Name1} </Name1>
                    <Name2> {request.Name2} </Name2>
                    <Name3> {request.Name3} </Name3>
                    <SearchTerm> {request.Search_term} </SearchTerm>
                    <Street2> {request.Street_2} </Street2>
                    <Street3> {request.Street_3} </Street3>
                    <StreetHouseNumber> {request.Street_House_number} </StreetHouseNumber>
                    <Street4>{request.Street_4}</Street4>
                    <Street5></Street5>
                    <City>{request.City}</City>
                    <PostalCode>{request.Postal_Code}</PostalCode>
                    <District>{request.District}</District>
                    <Country>{request.Country}</Country>
                    <Region>{request.Region}</Region>
                    <Language>{request.Language}</Language>
                    <Telephone>{request.Telephone}</Telephone>
                    <TelExtens></TelExtens>
                    <Fax>{request.Fax}</Fax>
                    <MobilePhone>{request.Mobile_Phone}</MobilePhone>
                    <EMail>{request.E_Mail}</EMail>
                    <TaxNumber3>{request.Tax_Number_3}</TaxNumber3>
                    <Industry>{request.Industry}</Industry>
                    <InitiatorsName>{request.Initiators_name}</InitiatorsName>
                    <PanNumber>{request.Pan_Number}</PanNumber>
                    <GstVenClass>{request.GST_Ven_Class}</GstVenClass>
                    <FirstName>{request.First_name}</FirstName>
                    <CpName> {request.Contact_person} </CpName>
                    <ReconAccount>{request.Recon_account}</ReconAccount>
                    <OrderCurrency>{request.Order_currency}</OrderCurrency>
                    <IncotermsText>{request.Incoterms}</Incoterms>
                    <IncotermsText>{request.City}</IncotermsText>
                    <SchemaGroupVendor>{request.Schema_Group_Vendor}</SchemaGroupVendor>
                    <GrBasedInvVerif>{request.GR_based_Inv_Verif}</GrBasedInvVerif>
                    <SrvBasedInvVerif>{request.SRV_based_Inv_Verif}</SrvBasedInvVerif>
                    <VendorCode></VendorCode>  
                    <Message></Message>
                   </item>
                 </ImVendorData>
                </ZmmRfcVendorCreate>
            </soap:Body>
        </soap:Envelope>
            ";

            LogWritter.WriteErrorLog(new string('*', 10) + "XML PAYLOAD" + new string('*', 10));
            LogWritter.WriteErrorLog(soapRequestXml);
            LogWritter.WriteErrorLog(new string('*', 10) + "XML PAYLOAD" + new string('*', 10));

            return soapRequestXml;

        }



        public async Task<ZMM_RFC_VENDOR_CREATEResponse> GetVendorCreateResponse(int formId)
        {
            return await _vendorService.CreateVendorAsync(GetCompleteFormDetailsForVendorCreation(formId));
        }

        public VendorItem GetCompleteFormDetailsForVendorCreation(int Form_Id)
        {
            var formData = this._dbContext.Forms.FirstOrDefault(x => x.Form_Id == Form_Id);
            VendorItem payload = new VendorItem();
            switch (formData.Vendor_Type_Id)
            {
                case 1:
                    payload = GetNonTransporterForVendorCreation(Form_Id);
                    break;
                case 2:
                    payload = GetNonTransporterForVendorCreation(Form_Id);
                    break;
                case 3:
                    payload = GetNonTransporterForVendorCreation(Form_Id);
                    break;
                case 4:
                    payload = GetTransportVendorForVendorCreation(Form_Id);
                    break;
                case 5:
                    payload = GetNonTransporterForVendorCreation(Form_Id);
                    break;
                default:
                    break;
            }
            return payload;

        }
        public VendorItem GetNonTransporterForVendorCreation(int Form_Id)
        {
            var sapPayloadDto = _dbContext.Forms
                .Where(f => f.Form_Id == Form_Id)
                .Include(c => c.CompanyCode)
                .Select(f => new
                {
                    Form = f,
                    VendorPersonalData = _dbContext.Vendor_Personal_Data.Where(vpd => vpd.Form_Id == f.Form_Id)
                    .Include(x => x.GSTVenClass)
                    .Include(x => x.Title).FirstOrDefault(),
                    Addresses = _dbContext.Addresses.Where(x => x.Form_Id == f.Form_Id)
                    .Include(x => x.AddressTypes)
                    .Include(x => x.Country)
                    .Include(x => x.Region).ToList(),
                    Contacts = _dbContext.Contacts.Where(x => x.Form_Id == f.Form_Id)
                    .Include(x => x.ContactTypes).ToList(),
                    CommercialProfile = _dbContext.Commercial_Profile.FirstOrDefault(x => x.Form_Id == f.Form_Id),
                    AdditionalFields = _dbContext.AdditionalFields.Where(x => x.Form_Id == f.Form_Id)
                    .Include(x => x.Incoterms)
                    .Include(x => x.Industry)
                    .Include(x => x.VendorAccountGroup)
                    .Include(x => x.ReconciliationAccount)
                    .Include(x => x.PurchaseOrganization)
                    .Include(x => x.SchemaGroup).FirstOrDefault(),
                    Initiator = _dbContext.Users.FirstOrDefault(x => x.Employee_Id == f.Created_By)
                })
                .AsEnumerable()
                .Select(x => new VendorItemDto
                {
                    COMPANY_CODE = x.Form.CompanyCode.Company_Code,
                    PURCHASING_ORG = x.AdditionalFields.PO_Code,
                    ACCOUNT_GRP = x.AdditionalFields.VendorAccountGroup.Code,
                    NAME1 = x.VendorPersonalData.Organization_Name,
                    NAME2 = "",
                    NAME3 = "",
                    SEARCH_TERM = x.AdditionalFields.Search_Term,
                    ADDRESSES = x.Addresses,
                    LANGUAGE = x.AdditionalFields.Language,
                    CONTACTS = x.Contacts,
                    TAX_NUMBER_3 = x.CommercialProfile?.GSTIN ?? "NA",
                    INDUSTRY = x.AdditionalFields.Industry.Code,
                    INITIATORS_NAME = $"{x.Initiator.First_Name} {x.Initiator.Middle_Name} {x.Initiator.Last_Name}".TrimEnd(),
                    PAN_NUMBER = x.CommercialProfile?.PAN ?? "NA",
                    GST_VEN_CLASS = x.VendorPersonalData?.GSTVenClass?.Code ?? "NA",
                    RECON_ACCOUNT = x.AdditionalFields.ReconciliationAccount.Code,
                    ORDER_CURRENCY = x.AdditionalFields.Order_Currency,
                    INCOTERMS = x.AdditionalFields.Incoterms.Code,
                    INCOTERMS_TEXT = "",
                    SCHEMA_GROUP_VENDOR = x.AdditionalFields.SchemaGroup.Code,
                    GR_BASED_INV_VERIF = x.AdditionalFields.GrBased,
                    SRV_BASED_INV_VERIF = x.AdditionalFields.SrvBased
                }).FirstOrDefault();


            var singleContact = GetContactDetail(sapPayloadDto.CONTACTS);
            var singleAddress = GetAddressDetail(sapPayloadDto.ADDRESSES);

            var sapPayload = new VendorItem()
            {
                COMPANY_CODE = sapPayloadDto.COMPANY_CODE,
                PURCHASING_ORG = sapPayloadDto.PURCHASING_ORG,
                ACCOUNT_GRP = sapPayloadDto.ACCOUNT_GRP,
                NAME1 = sapPayloadDto.NAME1,
                NAME2 = "",
                NAME3 = "",
                SEARCH_TERM = sapPayloadDto.SEARCH_TERM,
                STREET_HOUSE_NUMBER = singleAddress.House_No,
                STREET_2 = singleAddress.Street_2,
                STREET_3 = singleAddress.Street_3,
                STREET_4 = singleAddress.Street_4,
                STREET_5 = "",
                DISTRICT = singleAddress.District,
                POSTAL_CODE = singleAddress.Postal_Code,
                CITY = singleAddress.City,
                COUNTRY = singleAddress.Country.Code,
                REGION = singleAddress.Region.Code,
                TELEPHONE = singleAddress.Tel,
                TEL_EXTENS = "",
                FAX = singleAddress.Fax,
                MOBILE_PHONE = singleContact.Mobile_Number,
                E_MAIL = singleContact.Email_Id,
                CP_NAME = singleContact.Name,
                FIRST_NAME = singleContact.Name,
                LANGUAGE = sapPayloadDto.LANGUAGE.Substring(0, 1),
                TAX_NUMBER_3 = sapPayloadDto.TAX_NUMBER_3,
                INDUSTRY = sapPayloadDto.INDUSTRY,
                INITIATORS_NAME = sapPayloadDto.INITIATORS_NAME,
                PAN_NUMBER = sapPayloadDto.PAN_NUMBER,
                GST_VEN_CLASS = sapPayloadDto.GST_VEN_CLASS,
                RECON_ACCOUNT = sapPayloadDto.RECON_ACCOUNT,
                ORDER_CURRENCY = sapPayloadDto.ORDER_CURRENCY,
                INCOTERMS = sapPayloadDto.INCOTERMS,
                INCOTERMS_TEXT = "TURBHE",
                SCHEMA_GROUP_VENDOR = sapPayloadDto.SCHEMA_GROUP_VENDOR,
                GR_BASED_INV_VERIF = sapPayloadDto.GR_BASED_INV_VERIF,
                SRV_BASED_INV_VERIF = sapPayloadDto.SRV_BASED_INV_VERIF,
                VENDOR_CODE = "",
                MESSAGE = ""
            };

            string OrgName = sapPayload.NAME1;
            if (!string.IsNullOrEmpty(OrgName))
            {
                int maxlength = 40;
                int length = sapPayload.NAME1.Length;
                if (length > maxlength)
                {
                    sapPayload.NAME1 = OrgName.Substring(0, maxlength);
                    if (length > 40)
                    {
                        sapPayload.NAME2 = OrgName.Substring(maxlength, Math.Min(length - maxlength, maxlength) + maxlength);
                    }
                    if (length > 80)
                    {
                        sapPayload.NAME3 = OrgName.Substring(maxlength * 2, Math.Min(length - maxlength, maxlength) + maxlength * 2);
                    }
                }
            }



            return sapPayload;
        }
        public VendorItem GetTransportVendorForVendorCreation(int Form_Id)
        {
            var sapPayloadDto = _dbContext.Forms
                .Where(f => f.Form_Id == Form_Id)
                .Include(c => c.CompanyCode)
                .Select(f => new
                {
                    Form = f,
                    VendorPersonalData = _dbContext.Transport_Vendor_Personal_Data.Where(vpd => vpd.Form_Id == f.Form_Id)
                    .Include(x => x.GSTVenClass)
                    .Include(x => x.Title).FirstOrDefault(),
                    Addresses = _dbContext.Addresses.Where(x => x.Form_Id == f.Form_Id)
                    .Include(x => x.AddressTypes)
                    .Include(x => x.Country)
                    .Include(x => x.Region).ToList(),
                    Contacts = _dbContext.Contacts.Where(x => x.Form_Id == f.Form_Id)
                    .Include(x => x.ContactTypes).ToList(),
                    CommercialProfile = _dbContext.Commercial_Profile.FirstOrDefault(x => x.Form_Id == f.Form_Id),
                    AdditionalFields = _dbContext.AdditionalFields.Where(x => x.Form_Id == f.Form_Id)
                    .Include(x => x.Incoterms)
                    .Include(x => x.Industry)
                    .Include(x => x.VendorAccountGroup)
                    .Include(x => x.ReconciliationAccount)
                    .Include(x => x.PurchaseOrganization)
                    .Include(x => x.SchemaGroup).FirstOrDefault(),
                    Initiator = _dbContext.Users.FirstOrDefault(x => x.Employee_Id == f.Created_By)
                })
                .AsEnumerable()
                .Select(x => new VendorItemDto
                {
                    COMPANY_CODE = x.Form.CompanyCode.Company_Code,
                    PURCHASING_ORG = x.AdditionalFields.PO_Code,
                    ACCOUNT_GRP = x.AdditionalFields.VendorAccountGroup.Code,
                    NAME1 = x.VendorPersonalData.Name_of_Transporter,
                    NAME2 = "",
                    NAME3 = "",
                    SEARCH_TERM = x.AdditionalFields.Search_Term,
                    ADDRESSES = x.Addresses,
                    LANGUAGE = x.AdditionalFields.Language,
                    CONTACTS = x.Contacts,
                    TAX_NUMBER_3 = x.CommercialProfile?.GSTIN ?? "NA",
                    INDUSTRY = x.AdditionalFields.Industry.Code,
                    INITIATORS_NAME = $"{x.Initiator.First_Name} {x.Initiator.Middle_Name} {x.Initiator.Last_Name}".TrimEnd(),
                    PAN_NUMBER = x.CommercialProfile?.PAN ?? "NA",
                    GST_VEN_CLASS = x.VendorPersonalData?.GSTVenClass?.Code ?? "NA",
                    RECON_ACCOUNT = x.AdditionalFields.ReconciliationAccount.Code,
                    ORDER_CURRENCY = x.AdditionalFields.Order_Currency,
                    INCOTERMS = x.AdditionalFields.Incoterms.Code,
                    INCOTERMS_TEXT = "",
                    SCHEMA_GROUP_VENDOR = x.AdditionalFields.SchemaGroup.Code,
                    GR_BASED_INV_VERIF = x.AdditionalFields.GrBased,
                    SRV_BASED_INV_VERIF = x.AdditionalFields.SrvBased
                }).FirstOrDefault();


            var singleContact = GetContactDetail(sapPayloadDto.CONTACTS);
            var singleAddress = GetAddressDetail(sapPayloadDto.ADDRESSES);

            var sapPayload = new VendorItem()
            {
                COMPANY_CODE = sapPayloadDto.COMPANY_CODE,
                PURCHASING_ORG = sapPayloadDto.PURCHASING_ORG,
                ACCOUNT_GRP = sapPayloadDto.ACCOUNT_GRP,
                NAME1 = sapPayloadDto.NAME1,
                NAME2 = "",
                NAME3 = "",
                SEARCH_TERM = sapPayloadDto.SEARCH_TERM,
                STREET_HOUSE_NUMBER = singleAddress.House_No,
                STREET_2 = singleAddress.Street_2,
                STREET_3 = singleAddress.Street_3,
                STREET_4 = singleAddress.Street_4,
                STREET_5 = "",
                DISTRICT = singleAddress.District,
                POSTAL_CODE = singleAddress.Postal_Code,
                CITY = singleAddress.City,
                COUNTRY = singleAddress.Country.Code,
                REGION = singleAddress.Region.Code,
                TELEPHONE = singleAddress.Tel,
                TEL_EXTENS = "",
                FAX = singleAddress.Fax,
                MOBILE_PHONE = singleContact.Mobile_Number,
                E_MAIL = singleContact.Email_Id,
                CP_NAME = singleContact.Name,
                FIRST_NAME = singleContact.Name,
                LANGUAGE = sapPayloadDto.LANGUAGE.Substring(0, 1),
                TAX_NUMBER_3 = sapPayloadDto.TAX_NUMBER_3,
                INDUSTRY = sapPayloadDto.INDUSTRY,
                INITIATORS_NAME = sapPayloadDto.INITIATORS_NAME,
                PAN_NUMBER = sapPayloadDto.PAN_NUMBER,
                GST_VEN_CLASS = sapPayloadDto.GST_VEN_CLASS,
                RECON_ACCOUNT = sapPayloadDto.RECON_ACCOUNT,
                ORDER_CURRENCY = sapPayloadDto.ORDER_CURRENCY,
                INCOTERMS = sapPayloadDto.INCOTERMS,
                INCOTERMS_TEXT = "TURBHE",
                SCHEMA_GROUP_VENDOR = sapPayloadDto.SCHEMA_GROUP_VENDOR,
                GR_BASED_INV_VERIF = sapPayloadDto.GR_BASED_INV_VERIF,
                SRV_BASED_INV_VERIF = sapPayloadDto.SRV_BASED_INV_VERIF,
                VENDOR_CODE = "",
                MESSAGE = ""
            };

            string OrgName = sapPayload.NAME1;
            if (!string.IsNullOrEmpty(OrgName))
            {
                int maxlength = 40;
                int length = sapPayload.NAME1.Length;
                if (length > maxlength)
                {
                    sapPayload.NAME1 = OrgName.Substring(0, maxlength);
                    if (length > 40)
                    {
                        sapPayload.NAME2 = OrgName.Substring(maxlength, Math.Min(length - maxlength, maxlength) + maxlength);
                    }
                    if (length > 80)
                    {
                        sapPayload.NAME3 = OrgName.Substring(maxlength * 2, Math.Min(length - maxlength, maxlength) + maxlength * 2);
                    }
                }
            }

            return sapPayload;
        }


        public async Task SendSuccessVendorCreationMails(Form form, ZMM_RFC_VENDOR_CREATEResponse sapVendorCodeResponse)
        {
            var sendMail = new WecomeApprovedVendorMail()
            {
                Recepient = form.Vendor_Name,
                ToEmail = form.Vendor_Mail,
                Username = sapVendorCodeResponse.EX_VENDOR_DATA[0].VENDOR_CODE,
                Password = _appSetting.DefaultPassword
            };
            await _emailHelper.SendWelcomeMailToVendors(sendMail);

            var tasks = (from task in _dbContext.Tasks
                         where task.Form_Id == form.Form_Id
                         select task).OrderBy(x => x.Task_Id).ToList();
            var mxLvl = tasks.Select(x => x.Level).Max();
            for (var i = 1; i < mxLvl; i++)
            {
                var tk = tasks.FirstOrDefault(x => x.Level == i);
                var user = this._dbContext.Users.FirstOrDefault(x => x.Employee_Id == tk.Owner_Id);
                var notifyMail = new VendorCreationNotification()
                {
                    Recepient = $"{user.First_Name} {user.Middle_Name} {user.Last_Name}".TrimEnd(),
                    ToMail = user.Email,
                    VendorCode = form.Vendor_Code,
                    VendorMail = form.Vendor_Mail,
                    VendorMobile = form.Vendor_Mobile,
                    VendorName = form.Vendor_Name
                };
                await _emailHelper.SendVendorCreationNotification(notifyMail);
            }
        }

        #endregion
    }
}
