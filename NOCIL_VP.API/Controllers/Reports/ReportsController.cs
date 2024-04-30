using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NOCIL_VP.Domain.Core.Dtos.Master;
using NOCIL_VP.Infrastructure.Data.Helpers;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Reports;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace NOCIL_VP.API.Controllers.Reports
{
    [Route("api/[controller]/[action]")]
    [ApiController]
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

            //_reportRepository.CreateFolder();
            //var FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "TempFolder", FileNm);
            //FileStream stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);

            MemoryStream stream = new MemoryStream();
            workbook.Write(stream);
            byte[] fileByteArray = stream.ToArray();
            return File(fileByteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileNm);
        }
    }
}
