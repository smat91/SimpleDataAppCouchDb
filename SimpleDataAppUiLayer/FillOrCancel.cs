using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using SimpleDataAppDataLayer;
using SimpleDataAppDataLayer.Entities;

namespace SimpleDataAppUiLayer
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
            List<Order> orderList = await DataAccess.GetOrdersAsList();
            Dictionary<string, string> customerDict = await DataAccess.GetCustomersFromOderList(orderList);

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

        /// <summary>
        /// Cancels an order
        /// </summary>
        private async void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (dgvCustomerOrders.SelectedCells.Count > 0)
            {
                var selectedOrderId = dgvCustomerOrders.SelectedCells[0].Value.ToString();
                var response = await DataAccess.DeleteOrderById(selectedOrderId);
                if (response)
                {
                    ShowOrders();
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
                var response = await DataAccess.FillOrderById(selectedOrderId);
                if (response)
                {
                    ShowOrders();
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
