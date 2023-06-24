# Tower of Hanoi

This is one of the first algorithms everyone learns when first hearing about recursive programming. It is a perfect example of how to assume of some base case and operate based on it.

The pseudocode for the recursive algorithm is

```pseudo
func hanoi(N, source, dest, helper):
    Move N-1 disks from source to helper using dest
    Move 1 disk from source to dest
    Move N-1 disks from helper to dest using source
```