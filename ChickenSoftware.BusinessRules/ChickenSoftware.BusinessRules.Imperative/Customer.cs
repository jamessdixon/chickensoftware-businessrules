using System;
namespace ChickenSoftware.BusinessRules.Imperative
{
    public class Customer
    {
        public int Id
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
        public Order Order
        {
            get;
            set;
        }
        public bool IsPreferred
        {
            get;
            set;
        }
   }
}
