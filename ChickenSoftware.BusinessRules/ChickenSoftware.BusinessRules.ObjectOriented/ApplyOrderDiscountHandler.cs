using System;
using System.Configuration;
using System.Linq;

namespace ChickenSoftware.BusinessRules.ObjectOriented
{
    public class ApplyOrderDiscountHandler: Handler
    {
        Handler _successor = null;
        Customer _customer = null;

        public ApplyOrderDiscountHandler(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }
            _customer = customer;
        }

        public override void Process()
        {
            try
            {
                var attachmentPoint = Int32.Parse(ConfigurationManager.AppSettings["AttachmentPoint"]);
                var totalBill = _customer.Order.LineItems.Sum(i => i.BilledAmount + i.Tax);
                if (totalBill > attachmentPoint && _customer.IsPreferred)
                {
                    foreach (var item in _customer.Order.LineItems)
                    {
                        item.Discount = item.BilledAmount * .95;
                    }
                }
                _successor.Process();
            }
            catch (ConfigurationException ex)
            {
                Logger.Write("Something bad happened");
                throw new BuisnessRulesException("ApplyOrderDiscountHandler", ex);
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
