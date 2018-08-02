using System;
using System.Configuration;
using System.Net.Mail;

namespace ChickenSoftware.BusinessRules.ObjectOriented
{
    public class SendEmailConfirmationHandler: Handler
    {
        Handler _successor = null;
        Customer _customer = null;

        public SendEmailConfirmationHandler(Customer customer)
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
            var subject = String.Format("Your Order {0} From This Awesome Company Has Been Received", _customer.Order.Id);
            var body = "We'll let you know when the items have shipped";
            var client = new SmtpClient();
            try
            {
                client.Send(from, to, subject, body);
                _successor.Process();
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Logger.Write("Something bad happened");
                throw new BuisnessRulesException("SendEmailConfirmationHandler", ex);
            }

        }

        public override void SetSuccessor(Handler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            _successor = handler;  
        }
    }
}
