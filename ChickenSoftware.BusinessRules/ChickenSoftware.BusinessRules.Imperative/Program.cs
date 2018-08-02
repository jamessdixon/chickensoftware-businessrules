using System;
using System.Web;

namespace ChickenSoftware.BusinessRules.Imperative
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            do
            {
                //listen for request here
                var request = new HttpRequest("", "", "");
                var orderHandler = new OrderHandler();
                orderHandler.HandleOrderRequest(request);
            } while (true);
        }
    }
}
