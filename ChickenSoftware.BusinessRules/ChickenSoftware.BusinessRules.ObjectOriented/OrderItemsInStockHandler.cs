using System;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;

namespace ChickenSoftware.BusinessRules.ObjectOriented
{
    public class OrderItemsInStockHandler: Handler
    {
        Handler _successor = null;
        Order _order = null;

        public OrderItemsInStockHandler(Order order)
        {
            if(order == null)
            {
                throw new ArgumentNullException("order");
            }
            _order = order;
        }

        public override void Process()
        {
            try
            {
                using (var client = new WebClient())
                {
                    var uri = ConfigurationManager.AppSettings["WarehouseUri"];
                    foreach (var item in _order.LineItems)
                    {
                        var response = client.DownloadString(uri + item.Id);
                        var inStock = JsonConvert.DeserializeObject<Boolean>(response);
                        if (!inStock)
                        {
                            Logger.Write("Something bad happened");
                        }
                        else
                        {
                            _successor.Process();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Logger.Write("Something bad happened");
                throw new BuisnessRulesException("OrderItemsInStockHandler", ex);
            }
        }

        public override void SetSuccessor(Handler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
            _successor = handler;        }
    }
}
