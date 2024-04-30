﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Master
{
    public class AddressType
    {
        [Key]
        public int Address_Type_Id {  get; set; }
        [Required,MaxLength(50)]
        public string Address_Type { get; set; }
    }
}
