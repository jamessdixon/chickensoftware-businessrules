using System;
using System.Configuration;
using System.Net.Mail;

namespace ChickenSoftware.BusinessRules.ObjectOriented
{
    public class HandleFullfillmentStausHandler: Handler
    {
        Handler _successor = null;
        Customer _customer = null;

        public HandleFullfillmentStausHandler(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }
            _customer = customer;
        }

        public override void Process()
        {
            var to = _customer.Email;
            var from = ConfigurationManager.AppSettings["fromAddress"];
            var subject = String.Format("Your Order {0} From This Awesome Company Has Been Shipped", _customer.Order.Id);
            var body = "Enjoy your stuff";
            var client = new SmtpClient();
            try
            {
                client.Send(from, to, subject, body);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Logger.Write("Something bad happened");
                throw new BuisnessRulesException("HandleFullfillmentStausHandler", ex);
            }

        }

        public override void SetSuccessor(Handler handler)
        {
            //Liskov Violation
        }
    }
}
