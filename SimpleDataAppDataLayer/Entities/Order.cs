using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataAppDataLayer.Entities
{
    public class Order
    {
        public string _id { get; set; }     // document id for couch db
        public string _rev { get; set; }    // document revision for couch db

        public string Customer { get; set; }
        public string OrderState { get; set; }
        public string OrderDate { get; set; }
        public int OrderAmount { get; set; }
    }
}
