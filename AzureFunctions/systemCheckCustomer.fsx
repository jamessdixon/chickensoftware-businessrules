#r "Newtonsoft.Json"
#r "System.Data"
#r "System.Configuration"

open System.Data
open Newtonsoft.Json
open System.Configuration
open System.Data.SqlClient

type Product = {Id: int; Sku: string}
type OrderLineItem = {Id:int; BilledAmount: float; Discount: float; Tax: float; Product: Product}
type Order = {Id: int; OpenedDate: DateTime; ClosedDate: DateTime; LineItems: OrderLineItem seq}
type Customer = {Id: int; FirstName: string; LastName: string; EMail: string; Order: Order; IsPreferred: Boolean}

let Run(customer: Customer, customerInSystemQueueItem:byref<Customer>,customerNotInSystemQueueItem: byref<Customer>) =
    try
        let connectionString = ConfigurationManager.AppSettings.["databaseConnectionString"]
        use connection = new SqlConnection(connectionString)
        let commandText = "usp_verifyCustomer"
        use command = new SqlCommand(commandText,connection)
        command.CommandType <- CommandType.StoredProcedure
        command.Parameters.AddWithValue("@customerId",customer.Id) |> ignore
        connection.Open()
        let isInSystem = command.ExecuteScalar() :?> int
        match isInSystem with
        | 1 -> customerInSystemQueueItem <- customer
        | 0 ->  customerNotInSystemQueueItem <- customer
    with _ -> customerNotInSystemQueueItem <- customer


