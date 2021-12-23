using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCouch;
using MyCouch.Requests;
using MyCouch.Responses;
using SimpleDataAppDataLayer.Entities;
using SimpleDataAppDataLayer.Views;

namespace SimpleDataAppDataLayer
{
    public class DataAccess
    {
        public static async void InitViews()
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
            {
                var getResponse = await client.Documents.GetAsync("_design/orders");
                Console.WriteLine(getResponse);
                if (getResponse.IsEmpty)
                {
                    var putResponse = await client.Documents.PostAsync(DbViews.OrderList);
                    Console.WriteLine(putResponse.ToStringDebugVersion());
                }
            }
        }

        public static async Task<string> CreateNewCustomer(string customerName)
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
            {
                var customer = new Entities.Customer()
                {
                    _id = Guid.NewGuid().ToString(),
                    Name = customerName,
                };

                var response = await client.Entities.PutAsync(customer);
                Console.Write(response.ToStringDebugVersion());

                return customer._id;
            }
        }

        public static async Task<string> CreateNewOrder(string customerId, DateTime oderDate, int oderAmount)
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
            {

                var order = new Entities.Order()
                {
                    _id = Guid.NewGuid().ToString(),
                    Customer = customerId,
                    OrderState = "O",
                    OrderDate = oderDate.ToString(),
                    OrderAmount = oderAmount,
                };

                var response = await client.Entities.PutAsync(order);
                Console.Write(response.ToStringDebugVersion());

                return order._id; 
            }
        }

        public static async Task<List<Order>> GetOrdersAsList()
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
            {
                var query = new QueryViewRequest("orders", "orders_with_customers").Configure(q => q
                    .Reduce(false));
                ViewQueryResponse<Entities.Order> response = await client.Views.QueryAsync<Entities.Order>(query);
                Console.WriteLine(response);

                List<Entities.Order> orderList = new List<Entities.Order>();

                if (!response.IsEmpty && response.IsSuccess)
                {
                    foreach (var row in response.Rows)
                    {
                        Order order = row.Value;
                        orderList.Add(order);
                    }
                }
                return orderList;
            }
        }

        public static async Task<Dictionary<string, string>> GetCustomersFromOderList(List<Order> orderList)
        {
            HashSet<string> customerSet = new HashSet<string>();
            Dictionary<string, string> customerDict = new Dictionary<string, string>();

            if (orderList == null)
            {
                return customerDict;
            }

            foreach (var order in orderList)
            {
                customerSet.Add(order.Customer);
            }

            using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
            {
                foreach (var customer in customerSet)
                {
                    var response = client.Entities.GetAsync<Customer>(customer);
                    Console.WriteLine(response);
                    var answer = response.Result.Content;
                    customerDict.Add(customer, answer.Name);
                }

                return customerDict;
            }
        }

        public static async Task<bool> DeleteOrderById(string orderId)
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
            {
                var getEntityResponse = await client.Entities.GetAsync<Order>(orderId);
                Console.WriteLine(getEntityResponse);

                if (getEntityResponse.IsSuccess)
                {
                    var answer = getEntityResponse.Content;
                    var getDeleteResponse = await client.Documents.DeleteAsync(answer._id, answer._rev);
                    Console.WriteLine(getDeleteResponse);

                    return true;
                }

                return false;
            }
        }

        public static async Task<bool> FillOrderById(string orderId)
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
            {
                var getEntityResponse = await client.Entities.GetAsync<Order>(orderId);
                Console.WriteLine(getEntityResponse);

                if (getEntityResponse.IsSuccess)
                {
                    var answer = getEntityResponse.Content;

                    if (answer.OrderState == "O")
                    {
                        answer.OrderState = "X";
                        var response = await client.Entities.PutAsync(answer);
                        Console.WriteLine(response);

                        return response.IsSuccess;
                    }
                    return false;
                }
                return false;
            }
        }
    }
}
