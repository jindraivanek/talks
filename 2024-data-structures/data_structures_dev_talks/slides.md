---
# try also 'default' to start simple
theme: default
# random image from a curated Unsplash collection by Anthony
# like them? see https://unsplash.com/collections/94734566/slidev
#background: images/immutable-tree-3061166569.png
# apply any windi css classes to the current slide
class: 'text-center'
# https://sli.dev/custom/highlighters.html
highlighter: shiki
# show line numbers in code blocks
lineNumbers: false
# some information about the slides, markdown enabled
info: |
  ## Immutable data structures presentation
# persist drawings in exports and build
drawings:
  persist: false
# use UnoCSS (experimental)
css: unocss
---

<style>
h1 {
  color: white;
  text-shadow: 2px 2px 4px #000000;
}
.imageText {
  font-size: 1.8rem;
  background-color: rgba(0, 0, 0, 0.2);;
  text-align: center;
  text-shadow: 2px 2px 4px #000000;
}
</style>

# Immutable Data Structures
![Alt text](img/immutable-tree-3061166569.png)

---

# Immutable Data Structures
   - why, how, structural sharing
   - Linked list (F# list)
   - Tree (F# Set, Map)
   - tuples, records, classes

---

# Immutable Data Structures
* no part of object can be changed after it's created

## Why?
* mutation is common source of bugs
* immutable data structures are easier to reason about
  - value passed to a function, can't be changed
* immutable data structures are thread-safe
* bonus: memory efficient time traveling

---

MYTH: to create new immutable value, you need to copy the whole thing

<Transform :scale="0.8">
<img src ="img/meme.jpg"/>
</Transform>

---

# How?
* we can share parts of the structure between old and new value
* **Structural sharing**

![Structural sharing](img/structural_sharing.png)

---


## (Linked) list

```fsharp
let listA = [1; 2; 3]
let listA = 1 :: 2 :: 3 :: []
```

![Alt text](img/list1.png)

---

## (Linked) list sharing

```fsharp
let listA = [1; 2; 3]
let listA = 1 :: 2 :: 3 :: []
let listA2 = listA
let listB = 4 :: listA
let listB2 = [4] @ listA
```

![width:1000px](img/linked_list_sharing.png)

---



![bg](img/terminusdb-commit-graph-diagram-regtech-1536x864.png)

---

# Set
Unordered set of values

Typically implemented as a (balanced) tree

```fsharp
let s = [11; 20; 29; 32; 41; 50; 65; 72; 91; 99] |> set
```

![Example tree](img/set1.png)

---

Insert = search + add

```fsharp
let s2 = s |> Set.add 35
```

![tree insert](img/tree-insert.gif)

from https://visualgo.net/en/bst

---

```fsharp
let s = [1; 7; 3; 9; 5; 6; 2; 8; 4] |> set
```

![bg contain](img/tree-inserts.gif)

from https://visualgo.net/en/bst

---

# Map
* Dictionary like immutable data structure
* Like `Set`, but with value linked with each key (node)

TODO image

---

```fsharp
let mapA = Map.ofList [1, "A"; 2, "B"; 3, "C"]
let mapB = Map.ofList [1, "A"; 2, "B"; 3, "C"; 4, "D"]
let mapB2 = Map.add 4 "D" mapA
mapB = mapB2 // true
```

---

## Records

```fsharp
{ Id: int; Name: string }
```

- Immutable by default
- No special immutable structure
- Update syntax create new record with not-changed fields shared with old record
  - ```fsharp
    { oldRecord with Name = "Bob" }
    ```
  - only reference is copied (except for *structs*)

---

# Structural comparison

- definition of equality based on values, not references
- all F# data types have defined structural comparison and ordering
- only few C# (compound) types have defined structural comparison and ordering
  - Tuples, Records, Array, ImmutableArray
- Immutability and structural comparison are different features, but it is common that immutable data structures have defined structural comparison
  - same value with different references are more common when working with immutable data structures

---

# QUESTIONS?