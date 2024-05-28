using NOCIL_VP.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Infrastructure.Data.Enums
{
    
    public static class RolesExtension
    {
        public const string Admin = "Admin";
        public const string Vendor = "Vendor";
        public const string DomesticPO = "Domestic PO";
        public const string ImportPO = "Import PO";
        public const string EngineeringPO = "Engineering PO";
        public const string DomesticRM = "Domestic RM";
        public const string ImportRM = "Import RM";
        public const string EngineeringRM = "Engineering RM";
        public const string GeneralManager = "General Manager";
        public const string SystemSAP = "System SAP";
    }

}
