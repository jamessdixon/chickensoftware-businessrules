using System;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace ChickenSoftware.BusinessRules.ObjectOriented
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //https://www.dofactory.com/net/chain-of-responsibility-design-pattern
            //https://www.codeproject.com/Articles/743783/Reusable-Chain-of-responsibility-in-Csharp
            Console.WriteLine("Starting");
            do
            {
                var request = new HttpRequest("", "", "");
                var customer = CreateCustomer(request);
                var handler1 = new ValidCustomerHandler(customer);
                var handler2 = new CustomerInSystemHandler(customer);
                var handler3 = new OrderItemsInStockHandler(customer.Order);
                var handler4 = new ApplyOrderDiscountHandler(customer);
                var handler5 = new SendEmailConfirmationHandler(customer);
                var handler6 = new SendToFullfillmentWarehouseHandler(customer);
                var handler7 = new HandleFullfillmentStausHandler(customer);

                handler1.SetSuccessor(handler2);
                handler2.SetSuccessor(handler3);
                handler3.SetSuccessor(handler4);
                handler4.SetSuccessor(handler5);
                handler5.SetSuccessor(handler6);
                handler6.SetSuccessor(handler7);

                handler1.Process();

            } while (true);
        }

        internal static Customer CreateCustomer(HttpRequest request)
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
                    return JsonConvert.DeserializeObject<Customer>(contents);

                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                Logger.Write("Something bad happened");
                return null;
            }
        }

    }
}
