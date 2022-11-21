namespace FS.SetMatching

module Common =
    let inline splitBySign (mixedSet: 'N[]) : ('N[] * 'N[]) =
        mixedSet
        |> Array.partition (fun el -> el > LanguagePrimitives.GenericZero)

    let removeFrom (orderedSet: 'N[]) (orderedSubset: 'N[]) =
        if orderedSubset.Length = 0 then
            orderedSet
        else
            let reducedSet = Array.zeroCreate (orderedSet.Length - orderedSubset.Length)
            let mutable j = 0
            for i in 0..orderedSet.Length-1 do
                if j < orderedSubset.Length then
                    if orderedSet.[i] = orderedSubset.[j] then
                        j <- j + 1
                    elif orderedSet.[i] < orderedSubset.[j] then
                        reducedSet.[i - j] <- orderedSet.[i]
                        j <- j + 1
                    else
                        reducedSet.[i - j] <- orderedSet.[i]
                else
                    reducedSet.[i - j] <- orderedSet.[i]
            reducedSet

    /// In a descendingly sorted array, returns the index of
    /// the largest element smaller or equal to the specified value
    /// with index in the specified range.
    /// If this does not exist returns the upper bound plus 1.
    let rec binarySearchUp (values: 'N[]) (l: int, u: int) (value: 'N) =
        if u = l then
            if value >= values.[u] then
                u
            else
                u + 1
        else
            let m = l + (u - l) / 2
            if value = values.[m] then
                m
            elif value > values.[m] then
                binarySearchUp values (l, m) value
            else
                binarySearchUp values (System.Math.Max(m, l+1), u) value

    /// In a descendingly sorted array, returns the index of
    /// the smallest element larger or equal to the specified value
    /// with index in the specified range.
    /// If this does not exist returns -1.
    let rec binarySearchDown (values: 'N[]) (l: int, u: int) (value: 'N) =
        if u = l then
            if value <= values.[u] then
                u
            else
                u - 1
        else
            let m = l + (u - l) / 2
            if value = values.[m] then
                m
            elif value < values.[m] then
                binarySearchDown values (m+1, u) value
            else
                binarySearchDown values (l, m) value
