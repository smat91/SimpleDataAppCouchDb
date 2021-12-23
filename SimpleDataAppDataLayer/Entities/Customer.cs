using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataAppDataLayer.Entities
{
    public class Customer
    {
        public string _id { get; set; }     // document id for couch db
        public string _rev { get; set; }    // document revision for couch db
        public string Name { get; set; }

    }
}
