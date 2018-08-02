using System;
using System.Collections.Generic;

namespace ChickenSoftware.BusinessRules.ObjectOriented
{
    public class Order
    {
        public int Id
        {
            get;
            set;
        }

        public DateTime OpenedDate
        {
            get;
            set;
        }

        public DateTime ClosedDate
        {
            get;
            set;
        }

        public ICollection<OrderLineItem> LineItems
        {
            get;
            set;
        }

    }
}
