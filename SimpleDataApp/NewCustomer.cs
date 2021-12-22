using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyCouch;
using SimpleDataApp.Entities;

namespace SimpleDataApp
{
    public partial class NewCustomer : Form
    {
        public NewCustomer()
        {
            InitializeComponent();
        }

        // Storage for IDENTITY values returned from database.
        private string customerID;
        private string orderID;

        /// <summary>
        /// Verifies that the customer name text box is not empty.
        /// </summary>
        private bool IsCustomerNameValid()
        {
            if (txtCustomerName.Text == "")
            {
                MessageBox.Show("Please enter a name.");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Verifies that a customer ID and order amount have been provided.
        /// </summary>
        private bool IsOrderDataValid()
        {
            // Verify that CustomerID is present.
            if (txtCustomerID.Text == "")
            {
                MessageBox.Show("Please create customer account before placing order.");
                return false;
            }
            // Verify that Amount isn't 0.
            else if ((numOrderAmount.Value < 1))
            {
                MessageBox.Show("Please specify an order amount.");
                return false;
            }
            else
            {
                // Order can be submitted.
                return true;
            }
        }

        /// <summary>
        /// Clears the form data.
        /// </summary>
        private void ClearForm()
        {
            txtCustomerName.Clear();
            txtCustomerID.Clear();
            dtpOrderDate.Value = DateTime.Now;
            numOrderAmount.Value = 0;
            this.customerID = "";
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        private async void btnCreateAccount_Click(object sender, EventArgs e)
        {
            if (IsCustomerNameValid())
            {

                // Create the connection.
                using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
                {
                    var customer = new Entities.Customer()
                        {
                            _id = Guid.NewGuid().ToString(),
                            Name = txtCustomerName.Text,
                        };

                    this.customerID = customer._id;
                    txtCustomerID.Text = customer._id;

                    var response = await client.Entities.PutAsync(customer);

                    Console.Write(response.ToStringDebugVersion());
                }
            }
        }

        /// <summary>
        /// Calls the Sales.uspPlaceNewOrder stored procedure to place an order.
        /// </summary>
        private async void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            // Ensure the required input is present.
            if (IsOrderDataValid())
            {
                // Create the connection.
                using (var client = new MyCouchClient("http://admin:admin@localhost:5984/", "testdb"))
                {

                    var order = new Entities.Order()
                    {
                        _id = Guid.NewGuid().ToString(),
                        Customer = this.customerID,
                        OrderState = "O",
                        OrderDate = dtpOrderDate.Value.ToString(),
                        OrderAmount = (int)numOrderAmount.Value,
                    };

                    this.orderID = order._id;

                    var response = await client.Entities.PutAsync(order);

                    Console.Write(response.ToStringDebugVersion());
                }
            }
        }

        /// <summary>
        /// Clears the form data so another new account can be created.
        /// </summary>
        private void btnAddAnotherAccount_Click(object sender, EventArgs e)
        {
            this.ClearForm();
        }

        /// <summary>
        /// Closes the form/dialog box.
        /// </summary>
        private void btnAddFinish_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
