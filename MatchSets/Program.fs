namespace FS.SetMatching

open System
open Common

module Program =
    let testSet = [| 1; 1; 2; 3; 6; 7; -1; -1; -3; -5; -13 |]

    [<EntryPoint>]
    let main args =
        printfn "Find matching pairs of subsets from:"
        printfn "%A" testSet
        printfn ""
        let _positives, _negatives = splitBySign testSet
        let negatives = _negatives |> Array.map (fun x -> -x) |> Array.sortDescending
        let positives = _positives |> Array.sortDescending

        let oneOnes = Special.findOneOne (positives, negatives)
        let positives = oneOnes |> Array.map fst |> Array.collect id |> removeFrom positives
        let negatives = oneOnes |> Array.map snd |> Array.collect id |> removeFrom negatives

        let oneTwos = Special.findOneTwo (positives, negatives)
        let positives = oneTwos |> Array.map fst |> Array.collect id |> removeFrom positives
        let negatives = oneTwos |> Array.map snd |> Array.collect id |> removeFrom negatives

        let twoOnes' = Special.findOneTwo (negatives, positives)
        let positives = twoOnes' |> Array.map snd |> Array.collect id |> removeFrom positives
        let negatives = twoOnes' |> Array.map fst |> Array.collect id |> removeFrom negatives
        let twoOnes = twoOnes' |> Array.map (fun (x, y) -> y, x)

        let remainingMatches = BruteForce.solve { LowerBound = Some 2; UpperBound = None } (positives, negatives)
        let allMatches = Array.concat [oneOnes; oneTwos; twoOnes; remainingMatches]
        printfn "Matching pairs:"
        for (posMatch, negMatch) in allMatches do
            printfn "%A" (posMatch, negMatch |> Array.map (fun x -> -x))
        0
