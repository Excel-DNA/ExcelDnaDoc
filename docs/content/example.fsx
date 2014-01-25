//(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"
#r "../../bin/Arcadia.dll"

// would normally



open Arcadia

open System
open System.ComponentModel
open Arcadia.ViewModel

type Product = 
    { ID : int
      Name : string
      UnitPrice : float }

type Inventory = 
    { Products : Product [] }

type OrderItem() = 
    inherit ObservableObject()
    let mutable productId = 0
    let mutable units = 0
    
    member this.ProductId 
        with get () = productId
        and set v = 
            productId <- v
            this.RaisePropertyChanged "ProductId"
    
    member this.Units 
        with get () = units
        and set v = 
            units <- v
            this.RaisePropertyChanged "Units"

type Order() = 
    inherit ObservableObject()
    let mutable id = 0
    let mutable date = DateTime.Now
    let mutable items = BindingList<OrderItem>()
    let mutable tax = 0.
    
    member this.ID 
        with get () = id
        and set v = 
            id <- v
            this.RaisePropertyChanged "ID"
    
    member this.Date 
        with get () = date
        and set v = 
            date <- v
            this.RaisePropertyChanged "Date"
    
    member this.Items 
        with get () = items
        and set v = 
            items <- v
            items.ListChanged.Add(fun _ -> this.RaisePropertyChanged "Items")
            this.RaisePropertyChanged "Items"
    
    member this.Tax 
        with get () = tax
        and set v = 
            tax <- v
            this.RaisePropertyChanged "Tax"

type OrderResult = 
    { TotalUnits : int
      PreTaxAmount : float
      TaxAmount : float
      TotalAmount : float }


module OrderMethods = 
    let getOrderResult(order : Order, inventory : Inventory) = 
        let price = seq [ for p in inventory.Products -> (p.ID, p.UnitPrice) ] |> dict
        let preTaxAmount = order.Items |> Seq.sumBy(fun i -> price.[i.ProductId] * float i.Units)
        { TotalUnits = order.Items |> Seq.sumBy(fun i -> i.Units)
          PreTaxAmount = preTaxAmount
          TaxAmount = preTaxAmount * order.Tax
          TotalAmount = preTaxAmount * (1. + order.Tax) }

type OrderCalculationEngine(data : IDataService) as this = 
    inherit CalculationEngine()
    
    // helper functions to add input/output nodes
    let input nodeId x = this.AddInput(x, nodeId)
    let output nodeId nodes f = this.AddOutput(nodes, NodeFunc(f), nodeId)

    // input backing fields

    let inventory = input "Inventory" <| data.LoadInventory()
    let order = input "Order" <| data.LoadOrder()

    // output backing fields

    let orderResult = output "OrderResult" (order, inventory) OrderMethods.getOrderResult

    // input nodes
    member this.Inventory = inventory
    member this.Order = order

    // output nodes
    member this.OrderResult = orderResult