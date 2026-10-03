# The Singularity Workshop — Ontology

![Ontology architecture](docs/ontology-boundary.svg)

**Ontology is a MicroBundle, not a constraint on MicroBundles.**

This package provides independent contracts for defining, versioning, indexing, addressing, and relating ontological information. It deliberately does **not** require every MicroBundle to use The Singularity Workshop's ontology.

## Why this exists

The Workshop needs semantic structure that can scale beyond renderer-specific conditionals without turning that structure into a mandatory global schema.

A useful mental model is:

```text
meaning
   |
   v
semantic address
   |
   v
integer index
   |
   v
relationship
   |
   v
manifestation
```

For example, an application could express a fish's swimming concept as an ontology address and relate it to a swimming behavior or animation. The renderer can consume the relationship without becoming an animal-taxonomy engine.

Read the deeper design rationale in [docs/THEORY.md](docs/THEORY.md) and the implementation boundary in [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md).

## Integer indexing without a ProtocolAI dependency

Ontology uses a deliberately small, protocol-neutral indexing primitive: `OntologyIndex`.

The default representation is one integer:

```text
OntologyIndex(42)
```

When a semantic model benefits from layers or array-like coordinates, the same primitive expands to an ordered set of integer dimensions:

```text
OntologyIndex.Create(2, 14, 7, 3)

dimension 0 -> 2
dimension 1 -> 14
dimension 2 -> 7
dimension 3 -> 3
```

There is **no ProtocolAI package reference in Ontology**.

That is intentional. ProtocolAI can be used by a consumer as the vocabulary/identity layer that maps strings or protocol symbols to integer IDs, while Ontology only sees the resulting integer index:

```text
human vocabulary
      |
      v
ProtocolAI (optional consumer-side mapping)
      |
      v
integer ID
      |
      v
OntologyIndex
      |
      v
semantic address
```

This keeps Ontology useful with ProtocolAI, without making ProtocolAI part of Ontology's dependency boundary.

ProtocolAI's current symbols are explicitly assigned integer IDs; they are not required to be cryptographic hashes. If another vocabulary system uses hashes, database keys, generated IDs, or local array indexes, those values can also be represented by `OntologyIndex` where their semantics are appropriate.

## Single dimension by default, multiple dimensions by choice

The indexing model deliberately does not impose a nine-layer or five-layer ontology.

A simple ontology can remain:

```text
[concept]
   |
   +-- integer index: 42
```

A layered ontology can express the location as coordinates:

```text
[layer 0, layer 1, layer 2, layer 3, layer 4]

[2, 14, 7, 3, 91]
```

The number of dimensions is determined by the ontology using them. The core does not assume that every ontology has the same rank.

This gives us a compact runtime/storage representation while preserving the human-readable semantic path for authoring, diagnostics, and explanation.

## Design boundary

```text
MicroBundleDomain
    defines the generic MicroBundle mechanism
             |
             +-------------------+
             |                   |
          Ontology          application content
             |
      defines its own
      semantic structure
             |
       OntologyIndex
       scalar or N-D
```

A developer using `The.SingularityWorkshop.MicroBundleDomain` may build a five-layer model, a nine-layer model, no ontology at all, or an entirely different structure. Ontology exists to make semantic structure available when a developer chooses it.

## Coexisting ontologies

An application may load multiple independently defined ontologies at the same time. Ontologies are identified by an explicit ID and version; no ontology is silently promoted to a universal schema.

The Singularity Workshop can publish a master ontology as **content** that contains domains such as the animal kingdom, physics, materials, AEC, or other knowledge families. That master ontology remains an ontology definition, not a requirement imposed on unrelated MicroBundles.

Different ontologies can also choose different index dimensionality:

```text
Animals
  1 integer

AEC
  5 dimensions

Physics
  3 dimensions

Application-specific ontology
  whatever its model requires
```

## Semantic addresses

Ontology addresses allow behavior and data to be located by meaning rather than by renderer-specific conditionals:

```text
ontology://<ontology-id>/<version>/<semantic-path>
```

For example:

```text
ontology://42/1.0.0/life/animal/fish/locomotion/swim
```

The address can additionally carry its compact `OntologyIndex` without replacing the readable path.

