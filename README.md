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

## Documentation

- **[Theory](docs/THEORY.md)** — the reasoning and design principles behind optional, addressable semantics.
- **[Architecture](docs/ARCHITECTURE.md)** — package boundaries, compact indexing, semantic resolution, coexistence, and evolution.
- **[Architecture visual](docs/ontology-boundary.svg)** — a visual map of the package boundary.
- **[Semantic resolution visual](docs/semantic-resolution.svg)** — a visual from semantic meaning to manifestation.

## Status

This package is an early public contract. The initial release establishes the architectural boundary and intentionally keeps the model small.

The integer indexing model is intentionally protocol-neutral: it gives the runtime a compact scalar or multidimensional representation without forcing every consumer to adopt the same vocabulary system.
