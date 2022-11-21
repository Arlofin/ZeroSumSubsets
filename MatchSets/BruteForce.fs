namespace FS.SetMatching

open System.Collections.Generic
open SetOperations

module BruteForce =
    let value = snd
    let inline private allSets (range: SearchRange) (spec: Specification<'N>) =
        let subsets (set: 'N[]) =
            seq {
                let iset = set |> Array.mapi (fun i x -> i, x)
                for n in (range.LowerBound |> Option.defaultValue 1)..(range.UpperBound |> Option.defaultValue set.Length) do
                    SetOperations.SubSets(iset, n)
            }
            |> Seq.concat
        let positiveSubsets = subsets (fst spec)
        let negativeSubsets = subsets (snd spec)
        SetOperations.CrossProduct([positiveSubsets; negativeSubsets])
        |> Seq.map (fun posNegPair -> posNegPair.[0], posNegPair.[1])
        |> Seq.filter (fun (positives, negatives) ->
            Array.sumBy value positives = Array.sumBy value negatives)
        |> Seq.toArray

    let inline private selectOptimal (ambigousSolution : ((int * 'N)[] * (int * 'N)[])[]) : Solution<'N> =
        let sortedPairs = ambigousSolution |> Array.sortBy (fun (positives, negatives) -> positives.Length + negatives.Length)
        let consumedPositives = HashSet<int * 'N>()
        let consumedNegatives = HashSet<int * 'N>()
        let selectedPairs = List<'N[] * 'N[]>()
        for (positives, negatives) in sortedPairs do
            if positives |> Array.forall (fun pos -> not (consumedPositives.Contains pos)) && negatives |> Array.forall (fun neg -> not (consumedNegatives.Contains neg)) then
                selectedPairs.Add (positives |> Array.map value, negatives |> Array.map value)
                for pos in positives do consumedPositives.Add pos |> ignore
                for neg in negatives do consumedNegatives.Add neg |> ignore
        selectedPairs
        |> Seq.toArray

    let inline solve (range: SearchRange) (spec: Specification<'N>) : Solution<'N> =
        spec
        |> allSets range
        |> selectOptimal
