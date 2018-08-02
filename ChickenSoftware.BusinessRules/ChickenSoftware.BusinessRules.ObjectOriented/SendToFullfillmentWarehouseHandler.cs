using System;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;

namespace ChickenSoftware.BusinessRules.ObjectOriented
{
    public class SendToFullfillmentWarehouseHandler: Handler
    {
        Handler _successor = null;
        Customer _customer = null;

        public SendToFullfillmentWarehouseHandler(Customer customer)
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
                using (var client = new WebClient())
                {
                    var uri = ConfigurationManager.AppSettings["FullFillmentUri"];
                    var json = JsonConvert.SerializeObject(_customer);
                    var response = client.UploadString(uri, json);
                    _successor.Process();
                }
            }
            catch (WebException ex)
            {
                Logger.Write("Something bad happened");
                throw new BuisnessRulesException("HandleFullfillment", ex);
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
