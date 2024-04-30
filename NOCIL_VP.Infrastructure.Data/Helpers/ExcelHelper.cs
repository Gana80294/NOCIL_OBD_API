using NOCIL_VP.API.Logging;
using NOCIL_VP.Domain.Core.Dtos.Master;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Helpers
{
    public class ExcelHelper
    {
        public ExcelHelper()
        {

        }


        public ISheet CreateNPOIworksheetForVendorMaster(List<VendorMasterDto> data, IWorkbook workbook = null)
        {
            List<string> headings = new List<string>() { "Vendor Name", "Vendor Mail", "Vendor Mobile", "Vendor Type", "Vendor Code" };
            ISheet sheet = workbook.CreateSheet("Vendor details");
            try
            {
                IRow row = sheet.CreateRow(0);
                var font = workbook.CreateFont();
                font.FontHeightInPoints = 11;
                font.FontName = "Calibri";
                font.IsBold = true;
                var titleStyle = workbook.CreateCellStyle();
                titleStyle.SetFont(font);

                for (var k = 0; k < headings.Count; k++)
                {
                    ICell cell = row.CreateCell(k);
                    cell.CellStyle = titleStyle;
                    cell.SetCellValue(headings[k]);
                }

                for (var k = 1; k <= data.Count; k++)
                {
                    IRow rowdef = sheet.CreateRow(k);

                    ICell celldef0 = rowdef.CreateCell(0);
                    celldef0.SetCellValue(data[k - 1].Vendor_Name);

                    ICell celldef1 = rowdef.CreateCell(1);
                    celldef1.SetCellValue(data[k - 1].Vendor_Mail);

                    ICell celldef2 = rowdef.CreateCell(2);
                    celldef2.SetCellValue(data[k - 1].Vendor_Mobile);

                    ICell celldef3 = rowdef.CreateCell(3);
                    celldef3.SetCellValue(data[k - 1].Vendor_Type);

                    ICell celldef4 = rowdef.CreateCell(4);
                    celldef4.SetCellValue(data[k - 1].Vendor_Code);
                }
                return sheet;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
