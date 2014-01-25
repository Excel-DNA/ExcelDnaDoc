C# Example
----------

Snippet from C# example that can be found on GitHub site.

    [lang=csharp]
    using System;
    using Arcadia;
    using Data;

    public class OrderCalculationEngine : CalculationEngine, IOrderCalculationEngine
    {
        public OrderCalculationEngine(IDataService data)
            : base()
        {
            // inputs
            var inventory = AddInput(data.LoadInventory(), "Inventory");
            var order = AddInput(data.LoadOrder(), "Order");

            // outputs
            var orderResult = AddOutput(Tuple.Create(order, inventory),
                              new NodeFunc<Tuple<Order, Inventory>, OrderResult>(OrderMethods.GetOrderResults),
                              "OrderResult");

            Inventory = inventory;
            Order = order;
            OrderResult = orderResult;
        }

        public INode<Inventory> Inventory { get; private set; }

        public INode<Order> Order { get; private set; }

        public INode<OrderResult> OrderResult { get; private set; }

        public bool AutoCalculate
        {
            get { return this.Calculation.Automatic; }
            set { this.Calculation.Automatic = value; }
        }
    }