using NOCIL_VP.Domain.Core.Entities.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOCIL_VP.Domain.Core.Entities.Mappings
{
    public class User_Role_Mapping
    {
        public string Employee_Id { get; set; }
        public int Role_Id { get; set; }
    }
}
