#r "Newtonsoft.Json"

open Newtonsoft.Json

type Product = {Id: int; Sku: string}
type OrderLineItem = {Id:int; BilledAmount: float; Discount: float; Tax: float; Product: Product}
type Order = {Id: int; OpenedDate: DateTime; ClosedDate: DateTime; LineItems: OrderLineItem seq}
type Customer = {Id: int; FirstName: string; LastName: string; EMail: string; Order: Order; IsPreferred: Boolean}

let Run(customer: Customer, validCustomerQueueItem: byref<Customer>, badCustomerQueueItem: byref<Customer>) =
    let invalidFirstName = String.IsNullOrEmpty(customer.FirstName)
    let invalidLastName = String.IsNullOrEmpty(customer.LastName)
    match invalidFirstName, invalidLastName with
    | false, false -> validCustomerQueueItem <- customer
    | _, _ -> badCustomerQueueItem <- customer
