﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class PurchaseOrganization
    {
        [Key, MaxLength(10)]
        public string PO_Code { get; set; }
        [Required, MaxLength(50)]
        public string Description { get; set; }
    }
}
