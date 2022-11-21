Zero Sum Subset Search
======================

Given a set of numbers (resp. of any additive, comparable elements) find a maximal incomplete partition such that each member of the partition (i.e. subset of the numbers) sums up to zero.
Thereby we allow sets with duplicates.
The solution is not necessarily unique. We favour partitions consisting of smaller subsets.

Internally, we slightly redefine this task by splitting the numbers into the positive numbers P and the absolute value of the negative numbers N,
and search for maximal (incomplete) "parallel partitions" {(p_i, n_i)} such that each p_i is a subset of P and each n_i a subset of N,
the p_i are pairwise disjoint as well as the n_i,
and the sum over the elements of p_i equals the sum over the elements of n_i. This redefinition has the advantage of being symmetric in P and N.

We do not know an efficient general algorithm. Therefore we apply the following heuristic approach:

1. Sort P and N descendingly.
2. Find the elements that are equal on both sides, making use of the order. Put them aside and remove them from P resp. N.
3. For each element x in P, search the largest element y in N with x > y. If such exists, search descendingly through elements y' in N smaller or equal to y until x-y' is found in N\\{y'} (or no elements remain).
   If found, {x} and {y, x-y'} are put aside, x is removed from P and y, x-y' are removed from N.
4. Repeat the previous step with P and N interchanged.
5. Brute force search all remaining subsets (up to a depth that allows the algorithm to finish in reasonable time). If an element occurs in more than one
   subset, prefer the one with minimal size.

Example
-------

The set

{1, 1, 2, 3, 6, 7, -1, -1, -3, -5, -13}

has (at least) two maximal partitions:

{1 - 1, 1 - 1, 3 - 3, 6 + 7 - 13 }

and

{1 - 1, 1 - 1, 2 + 3 - 5, 6 + 7 - 13 }
