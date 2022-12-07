using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshopping.web.Models
{
    public class SmallCartViewModel
    {
        public int NumberOfItems { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
