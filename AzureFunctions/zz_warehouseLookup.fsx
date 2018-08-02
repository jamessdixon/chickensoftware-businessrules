#r "System.Net.Http"
#r "Newtonsoft.Json"

open System.Net
open System.Net.Http
open Newtonsoft.Json

let Run(req: HttpRequestMessage) =
    async {
        let id =
            req.GetQueryNameValuePairs()
            |> Seq.tryFind (fun q -> q.Key = "id")

        let random = new System.Random();
        let randomNumber = random.Next(100);
        match randomNumber < 75 with
        | true -> return req.CreateResponse(HttpStatusCode.OK, "true");
        | false -> return req.CreateResponse(HttpStatusCode.OK, "false");
        
    } |> Async.RunSynchronously
