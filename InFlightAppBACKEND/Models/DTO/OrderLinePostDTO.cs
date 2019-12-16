using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InFlightAppBACKEND.Models.DTO
{
    public class OrderLinePostDTO{
        public int ProductID { get; set; }
        public int Amount { get; set; }
    }
}
