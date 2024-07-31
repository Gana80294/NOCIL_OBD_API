using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NOCIL_VP.Domain.Core.Dtos.Master;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Domain.Core.Dtos.Registration.CommonData;
using NOCIL_VP.Domain.Core.Dtos.Registration.Domestic;
using NOCIL_VP.Domain.Core.Dtos.Registration.Evaluation;
using NOCIL_VP.Domain.Core.Dtos.Registration.Transport;
using NOCIL_VP.Domain.Core.Entities;
using Org.BouncyCastle.Asn1.X509.SigI;

namespace NOCIL_VP.API.Controllers.Registration
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class GetFormDataController : ControllerBase
    {
        private VpContext _dbContext;
        private IMapper _mapper;

        public GetFormDataController(VpContext context, IMapper mapper)
        {
            this._dbContext = context;
            this._mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetFormData(int formId, string tableName)
        {
            var form = this._dbContext.Forms.FirstOrDefault(x => x.Form_Id == formId);
            if (form == null) return BadRequest("Unable to find the form");
            else
            {
                switch (tableName)
                {
                    case ("DomesticVendorPersonalData"):
                        return Ok(this._mapper.Map<VendorPersonalData_Dto>(await this._dbContext.Vendor_Personal_Data.FirstOrDefaultAsync(f => f.Form_Id == formId)));
                    case ("VendorOrganizationProfile"):
                        return Ok(this._mapper.Map<VendorOrganizationProfile_Dto>(await this._dbContext.Vendor_Organization_Profile.FirstOrDefaultAsync(f => f.Form_Id == formId)));
                    case ("TechnicalProfile"):
                        return Ok(this._mapper.Map<TechnicalProfile_Dto>(await this._dbContext.Technical_Profile.FirstOrDefaultAsync(f => f.Form_Id == formId)));
                    case ("CommercialProfile"):
                        return Ok(this._mapper.Map<CommercialProfile_Dto>(await this._dbContext.Commercial_Profile.FirstOrDefaultAsync(f => f.Form_Id == formId)));
                    case ("BankDetail"):
                        return Ok(this._mapper.Map<Bank_Detail_Dto>(await this._dbContext.Bank_Details.FirstOrDefaultAsync(f => f.Form_Id == formId)));
                    case ("TransportVendorPersonalData"):
                        return Ok(this._mapper.Map<TransportVendorPersonalData_Dto>(await this._dbContext.Transport_Vendor_Personal_Data.FirstOrDefaultAsync(f => f.Form_Id == formId)));
                    case ("Attachments"):
                        return Ok(this._mapper.Map<List<AttachmentDto>>(await this._dbContext.Attachments.Where(f => f.Form_Id == formId).ToListAsync()));
                    case ("Subsideries"):
                        return Ok(this._mapper.Map<List<Subsideries_Dto>>(await this._dbContext.Subsideries.Where(f => f.Form_Id == formId).ToListAsync()));
                    case ("MajorCustomers"):
                        return Ok(this._mapper.Map<List<MajorCustomer_Dto>>(await this._dbContext.MajorCustomers.Where(f => f.Form_Id == formId).ToListAsync()));
                    case ("Addresses"):
                        //return Ok(this._mapper.Map<List<Address_Dto>>(await this._dbContext.Addresses.Where(f => f.Form_Id == formId).ToListAsync()));
                        return Ok(this._dbContext.Addresses.Where(f => f.Form_Id == formId).Select(x => new Address_Dto
                        {
                            Address_Id = x.Address_Id,
                            Form_Id = x.Form_Id,
                            Address_Type_Id = x.Address_Type_Id,
                            House_No = x.House_No,
                            Street_2 = x.Street_2,
                            Street_3 = x.Street_3,
                            Street_4 = x.Street_4,
                            District = x.District,
                            Postal_Code = x.Postal_Code,
                            City = x.City,
                            Country_Code = x.Country.Code,
                            Region_Id = x.Region_Id,
                            Country_Name = x.Country.Name,
                            Region_Name = x.Region.Name,
                            Tel = x.Tel,
                            Fax = x.Fax,
                            Website = x.Website

                        }));
                    case ("Contacts"):
                        return Ok(this._mapper.Map<List<Contact_Dto>>(await this._dbContext.Contacts.Where(f => f.Form_Id == formId).ToListAsync()));
                    case ("VendorBranches"):
                        return Ok(this._mapper.Map<List<VendorBranch_Dto>>(await this._dbContext.VendorBranches.Where(f => f.Form_Id == formId).ToListAsync()));
                    case ("ProprietorOrPartners"):
                        return Ok(this._mapper.Map<List<ProprietorOrPartner_Dto>>(await this._dbContext.Proprietor_or_Partners.Where(f => f.Form_Id == formId).ToListAsync()));
                    case ("AnnualTurnOvers"):
                        return Ok(this._mapper.Map<List<AnnualTurnOver_Dto>>(await this._dbContext.Annual_TurnOver.Where(f => f.Form_Id == formId).ToListAsync()));
                    case ("TankerDetails"):
                        return Ok(this._mapper.Map<List<TankerDetail_Dto>>(await this._dbContext.Tanker_Details.Where(f => f.Form_Id == formId).ToListAsync()));
                    case ("NocilRelatedEmployees"):
                        return Ok(this._mapper.Map<List<NocilRelatedEmployeeDto>>(await this._dbContext.NocilRelatedEmployees.Where(f => f.Form_Id == formId).ToListAsync()));
                    case ("AdditionalFields"):
                        return Ok(this._mapper.Map<AdditionalFields_Dto>(await this._dbContext.AdditionalFields.FirstOrDefaultAsync(f => f.Form_Id == formId)));
                    default:
                        return BadRequest($"Invalid Table name - {tableName}");
                }
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetVendorProfile(int formId)
        {
            var profile = await (from form in _dbContext.Forms
                                 where form.Form_Id == formId
                                 join grade in _dbContext.VendorGrades on form.Form_Id equals grade.FormId
                                 into grades
                                 select new
                                 {
                                     form = form,
                                     grade = grades.FirstOrDefault(),
                                 }).FirstOrDefaultAsync();
            var personalData = new { since = "", name = "" };
            if (profile != null)
            {
                switch (profile.form.Vendor_Type_Id)
                {
                    case 1:

                        personalData = this._dbContext.Vendor_Personal_Data.Where(x => x.Form_Id == formId).Select(x => new
                        {
                            since = x.Plant_Installation_Year.ToString(),
                            name = x.Organization_Name
                        }).FirstOrDefault();
                        break;
                    case 2:
                        personalData = this._dbContext.Vendor_Personal_Data.Where(x => x.Form_Id == formId).Select(x => new
                        {
                            since = x.Plant_Installation_Year.ToString(),
                            name = x.Organization_Name
                        }).FirstOrDefault();
                        break;
                    case 3:
                        personalData = this._dbContext.Vendor_Personal_Data.Where(x => x.Form_Id == formId).Select(x => new
                        {
                            since = x.Plant_Installation_Year.ToString(),
                            name = x.Organization_Name
                        }).FirstOrDefault();
                        break;
                    case 4:
                        personalData = this._dbContext.Transport_Vendor_Personal_Data.Where(x => x.Form_Id == formId).Select(x => new
                        {
                            since = x.Date_of_Establishment.Value.Year.ToString(),
                            name = x.Name_of_Transporter
                        }).FirstOrDefault();
                        break;
                    case 5:
                        personalData = this._dbContext.Vendor_Personal_Data.Where(x => x.Form_Id == formId).Select(x => new
                        {
                            since = x.Plant_Installation_Year.ToString(),
                            name = x.Organization_Name
                        }).FirstOrDefault();
                        break;
                    default:
                        break;
                }

                var res = new VendorProfileDto()
                {
                    Vendor_Name = personalData?.name,
                    Year = personalData?.since,
                    Grade = profile.grade != null ? new VendorGradeDto()
                    {
                        FormId = profile.form.Form_Id,
                        GradeId = profile.grade.GradeId,
                        Grade = profile.grade.Grade,
                        Last_Audited_By = profile.grade.Last_Audited_By,
                        Last_Audit_Date = profile.grade.Last_Audit_Date,
                        Location = profile.grade.Location,
                        Vendor_Code = profile.grade.Vendor_Code
                    } : new VendorGradeDto()
                };
                return Ok(res);
            }
            else
            {
                return Ok(new VendorProfileDto());
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetVendorByCode(string VendorCode)
        {
            try
            {
                var forms = (from form in _dbContext.Forms
                           where form.Vendor_Code == VendorCode
                           select new
                           {
                               FormId = form.Form_Id,
                               VT_Id = form.Vendor_Type_Id,
                               Vendor_Type = form.VendorType.Vendor_Type
                           }).ToList().FirstOrDefault();

                var res = new
                {
                    FormId = 0,
                    Vendor_Name = "",
                    VT_Id = 0,
                    Vendor_Type = ""
                };
                if (forms != null)
                {
                    switch (forms.VT_Id)
                    {
                        case 1:

                            res = this._dbContext.Vendor_Personal_Data.Where(x => x.Form_Id == forms.FormId).Select(x => new
                            {
                                FormId = forms.FormId,
                                Vendor_Name = x.Organization_Name,
                                VT_Id = forms.VT_Id,
                                Vendor_Type = forms.Vendor_Type
                            }).FirstOrDefault();
                            break;
                        case 2:
                            res = this._dbContext.Vendor_Personal_Data.Where(x => x.Form_Id == forms.FormId).Select(x => new
                            {
                                FormId = forms.FormId,
                                Vendor_Name = x.Organization_Name,
                                VT_Id = forms.VT_Id,
                                Vendor_Type = forms.Vendor_Type
                            }).FirstOrDefault();
                            break;
                        case 3:
                            res = this._dbContext.Vendor_Personal_Data.Where(x => x.Form_Id == forms.FormId).Select(x => new
                            {
                                FormId = forms.FormId,
                                Vendor_Name = x.Organization_Name,
                                VT_Id = forms.VT_Id,
                                Vendor_Type = forms.Vendor_Type
                            }).FirstOrDefault();
                            break;
                        case 4:
                            res = this._dbContext.Transport_Vendor_Personal_Data.Where(x => x.Form_Id == forms.FormId).Select(x => new
                            {
                                FormId = forms.FormId,
                                Vendor_Name = x.Name_of_Transporter,
                                VT_Id = forms.VT_Id,
                                Vendor_Type = forms.Vendor_Type
                            }).FirstOrDefault();
                            break;
                        case 5:
                            res = this._dbContext.Vendor_Personal_Data.Where(x => x.Form_Id == forms.FormId).Select(x => new
                            {
                                FormId = forms.FormId,
                                Vendor_Name = x.Organization_Name,
                                VT_Id = forms.VT_Id,
                                Vendor_Type = forms.Vendor_Type
                            }).FirstOrDefault();
                            break;
                        default:
                            break;
                    }
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
