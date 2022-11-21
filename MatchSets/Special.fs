namespace FS.SetMatching

open System.Collections.Generic

module Special =
    let inline findOneOne (spec: Specification<'N>) : Solution<'N> =
        let positivesEnumerator = ((fst spec) :> IEnumerable<'N>).GetEnumerator()
        let negativesEnumerator = ((snd spec)  :> IEnumerable<'N>).GetEnumerator()
        let matches = List<'N>()
        let mutable hasNext = positivesEnumerator.MoveNext() && negativesEnumerator.MoveNext()
        while hasNext do
            if positivesEnumerator.Current = negativesEnumerator.Current then
                matches.Add positivesEnumerator.Current
                hasNext <- positivesEnumerator.MoveNext() && negativesEnumerator.MoveNext()
            elif positivesEnumerator.Current > negativesEnumerator.Current then
                hasNext <- positivesEnumerator.MoveNext()
            else
                hasNext <- negativesEnumerator.MoveNext()
        matches
        |> Seq.map (fun x -> [| x |], [| x |])
        |> Seq.toArray

    let inline findOneTwo (spec: Specification<'N>) : Solution<'N> =
        let positives = fst spec
        let negatives = snd spec
        let matches = List<'N[] * 'N[]>()
        let mutable i = 0
        let mutable jUp = 0
        while i < positives.Length && jUp < negatives.Length - 1 do
            let jUp = Common.binarySearchUp negatives (jUp, negatives.Length - 1) positives.[i]
            if jUp < negatives.Length then
                let mutable jDown = negatives.Length - 1
                let mutable j = jUp
                let mutable found = false
                while j < negatives.Length && not found do
                    let gap = positives.[i] - negatives.[j]
                    let j2 = Common.binarySearchDown negatives (0, jDown) gap
                    if j2 >= 0 && negatives.[j2] = gap && j2 <> j then
                        matches.Add ([| positives.[i] |], [| negatives.[j]; gap |])
                        found <- true
                        jDown <- j
                    j <- j + 1
            i <- i + 1
        matches
        |> Seq.toArray
