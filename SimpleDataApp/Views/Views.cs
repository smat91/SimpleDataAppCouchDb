using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataApp.Views
{
    public static class Views
    {
        public const string OrderList =
            @"{
                ""_id"": ""_design/orders"",
                ""language"": ""javascript"",
                ""views"": {
                    ""orders_with_customers"": {
                        ""map"": ""function(doc) { if(doc.$doctype == 'order') {emit(doc._id, {'_id': doc._id, '_rev': doc._rev, 'Customer': doc.customer, 'OrderState': doc.orderState, 'OrderDate': doc.orderDate, 'OrderAmount': doc.orderAmount}); }}""   
                    }
                }
            }";
    }
}
