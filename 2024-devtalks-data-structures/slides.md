---
title: 'Immutable Data Structures'
layout: intro
background: 'img/intro-immutable-tree.png'
# Presentation Setup (for all option see: https://sli.dev/custom/#frontmatter-configures)
theme: ./theme
class: 'text-center'
highlighter: shiki # https://sli.dev/custom/highlighters.html
lineNumbers: true # show line numbers in code blocks
drawings:
  persist: false # persist drawings in exports and build
transition: none # no transition for better online screen sharing - or use "slide-left"
css: unocss
mdc: true # enable "Markdown Components" syntax: https://sli.dev/guide/syntax#mdc-syntax
routerMode: hash # more compatible with static site hosting
---

::date::
DevTalks 18.10.2024

::title::
# Immutable Data Structures

::description::
<div><strong>Jindřich Ivánek</strong></div>
<div>F# Expert at Ciklum</div>

<!--
Note
-->

---

# Immutable Data

## Definition
* no part of object can be changed after it's created

## Why use them?

<v-clicks>

- mutation is common source of bugs
- immutable data are easier to reason about
  - value passed to a function, can't be changed
  - easier refactoring
- immutable data structures are **thread-safe**
- bonus: memory efficient time travelling

</v-clicks>

<!--
What's Immutable data? One of definitions is that no part of the object cannot be changed after it's creation. That gives us many advantages.

Mutation is common source of bugs. Procedures changing data are order dependent and harder to refactor.

Immutable data are easier to reason about. Value passed to a function can't be changed - big part of uncertainty disappear.

Maybe most important: immutable data is thread-safe, no race condition possible.

As a bonus we got the ability to store history of changes in memory efficient way.

-->

---

TODO example?

---

# Immutable update
MYTH: to "change" immutable value, you need to copy the whole thing

<Transform :scale="0.7">
<img src ="img/meme.jpg"/>
</Transform>

<!--

There is common misconception that for every small update of immutable data we need to copy all the data and then make the change. And that making immutable data slow and memory expensive. That's not true, we can use clever data structures that can share parts of the structure between old and new value. And also it turns out that performance penalty is not that big.

-->

---

# How?
* we can share parts of the structure between old and new value
* **Structural sharing**

![Structural sharing](img/structural_sharing.png)

<!-- 
The trick is to use references inside the data structure to share parts of the structure.

In the rest of the talk I will show you two most common immutable data structures: linked list and set represented by balanced tree.
-->

---

# (Linked) list

```fsharp
let listA = [1; 2; 3]
let listA = 1 :: 2 :: 3 :: []
```

![Alt text](img/list1.png)

<!--
In linked list every item contains link to rest of the list.

We can insert new item only at front of list.

When we need to retrieve item at n-th position, we need to go through list.
-->

---

## (Linked) list sharing

```fsharp
let listA = [1; 2; 3]
let listA = 1 :: 2 :: 3 :: []
let listA2 = listA
let listB = 4 :: listA
let listB2 = [4] @ listA
```

<!-- ![width:1000px](img/linked_list_sharing.png) -->

```mermaid
graph LR
A("listA") --> 1 --> 2 --> 3 --> Nil("[]")
A2("listA2") --> 1
B("listB") --> 4 --> 1
B2("listB2") --> 4
```

<!--
When we append new item to list, we just link to original list from new item. Old list reference is still valid.

List don't have explicit functions to change item, instead we can go through sublists and link them to some new item.
-->

---

# List - update head

```fsharp
let listA = [1; 2; 3]
let listB = 4 :: List.tail listA
```

```mermaid
graph LR
A("listA") --> 1 --> 2 --> 3 --> Nil("[]")
B("listB") --> 4 --> 2
```

<!--
To change value of item, we create new item and link it to rest of list after original item.

To change value of item inside the list, we first split item into two parts, and join them with new item.
-->

---

![bg](img/terminusdb-commit-graph-diagram-regtech-1536x864.png)

<!--
Git in principle is just linked list (with few more features) and branches are just named references.  
-->

---

# List Benchmark

TODO

---

# Set
Unordered set of values

Typically implemented as a (balanced) tree

```fsharp
let s = [11; 20; 29; 32; 41; 50; 65; 72; 91; 99] |> set
```

![Example tree](img/set1.png)

---

## Insert = search + add

```fsharp
let s2 = s |> Set.add 35
```

![tree insert](img/tree-insert.gif)


<font color="grey"> <p align="right">source: https://visualgo.net/en/bst</p></font>


---

## Insert - structural sharing

```fsharp
let s2 = s |> Set.add 35
```

![tree sharing](img/set_after_insert.png)

---

## Building new Set

```fsharp
let s = [1; 7; 3; 9; 5; 6; 2; 8; 4] |> set
```

![bg contain](img/tree-inserts.gif)

<font color="grey"> <p align="right">source: https://visualgo.net/en/bst</p></font>

---

Set Benchmark

TODO

---

# Map
* Dictionary like immutable data structure
* Like `Set`, but with value linked with each key (node)

TODO image

---

## Map sharing

```fsharp
let mapA = Map.ofList [1, "A"; 2, "B"; 3, "C"]
let mapB = Map.ofList [1, "A"; 2, "B"; 3, "C"; 4, "D"]
let mapB2 = Map.add 4 "D" mapA
mapB = mapB2 // true
```

---

# Map Benchmark

TODO

---

# Records

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

  TODO image?

---

# Structural comparison in .NET

- definition of equality based on values, not references
- all F# data types have defined structural comparison and ordering
- Immutability and structural comparison are different features, but it is common that immutable data structures have defined structural comparison
  - same value with different references is more common when working with immutable data structures

---

# Questions?

---
layout: thank-you
---

# Thank you!

---
