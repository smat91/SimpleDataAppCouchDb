using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataApp.Entities
{
    class Order
    {
        public string _id { get; set; }
        public string _rev { get; set; }

        public string Customer { get; set; }
        public string OrderState { get; set; }
        public string OrderDate { get; set; }
        public int OrderAmount { get; set; }
    }
}
