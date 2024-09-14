using AutoMapper;
using Microsoft.Extensions.Options;
using NOCIL_VP.Domain.Core.Configurations;
using NOCIL_VP.Domain.Core.Dtos.Response;
using NOCIL_VP.Domain.Core.Dtos;
using NOCIL_VP.Domain.Core.Entities.Registration;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Infrastructure.Data.Enums;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Master;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NOCIL_VP.Domain.Core.Entities.Logs;
using NOCIL_VP.Domain.Core.Dtos.Dashboard;
using Newtonsoft.Json;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos.Master;
using NOCIL_VP.Domain.Core.Entities.Approval;
using NOCIL_VP.Domain.Core.Entities.Registration.CommonData;
using SAP_VENDOR_CREATE_SERVICE;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Registration
{
    public class EditRequestRepository : Repository<Form>, Interfaces.Repositories.Registration.IEditRequestRepository
    {
        private readonly VpContext _dbContext;

        private readonly EmailHelper _emailHelper;
        private readonly IDomesticAndImportRepository _domesticRepository;
        private readonly ModelValidator _validator;
        private readonly ITransportRepository _transportRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IMapper _mapper;


        public EditRequestRepository(VpContext context,
            IDomesticAndImportRepository domesticRepository,
            ITransportRepository transportRepository,
            IServiceRepository serviceRepository,
            IUserRepository userRepository,
             IRegistrationRepository registrationRepository,
            IMapper mapper,
            EmailHelper email, IOptions<AppSetting> opt, VendorService vendorService) : base(context)
        {
            this._dbContext = context;
            this._emailHelper = email;
            this._validator = new ModelValidator();
            this._transportRepository = transportRepository;
            this._serviceRepository = serviceRepository;
            this._registrationRepository = registrationRepository;
            this._mapper = mapper;
        }

        public async Task<ResponseMessage> RequestForEdit(RequestForEdit data)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var form = await _dbContext.Forms.FirstOrDefaultAsync(x => x.Form_Id == data.FormId);
                    var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Employee_Id == form.Created_By);

                    //send mail to request
                    var sendMail = new SendRequestToEditMail()
                    {
                        ToEmail = user.Email,
                        Username = $"{user.First_Name} {user.Middle_Name} {user.Last_Name}".TrimEnd(),
                        Form_Id = data.FormId,
                        Vendor_Type_Id = form.Vendor_Type_Id,
                        VendorCode = data.VendorCode,
                        Reason = data.Reason
                    };
                    await _emailHelper.SendRequestToEditMail(sendMail);

                    form.Status_Id = (int)FormStatusEnum.EditRequested;

                    var history = new EditRequestHistory()
                    {
                        Log_Id = 0,
                        Form_Id = data.FormId,
                        VendorCode = data.VendorCode,
                        RequestedOn = DateTime.Now,
                        IsApproved = false,
                        Status = FormStatusEnum.EditRequested.ToString(),
                        Message = data.Reason
                    };
                    await _dbContext.EditRequestHistories.AddAsync(history);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    transaction.Dispose();

                    return ResponseWritter.WriteSuccessResponse("Form edit requested successfully");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }

        public async Task<ResponseMessage> AcceptEditRequest(int formId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var form = _dbContext.Forms.FirstOrDefault(x => x.Form_Id == formId);
                    var sendMail = new SendMail()
                    {
                        ToEmail = form.Vendor_Mail,
                        Username = form.Vendor_Name,
                        Form_Id = formId,
                        Vendor_Type_Id = form.Vendor_Type_Id
                    };
                    await _emailHelper.SendAcceptEditReqMail(sendMail);
                    form.Status_Id = (int)FormStatusEnum.EditReqApproved;

                    var history = new EditRequestHistory()
                    {
                        Log_Id = 0,
                        Form_Id = formId,
                        //VendorCode = data.VendorCode,
                        RequestedOn = DateTime.Now,
                        IsApproved = false,
                        Status = FormStatusEnum.EditReqApproved.ToString(),
                        Message = "Edit Request Approved"
                    };
                    await _dbContext.EditRequestHistories.AddAsync(history);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    transaction.Dispose();
                    return ResponseWritter.WriteSuccessResponse("Form edit request successfully");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }

        public async Task<ResponseMessage> RejectEditRequest(int formId, string reason)
        {

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var form = _dbContext.Forms.FirstOrDefault(x => x.Form_Id == formId);
                    var sendMail = new SendRequestRejectToEditMail()
                    {
                        ToEmail = form.Vendor_Mail,
                        Username = form.Vendor_Name,
                        VendorCode = form.Vendor_Code,
                        Reason = reason
                    };
                    await _emailHelper.SendRejectEditReqMail(sendMail);
                    form.Status_Id = (int)FormStatusEnum.EditReqRejected;

                    var history = new EditRequestHistory()
                    {
                        Log_Id = 0,
                        Form_Id = formId,
                        //VendorCode = data.VendorCode,
                        RequestedOn = DateTime.Now,
                        IsApproved = false,
                        Status = FormStatusEnum.EditReqApproved.ToString(),
                        Message = "Edit Request Rejected"
                    };
                    await _dbContext.EditRequestHistories.AddAsync(history);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    transaction.Dispose();
                    return ResponseWritter.WriteSuccessResponse("Form edit request rejected successfully");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                    throw ex;
                }
            }
        }

        public async Task<List<DashboardDto>> GetEditRequestData(string employeeId)
        {
            var result = await (from form in _dbContext.Forms
                                join task in _dbContext.Tasks on form.Form_Id equals task.Form_Id
                                where form.Status_Id == (int)FormStatusEnum.EditRequested && task.Owner_Id == employeeId
                                select new DashboardDto
                                {
                                    FormId = form.Form_Id,
                                    VendorTypeId = form.Vendor_Type_Id,
                                    VendorType = form.VendorType.Vendor_Type,
                                    Email = form.Vendor_Mail,
                                    Mobile = form.Vendor_Mobile,
                                    Name = form.Vendor_Name,
                                    CreatedOn = form.Created_On,
                                    Status = form.FormStatus.Status,
                                    PendingWith = task.Role.Role_Name
                                }).Distinct().ToListAsync();
            return result;
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
            var form = _dbContext.Forms.FirstOrDefault(x => x.Form_Id == formData.Form_Id);
            form.Status_Id = (int)FormStatusEnum.EditApprovalPending;

            if (submitted) return ResponseWritter.WriteSuccessResponse("Form Upated Successfully");
            else throw new Exception("Workflow for this vendor type is not defined");
        }

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
                    //else if (oldTask.Level == maxLevl)
                    //{
                    //    // Add and Update entries
                    //    Tasks sapTask = new Tasks
                    //    {
                    //        Task_Id = 0,
                    //        Form_Id = approvalDto.Form_Id,
                    //        Owner_Id = null,
                    //        Level = oldTask.Level + 1,
                    //        StartDate = DateTime.Now,
                    //        Status = "SAP"
                    //    };
                    //    List<TransactionHistory> histories = new List<TransactionHistory>()
                    //    {
                    //        new TransactionHistory{ Log_Id = 0,Form_Id = approvalDto.Form_Id,Logged_Date = DateTime.Now, Message = $"Form {approvalDto.Form_Id} is approved by {approvalDto.EmployeeId}"},
                    //        new TransactionHistory { Log_Id = 0,Form_Id = approvalDto.Form_Id,Logged_Date = DateTime.Now, Message = $"Form {approvalDto.Form_Id} is moved to SAP"}
                    //    };
                    //    var form = this._dbContext.Forms.FirstOrDefault(x => x.Form_Id == approvalDto.Form_Id);
                    //    form.Status_Id = (int)FormStatusEnum.SAP;

                    //    // Save Changes
                    //    _dbContext.Tasks.Add(sapTask);
                    //    _dbContext.TransactionHistories.AddRange(histories);
                    //    await _dbContext.SaveChangesAsync();
                    //    try
                    //    {
                    //        // Call SAP functions
                    //        //var sapResponse = await CallSAPCreateVendor(approvalDto.Form_Id);
                    //        //ZmmRfcVendorCreateResponse sapVendorCodeResponse = _soapParser.ParseSoapResponse(sapResponse);

                    //        ZMM_RFC_VENDOR_CREATEResponse sapVendorCodeResponse = await GetVendorCreateResponse(approvalDto.Form_Id);

                    //        if (sapVendorCodeResponse != null && sapVendorCodeResponse.EX_VENDOR_DATA.Length > 0)
                    //        {
                    //            if (string.IsNullOrEmpty(sapVendorCodeResponse.EX_VENDOR_DATA[0].VENDOR_CODE))
                    //            {
                    //                throw new Exception("Vendor code not generated in SAP. " + sapVendorCodeResponse.EX_VENDOR_DATA[0].MESSAGE);
                    //            }
                    //            else
                    //            {
                    //                form.Vendor_Code = sapVendorCodeResponse.EX_VENDOR_DATA[0].VENDOR_CODE;
                    //                form.Status_Id = (int)FormStatusEnum.Approved;

                    //                var role = this._dbContext.Roles.FirstOrDefault(x => x.Role_Name.ToLower() == "vendor");
                    //                var user = new UserDto()
                    //                {
                    //                    Employee_Id = sapVendorCodeResponse.EX_VENDOR_DATA[0].VENDOR_CODE,
                    //                    First_Name = sapVendorCodeResponse.EX_VENDOR_DATA[0].NAME1,
                    //                    Middle_Name = sapVendorCodeResponse.EX_VENDOR_DATA[0].NAME2,
                    //                    Last_Name = sapVendorCodeResponse.EX_VENDOR_DATA[0].NAME3,
                    //                    Email = form.Vendor_Mail,
                    //                    Mobile_No = form.Vendor_Mobile,
                    //                    IsActive = true,
                    //                    Role_Id = role != null ? role.Role_Id : 10,
                    //                    Reporting_Manager_EmpId = null
                    //                };
                    //                await this._userRepository.AddUser(user);
                    //                await _dbContext.SaveChangesAsync();

                    //                await SendSuccessVendorCreationMails(form, sapVendorCodeResponse);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            throw new Exception("SAP Response is Empty");
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        throw new Exception("Error during SAP Vendor creation: " + ex.Message, ex);
                    //    }


                    //    // Send mail
                    //    //var approvedBy = (from user in _dbContext.Users
                    //    //                  where user.Employee_Id == approvalDto.EmployeeId
                    //    //                  join urMap in _dbContext.User_Role_Mappings on user.Employee_Id equals urMap.Employee_Id
                    //    //                  join role in _dbContext.Roles on urMap.Role_Id equals role.Role_Id
                    //    //                  select role).FirstOrDefault();

                    //    //var approval = new ApprovalInfo()
                    //    //{
                    //    //    ToEmail = form.Vendor_Mail,
                    //    //    ApprovedBy = approvedBy.Role_Name,
                    //    //    UserName = form.Vendor_Name,
                    //    //};
                    //    //await this._emailHelper.SendApprovalInfoMail(approval);
                    //}
                    else
                    {
                        throw new Exception("Unable to find the approval matrix");
                    }
                    await transaction.CommitAsync();
                    transaction.Dispose();
                    var form = _dbContext.Forms.FirstOrDefault(x => x.Form_Id == approvalDto.Form_Id);
                    form.Status_Id = (int)FormStatusEnum.EditApprovalPending;

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

        #endregion Approval Region
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
                    var form = _dbContext.Forms.FirstOrDefault(x => x.Form_Id == formData.Form_Id);
                    form.Status_Id = (int)FormStatusEnum.EditReqRejected;

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
                                  Form_Id = task.Forms.Form_Id,
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

    }
}
