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
                        return Ok(this._mapper.Map<List<Address_Dto>>(await this._dbContext.Addresses.Where(f => f.Form_Id == formId).ToListAsync()));
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
                           select new
                           {
                               form = form,
                               grade = grade
                           }).FirstOrDefaultAsync();
            string since = "";
            if (profile != null)
            {
                switch (profile.form.Vendor_Type_Id)
                {
                    case 1:
                        since = this._dbContext.Vendor_Personal_Data.FirstOrDefault(x => x.Form_Id == formId).Plant_Installation_Year.ToString();
                        break;
                    case 2:
                        since = this._dbContext.Vendor_Personal_Data.FirstOrDefault(x => x.Form_Id == formId).Plant_Installation_Year.ToString();
                        break;
                    case 3:
                        since = this._dbContext.Transport_Vendor_Personal_Data.FirstOrDefault(x => x.Form_Id == formId).Date_of_Establishment.ToString();
                        break;
                    case 4:
                        since = this._dbContext.Vendor_Personal_Data.FirstOrDefault(x => x.Form_Id == formId).Plant_Installation_Year.ToString();
                        break;
                    default:
                        break;
                }

                var res = new VendorProfileDto()
                {
                    Vendor_Name = profile.form.Vendor_Name,
                    Year = since,
                    Grade = new VendorGradeDto()
                    {
                        FormId = profile.form.Form_Id,
                        GradeId = profile.grade.GradeId,
                        Grade = profile.grade.Grade,
                        Last_Audited_By = profile.grade.Last_Audited_By,
                        Last_Audit_Date = profile.grade.Last_Audit_Date,
                        Location = profile.grade.Location,
                        Vendor_Code = profile.grade.Vendor_Code
                    }
                };
                return Ok(res);
            }
            else
            {
                return Ok(new VendorProfileDto());
            }

        }

    }
}
