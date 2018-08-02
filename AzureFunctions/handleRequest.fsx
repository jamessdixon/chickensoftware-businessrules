#r "System.Net.Http"
#r "Newtonsoft.Json"

open System.Net
open System.Net.Http
open Newtonsoft.Json

type Product = {Id: int; Sku: string}
type OrderLineItem = {Id:int; BilledAmount: float; Discount: float; Tax: float; Product: Product}
type Order = {Id: int; OpenedDate: DateTime; ClosedDate: DateTime; LineItems: OrderLineItem seq}
type Customer = {Id: int; FirstName: string; LastName: string; EMail: string; Order: Order; IsPreferred: Boolean}

let Run(req: HttpRequestMessage, customerQueueItem: byref<Customer>, badRequestQueueItem: byref<string>) =
    let contents = req.Content
    let jsonContent = contents.ReadAsStringAsync().Result
    try
        let customer = JsonConvert.DeserializeObject<Customer>(jsonContent)
        customerQueueItem <- customer 
        req.CreateResponse(HttpStatusCode.OK)
    with _ -> 
        badRequestQueueItem <- jsonContent 
        req.CreateResponse(HttpStatusCode.BadRequest)
