using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyCouch;
using MyCouch.Requests;
using MyCouch.Responses;
using SimpleDataApp.Entities;

namespace SimpleDataApp
{
    public partial class FillOrCancel : Form
    {
        public FillOrCancel()
        {
            InitializeComponent();
            ShowOrders();
        }

        /// <summary>
        /// Show all orders
        /// </summary>
        private async void ShowOrders()
        {
            using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
            {
                var query = new QueryViewRequest("orders", "orders_with_customers").Configure(q => q
                    .Reduce(false));
                ViewQueryResponse<Entities.Order> result = await client.Views.QueryAsync<Entities.Order>(query);
                
                List<Entities.Order> orderList = new List<Entities.Order>();
                Dictionary<string,string> customerDict = new Dictionary<string, string>();
                HashSet<string> customerSet = new HashSet<string>();

                if (!result.IsEmpty && result.IsSuccess)
                {
                    foreach (var row in result.Rows)
                    {
                        Order order = row.Value;
                        orderList.Add(order);
                        customerSet.Add(order.Customer);
                    }
                }

                foreach (var customer in customerSet)
                {
                    var getEntityResponse = client.Entities.GetAsync<Customer>(customer);
                    var answer = getEntityResponse.Result.Content;
                    customerDict.Add(customer, answer.Name);
                }

                // Create a data table to hold the retrieved data.
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("id", typeof(string));
                dataTable.Columns.Add("Customer", typeof(String));
                dataTable.Columns.Add("State", typeof(string));
                dataTable.Columns.Add("Date", typeof(DateTime));
                dataTable.Columns.Add("Amount", typeof(int));

                foreach (var order in orderList)
                {
                    DataRow newRow = dataTable.Rows.Add();

                    newRow["id"] = order._id;
                    newRow["Customer"] = customerDict[order.Customer];
                    newRow["State"] = order.OrderState;
                    newRow["Date"] = order.OrderDate;
                    newRow["Amount"] = order.OrderAmount;
                }

                dgvCustomerOrders.DataSource = dataTable;
                dgvCustomerOrders.AutoResizeColumns();
            }
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        private async void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (dgvCustomerOrders.SelectedCells.Count > 0)
            {
                var selectedOrderId = dgvCustomerOrders.SelectedCells[0].Value.ToString();

                using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
                {
                    var getEntityResponse = await client.Entities.GetAsync<Order>(selectedOrderId);

                    if (getEntityResponse.IsSuccess)
                    {
                        var answer = getEntityResponse.Content;
                        var getDeleteResponse = await client.Documents.DeleteAsync(answer._id, answer._rev);

                        ShowOrders();
                    }
                }
            }
        }

        /// <summary>
        /// Fills an order 
        /// </summary>
        private async void btnFillOrder_Click(object sender, EventArgs e)
        {
            if (dgvCustomerOrders.SelectedCells.Count > 0)
            {
                var selectedOrderId = dgvCustomerOrders.SelectedCells[0].Value.ToString();

                using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
                {
                    var getEntityResponse = await client.Entities.GetAsync<Order>(selectedOrderId);

                    if (getEntityResponse.IsSuccess)
                    {
                        var answer = getEntityResponse.Content;

                        if (answer.OrderState == "O")
                        {
                            answer.OrderState = "X";

                            var response = await client.Entities.PutAsync(answer);
                            if (response.IsSuccess)
                            {
                                ShowOrders();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        private void btnFinishUpdates_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
