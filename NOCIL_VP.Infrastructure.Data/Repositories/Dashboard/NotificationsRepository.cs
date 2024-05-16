using Microsoft.EntityFrameworkCore;
using NOCIL_VP.Domain.Core.Dtos.Dashboard;
using NOCIL_VP.Domain.Core.Entities;
using NOCIL_VP.Infrastructure.Interfaces.Repositories.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Repositories.Dashboard
{
    public class NotificationsRepository : INotificationsRepository
    {

        private VpContext _dbContext;

        public NotificationsRepository(VpContext context)
        {
            this._dbContext = context;
        }


        public async Task<List<ExpiryNotificationDto>> GetAllExpiryNotifications()
        {
            var res = await (from tb in _dbContext.Attachments
                       where tb.Is_Expiry_Available && tb.Expiry_Date.Value.Date.AddDays(-10) <= DateTime.Now.Date
                       select new ExpiryNotificationDto
                       {
                           Doc_Type = tb.File_Type,
                           Valid_Till_Date = (DateTime)tb.Expiry_Date,
                           Vendor_Name = tb.Forms.Vendor_Name,
                           Vendor_Mobile = tb.Forms.Vendor_Mobile,
                           Vendor_Mail = tb.Forms.Vendor_Mail
                       }).ToListAsync();
            return res;
        }

        public async Task<List<ExpiryNotificationDto>> GetExpiryNotificationsByVendorCode(string vCode)
        {
            var res = await(from tb in _dbContext.Forms
                            where tb.Vendor_Code==vCode
                            join docs in _dbContext.Attachments on tb.Form_Id equals docs.Form_Id
                            where docs.Is_Expiry_Available && docs.Expiry_Date.Value.Date.AddDays(-10) <= DateTime.Now.Date
                            select new ExpiryNotificationDto
                            {
                                Doc_Type = docs.File_Type,
                                Valid_Till_Date = (DateTime)docs.Expiry_Date,
                                Vendor_Name = tb.Vendor_Name,
                                Vendor_Mobile = tb.Vendor_Mobile,
                                Vendor_Mail = tb.Vendor_Mail
                            }).ToListAsync();
            return res;
        }
    }
}
