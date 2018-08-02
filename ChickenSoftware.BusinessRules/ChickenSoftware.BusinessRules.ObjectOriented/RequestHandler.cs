using System;
using System.IO;
using System.Web;
using Newtonsoft.Json;

namespace ChickenSoftware.BusinessRules.ObjectOriented
{
    public class RequestHandler : Handler
    {
        HttpRequest _request = null;
        Handler _successor = null;

        public RequestHandler(HttpRequest request)
        {
            _request = request;    
        }

        public override void Process()
        {
            var contents = String.Empty;
            try
            {
                using (var stream = _request.InputStream)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        contents = reader.ReadToEnd();
                    }
                }
                if (String.IsNullOrEmpty(contents))
                {
                    var customer = JsonConvert.DeserializeObject<Customer>(contents);
                    _successor.Process();

                }
            }
            catch (Exception ex)
            {
                Logger.Write("Something bad happened");
                throw new BuisnessRulesException("RequestHandler", ex);
            }

        }

        public override void SetSuccessor(Handler handler)
        {
            _successor = handler;
        }

    }
}
