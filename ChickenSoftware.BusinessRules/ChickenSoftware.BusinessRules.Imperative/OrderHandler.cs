using System;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Data;
using System.Net.Mail;

namespace ChickenSoftware.BusinessRules.Imperative
{
    public class OrderHandler
    {
        public OrderHandler()
        {
        }

        public void HandleOrderRequest(HttpRequest request)
        {
            var contents = String.Empty;
            try
            {
                using (var stream = request.InputStream)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        contents = reader.ReadToEnd();
                    }
                }
                if (String.IsNullOrEmpty(contents))
                {
                    var customer = JsonConvert.DeserializeObject<Customer>(contents);
                    var order = customer.Order;
                    if (IsCustomerValid(customer))
                    {
                        if (IsCustomerInSystem(customer))
                        {
                            if (IsOrderItemsInStock(order))
                            {
                                ApplyOrderDiscount(customer);
                                SendEmailConfirmation(customer);
                                var status = SendToFullfillmentWarehouse(customer);
                                HandleFullfillmentStaus(customer, status);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Something bad happened");
                throw new BuisnessRulesException("HandleOrderRequest", ex);
            }
        }

        internal bool IsCustomerValid(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            if (String.IsNullOrEmpty(customer.FirstName))
                return false;
            if (string.IsNullOrEmpty(customer.LastName))
                return false;
            return true;
        }

        internal bool IsCustomerInSystem(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            var connectionString = ConfigurationManager.ConnectionStrings["database"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                var commandText = "usp_verifyInvoince";
                using (var command = new SqlCommand(commandText))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(customer.Id);
                    try
                    {
                        connection.Open();
                        return (bool)command.ExecuteScalar();
                    }
                    catch (SqlException)
                    {
                        return false;
                    }
                }
            }
        }

        internal bool IsOrderItemsInStock(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }
            try
            {
                using (var client = new WebClient())
                {
                    var uri = ConfigurationManager.AppSettings["WarehouseUri"];
                    foreach (var item in order.LineItems)
                    {
                        var response = client.DownloadString(uri + item.Id);
                        var inStock = JsonConvert.DeserializeObject<Boolean>(response);
                        if (!inStock) return false;
                    }
                }
                return true;
            }
            catch (WebException)
            {
                Logger.Write("Something bad happened");
                return false;
            }
        }

        internal void ApplyOrderDiscount(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            try
            {
                var attachmentPoint = Int32.Parse(ConfigurationManager.AppSettings["AttachmentPoint"]);
                var totalBill = customer.Order.LineItems.Sum(i => i.BilledAmount + i.Tax);
                if (totalBill > attachmentPoint && customer.IsPreferred)
                {
                    foreach (var item in customer.Order.LineItems)
                    {
                        item.Discount = item.BilledAmount * .95;
                    }
                }
            }
            catch (ConfigurationException ex)
            {
                Logger.Write("Something bad happened");
                throw new BuisnessRulesException("ApplyOrderDiscount", ex);

            }
        }
        internal void SendEmailConfirmation(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            var to = customer.Email;
            var from = ConfigurationManager.AppSettings["fromAddress"];
            var subject = String.Format("Your Order {0} From This Awesome Company Has Been Received", customer.Order.Id);
            var body = "We'll let you know when the items have shipped";
            var client = new SmtpClient();
            try
            {
                client.Send(from, to, subject, body);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Logger.Write("Something bad happened");
                throw new BuisnessRulesException("SendEmailConfirmation", ex);
            }
        }

        internal OrderStatus SendToFullfillmentWarehouse(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            try
            {
                using (var client = new WebClient())
                {
                    var uri = ConfigurationManager.AppSettings["FullFillmentUri"];
                    var json = JsonConvert.SerializeObject(customer);
                    var response = client.UploadString(uri, json);
                    return OrderStatus.Ok;
                }
            }
            catch (WebException)
            {
                Logger.Write("Something bad happened");
                return OrderStatus.Error;
            }
        }

        internal void HandleFullfillmentStaus(Customer customer, OrderStatus orderStatus)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }
            if(orderStatus == OrderStatus.Ok)
            {
                var to = customer.Email;
                var from = ConfigurationManager.AppSettings["fromAddress"];
                var subject = String.Format("Your Order {0} From This Awesome Company Has Been Shipped", customer.Order.Id);
                var body = "Enjoy your stuff";
                var client = new SmtpClient();
                try
                {
                    client.Send(from, to, subject, body);
                }
                catch (System.Net.Mail.SmtpException ex)
                {
                    Logger.Write("Something bad happened");
                    throw new BuisnessRulesException("HandleFullfillment", ex);
                }
            }
            else
            {
                //Notify Internal Department
            }
        }
    }
}
