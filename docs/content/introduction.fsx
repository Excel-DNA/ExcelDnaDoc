(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use 
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"
#r "../../bin/Arcadia.dll"

(**
Introduction to Arcadia
=======================

Arcadia is an asynchronous calculation framework inspired by a discussion by Tobias Gedell on Eden <a href="http://www.youtube.com/watch?v=BsOtAXV_URI">YouTube video</a>.  

The main points of the discussion on Eden that stuck with me were :  

1. Laziness and partial recalc  
2. Caching  
3. Asynchronous result production  
4. Automatic parallelization  
5. Optional manual calculation  
6. Cancellation  

Currently I have implemented the above plus basic error handling (changes the node with error to an Error status, no logging of error currently.)  

**TO DO LIST**  
logging  
redo/undo  
serialization/persistense of ``CalculationEngine`` to database  
  
Arcadia is implemented using .Net generics so calculation "nodes" do not need to implement just a single numberic value.  Inputs/Outputs can be any POCO/recordset/struct that you want.  

Node Dependency Graph
=====================

Here is a dependency graph with input nodes (green) and output nodes (blue).  We will use this as an illustration of the dependency tree that we will now try to replicate using simple integer based nodes.  

<img src="img/NodeGraph.png" height="300" width="600" />


F# Example - simple integers
============================

First lets define some simple functions to represent some slow running functions.  

*)

open System.Threading

let add2 (x1,x2) = 
    Thread.Sleep 500
    x1 + x2

let add3 (x1,x2,x3) =
    Thread.Sleep 1000
    x1 + x2 + x3

(**
Now lets create a calculation engine that does simple addition at nodes based on the dependency graph we saw earlier. An optional custom ID can be assigned to a node.
If no node ID is given then ``InputNodes`` will be named in0, in1, in2, ... and ``OutputNodes`` will be named out0, out1, out2, ...
*)

open Arcadia

type SimpleCalcEngine() as this =
    inherit CalculationEngine()

    let input v = this.AddInput v
    let output nodes func = this.AddOutput(nodes, NodeFunc(func))

    // input nodes
    let in0 = input 1
    let in1 = input 1
    let in2 = input 1
    let in3 = input 1
    let in4 = input 1
    let in5 = input 1
    let in6 = input 1
    let in7 = input 1
    let in8 = input 1
    let in9 = input 1
    let in10 = input 1
    let in11 = input 1
    let in12 = input 1
    
    // output nodes
    let out0 = output (in0,in1) add2
    let out1 = output (in2,in3) add2
    let out2 = output (in4,in5,in6) add3
    let out3 = output (in7,in8) add2
    let out4 = output (out1,out2) add2
    let out5 = output (out0,out3) add2
    let out6 = output (in9,in10) add2
    let out7 = output (in11,in12) add2
    let out8 = output (out4,out6) add2
    let out9 = output (out5,out7,out8) add3
    let out10 = output (out0,out5) add2

(**
Test out our Calculation Engine
-------------------------------

Create an instance of the calculation engine and turn on automatic calculations. 
Run the following a statement at a time and see how it works.  
*)

let ce = SimpleCalcEngine()

/// print out the status and value of a given node.
let nodeValue(nodeId) = 
    let n = ce.Node<int>(nodeId)
    printfn "%s status:%A value:%i" (n.Id) (n.Status) (n.Value)

nodeValue "out9" // returns "out9 status:Dirty value:0"

ce.Calculation.Automatic <- true

// check again (will need to wait a few seconds while calculations complete)
nodeValue "out9" // returns "out9 status:Valid value:13"

(**
You can also do manual calculations if you didn't want to have everything calculating automatically.
*)
// set calculations back to manual
ce.Calculation.Automatic <- false

// set the value of in1 to 3 
ce.Node("in1").Value <- 3

// check the value of nodes dependent on in1
nodeValue "out9" // returns out9 status: Dirty value:13
nodeValue "out10" // returns out10 status: Dirty value: 6

// if we want to get the updated value we can request an update
ce.Node<int>("out9").AsyncCalculate()

// wait a couple of seconds (or not and see a Dirty result for out9)
nodeValue "out9" // returns out9 status: Valid value:15
nodeValue "out10" // returns out10 status: Dirty value: 6

(**
Since out9 does not depend on out10 it did not recalculate (point 1 from our starting list).

[Here](csharp_simple.html) is the above example implemented in C#.

An example of how this can be implemented in an MVVM application can be found on the GitHub site in the src/Samples folder.
*)