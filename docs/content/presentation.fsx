open System.Threading
open System.Diagnostics

            // Input of value * changed
type Node = | Input of int ref * Event<unit>
            // Output of node1 * node2 * nodeFunction * initialValue * dirty * changed
            | Output of Node * Node * (int -> int -> int) * int ref * bool ref * Event<unit>

let rec eval (node : Node) : Async<int> =
    match node with
    |Input(n, _) -> async { return n.Value }
    |Output(n1,n2,f, initValue, dirty, _) ->
        async {
            if dirty.Value then
                let! v = Async.Parallel [ eval n1; eval n2 ]
                initValue := f v.[0] v.[1]
                dirty := false
            return initValue.Value }

let setValue (node : Node) (v : int) =
    match node with
    | Input(n, e) -> 
        n := v
        e.Trigger()
    | Output(_) -> failwith "cannot set value of output node"

let input i = Input(ref i, Event<unit>())
let func n1 n2 f = 
    let initValue = ref 0
    let dirty = ref true
    let event = Event<unit>()

    let getEvent (node : Node) = match node with Input(_, e) | Output(_, _, _, _, _, e) -> e
    (getEvent n1).Publish.Add(fun _ -> dirty := true; event.Trigger())
    (getEvent n2).Publish.Add(fun _ -> dirty := true; event.Trigger())

    Output(n1, n2, f, initValue, dirty, event)

let i1 = input 1
let i2 = input 3
let i3 = input 5

let n1 = func i1 i2 (fun x1 x2 -> printfn "*** eval n1, thread %i" Thread.CurrentThread.ManagedThreadId ; x1+x2)
let n2 = func i2 i3 (fun x1 x2 -> printfn "*** eval n2, thread %i" Thread.CurrentThread.ManagedThreadId ; x1+x2)
let n3 = func n1 n2 (fun x1 x2 -> printfn "*** eval n3, thread %i" Thread.CurrentThread.ManagedThreadId ; x1+x2)


let evalAsync (node : Node) =
    async { let! v = eval node in printfn "node value %i" v } |> Async.Start


evalAsync n3


setValue i1 100