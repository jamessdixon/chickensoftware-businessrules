#r "Newtonsoft.Json"
#r "System.Configuration"
#r "System.Net"

open System.Net
open Newtonsoft.Json
open System.Configuration

type Product = {Id: int; Sku: string}
type OrderLineItem = {Id:int; BilledAmount: float; Discount: float; Tax: float; Product: Product}
type Order = {Id: int; OpenedDate: DateTime; ClosedDate: DateTime; LineItems: OrderLineItem seq}
type Customer = {Id: int; FirstName: string; LastName: string; EMail: string; Order: Order; IsPreferred: Boolean}

let Run(customer: Customer, itemsInSystemQueueItem:byref<Customer>,itemsNotInSystemQueueItem:byref<Customer>) =
    try
        use client = new WebClient()
        let uri = ConfigurationManager.AppSettings.["WarehouseUri"];
        let isAnyOutOfStock =
            customer.Order.LineItems
            |> Seq.map(fun li -> client.DownloadString(uri + li.Id.ToString()))
            |> Seq.map(fun r -> JsonConvert.DeserializeObject<Boolean>(r))
            |> Seq.contains(false)
        match isAnyOutOfStock with
        | true -> itemsNotInSystemQueueItem <- customer
        | false -> itemsInSystemQueueItem <- customer
    with _ -> itemsNotInSystemQueueItem <- customer
    
    