An ontology relationship can connect that semantic concept to a manifestation such as a behavior, animation, simulation rule, or UI affordance.

## Dependency direction

- `TheSingularityWorkshop.Ontology` may depend on `TheSingularityWorkshop.MicroBundleDomain`.
- `MicroBundleDomain` must remain independent of Ontology.
- `Ontology` does **not** depend on ProtocolAI.
- ProtocolAI can be an optional consumer-side vocabulary/identity source for Ontology indexes.
- `MicroBundleRepository` stores/retrieves content without requiring ontology knowledge.
- `FSM_COS` may compose ontology MicroBundles but does not hard-code ontology-specific meaning.

This keeps the mechanism reusable while allowing richer semantic systems to grow above it.

## Address-space mathematics

The indexing model is intentionally backed by explicit mathematics rather than a vague claim of “large scale.”

A single `ulong` coordinate has:

$$
2^{64}
$$

possible values.

For N dimensions with extents $D_0, D_1, \\dots, D_{N-1}$, the address-space cardinality is:

$$
|A| = \\prod_{i=0}^{N-1} D_i
$$

If every dimension uses the complete 64-bit coordinate domain:

$$
|A| = (2^{64})^N = 2^{64N}
$$

That means a five-dimensional full-`ulong` coordinate space contains $2^{320}$ possible addresses, while a nine-dimensional space contains $2^{576}$.

Those are **addressable possibilities**, not an instruction to allocate an array that large. The ontology describes semantic space; repositories and runtimes materialize only the portions that actually exist.

`OntologyIndexSpace` makes the distinction concrete. It can:

- describe bounded N-dimensional extents;
- calculate exact cardinality with `BigInteger`;
- flatten a coordinate into a row-major dense-array offset;
- reverse that offset back into the semantic coordinate.

See **[Address-space mathematics](docs/MATHEMATICS.md)** and the [address-space visual](docs/address-space.svg).

## The N-dimensional model

The core remains one integer by default:

`OntologyIndex(42)`

When a domain needs layers or arrays, it expands without changing the identity primitive:

`OntologyIndex.Create(2, 14, 7, 3)`

The rank belongs to the consuming ontology. There is no Workshop-wide requirement that an ontology have five, nine, or any other number of layers.

## NuGet package

This repository is the canonical home of the Workshop's optional ontology contracts. The package is designed to be consumed independently by applications that choose semantic addressing.

The dependency direction remains deliberate:

```text
MicroBundleDomain
      |
      +----> Ontology
      |
      +----> application/domain MicroBundles

ProtocolAI --optional consumer-side vocabulary mapping--> OntologyIndex
FSM_COS ----composes declared pieces---------------------> runtime
MicroBundleRepository ----stores materialized artifacts-> storage
```

Ontology does **not** depend on ProtocolAI, FSM_COS, or the repository implementation.

## Ecosystem example

The intended use is not “make everything an ontology.”

It is:

1. define semantic structure where it adds value;
2. give that structure stable addresses;
3. use relationships to connect meaning across independently owned domains;
4. let FSM_COS compose the participating MicroBundles;
5. let renderers, simulations, UI, or other manifestations consume the resolved meaning;
6. materialize only the data and behavior that actually exists.

This makes the Ontology package a concrete example of the Workshop's broader principle:

> **Optional structure should remain optional, while the structures that do exist should be explicit, addressable, and composable.**

## Documentation map

- **[Theory](docs/THEORY.md)** — why semantic structure is optional and how meaning can become addressable.
- **[Architecture](docs/ARCHITECTURE.md)** — package boundaries and semantic resolution.
- **[Mathematics](docs/MATHEMATICS.md)** — N-dimensional address-space cardinality and dense-array mapping.
- **[Ontology boundary visual](docs/ontology-boundary.svg)** — dependency and ownership boundaries.
- **[Semantic resolution visual](docs/semantic-resolution.svg)** — meaning to manifestation.
- **[Address-space visual](docs/address-space.svg)** — scalar, N-dimensional, and linearized indexing.

---

**The Singularity Workshop**

[GitHub](https://github.com/TrentBest/TheSingularityWorkshop.Ontology) · [NuGet](https://www.nuget.org/packages/TheSingularityWorkshop.Ontology)

*Meaning can be addressable without making one ontology mandatory.*
