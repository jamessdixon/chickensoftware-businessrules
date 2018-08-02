using System;
namespace ChickenSoftware.BusinessRules.Imperative
{
    public class OrderLineItem
    {
        public int Id
        {
            get;
            set;
        }

        public int OrderId
        {
            get;
            set;
        }

        public float BilledAmount
        {
            get;
            set;
        }

        public double Discount
        {
            get;
            set;
        }

        public float Tax
        {
            get;
            set;
        }

        public Product Product
        {
            get;
            set;
        }

    }
}
