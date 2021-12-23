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
using MyCouch.Contexts;
using SimpleDataAppDataLayer;

namespace SimpleDataAppUiLayer
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
                this.customerID = await DataAccess.CreateNewCustomer(txtCustomerName.Text);
                txtCustomerID.Text = this.customerID;
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
                this.orderID = await DataAccess.CreateNewOrder(this.customerID, dtpOrderDate.Value, (int)numOrderAmount.Value);
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
