namespace FS.SetMatching

/// Represents elements for subset matching
type Specification<'N when 'N: comparison and 'N: (static member (+) : 'N -> 'N -> 'N)> = 'N[] * 'N[]

/// Represents the minimal and maximal size of matching subsets.
type SearchRange = { LowerBound: option<int>; UpperBound: option<int> }

/// Represents pairs of matching subsets
type Solution<'N when 'N: comparison and 'N: (static member (+) : 'N -> 'N -> 'N)> = ('N[] * 'N[])[]

/// Represents an algorithm that finds pairs of mathing subsets
type Solver<'N when 'N: comparison and 'N: (static member (+) : 'N -> 'N -> 'N)> = Specification<'N> -> Solution<'N>
