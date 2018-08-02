using System;
namespace ChickenSoftware.BusinessRules.ObjectOriented
{
    public class ValidCustomerHandler : Handler
    {
        Handler _successor = null;
        Customer _customer = null;

        public ValidCustomerHandler(Customer customer)
        {
            if (customer == null)
            {
                throw new ArgumentNullException("customer");
            }
            _customer = customer;
        }

        public override void Process()
        {
            if (String.IsNullOrEmpty(_customer.FirstName))
                Logger.Write("Something Went Wrong");
            if (string.IsNullOrEmpty(_customer.LastName))
                Logger.Write("Something Went Wrong");
            _successor.Process();
        }

        public override void SetSuccessor(Handler handler)
        {
            if(handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            _successor = handler;
        }
    }
}
