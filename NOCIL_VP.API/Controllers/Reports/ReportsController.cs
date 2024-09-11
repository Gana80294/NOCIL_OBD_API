using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Domain.Core.Dtos.Master;
using NOCIL_VP.Domain.Core.Dtos.Registration;
using NOCIL_VP.Infrastructure.Data.Enums;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Reports;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace NOCIL_VP.API.Controllers.Reports
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class ReportsController : ControllerBase
    {
        private IReportRepository _reportRepository;
        private readonly ExcelHelper _excelHelper;
        public ReportsController(IReportRepository reportRepository)
        {
            this._reportRepository = reportRepository;
            _excelHelper = new ExcelHelper();
        }

        [HttpGet]
        public async Task<IActionResult> DownloadVendorsAsExcel(string type)
        {
            var data = new List<VendorMasterDto>();

            switch (type)
            {
                case "ISO":
                    data = await _reportRepository.GetAllVendorsByType(true);
                    break;
                case "NONISO":
                    data = await _reportRepository.GetAllVendorsByType(false);
                    break;
                case "Transport":
                    data = await _reportRepository.GetAllTransportVendors();
                    break;
            }

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = _excelHelper.CreateNPOIworksheetForVendorMaster(data, workbook);
            DateTime dt = DateTime.Today;
            string dtstr = dt.ToString("ddMMyyyyHHmmss");
            var FileNm = $"{type}_Vendors_{dtstr}.xlsx";

            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            byte[] fileByteArray = stream.ToArray();
            return File(fileByteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileNm);
        }

        [HttpPost]
        public async Task<IActionResult> SearchAllVendors(VendorReportDto reportDto)
        {
            return Ok(await _reportRepository.SearchAllVendors(reportDto));
        }


        [HttpPost]
        public async Task<IActionResult> DownloadFilteredVendors(VendorReportDto reportDto)
        {
            var data = new List<VendorMasterDto>();
            data = await _reportRepository.SearchAllVendors(reportDto);
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = _excelHelper.CreateNPOIworksheetForVendorMaster(data, workbook);
            DateTime dt = DateTime.Today;
            string dtstr = dt.ToString("ddMMyyyyHHmmss");
            var FileNm = $"Filtered_Vendors_{dtstr}.xlsx";

            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            byte[] fileByteArray = stream.ToArray();
            return File(fileByteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileNm);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadVendorByExpiry()
        {
            var data = new List<VendorMasterDto>();
            data = await _reportRepository.DownloadVendorByExpiry();

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = _excelHelper.CreateNPOIworksheetForExpiryVendor(data, workbook);
            DateTime dt = DateTime.Today;
            string dtstr = dt.ToString("ddMMyyyyHHmmss");
            var FileNm = $"Expiry_Vendors_{dtstr}.xlsx";

            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            byte[] fileByteArray = stream.ToArray();
            return File(fileByteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileNm);


        }
    }
}
