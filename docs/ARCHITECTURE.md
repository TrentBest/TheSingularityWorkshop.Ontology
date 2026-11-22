# Ontology Architecture

![Ontology boundary](ontology-boundary.svg)

The package sits above the generic MicroBundle mechanism and beside application/domain content. It does not become the authority for every MicroBundle.

## Boundary map

```text
MicroBundleDomain
       |
       +---- Ontology ---- semantic definitions
       |
       +---- Domain MicroBundles ---- application meaning

FSM_COS composes the resulting pieces without owning their ontology.
```

| Concern | Owner | Ontology dependency |
|---|---|---|
| MicroBundle lifecycle and contracts | MicroBundleDomain | None |
| Semantic definitions and addresses | Ontology | MicroBundleDomain is allowed |
| Physical storage and retrieval | MicroBundleRepository | None required |
| Runtime composition | FSM_COS | May compose ontology bundles |
| Domain meaning | Application/domain MicroBundles | Optional |
| Manifestation | Renderer/UI/simulation packages | Optional semantic resolution |

## Semantic resolution flow

```text
                 EXPERIENCE / APPLICATION
                           |
                           v
                    semantic request
                           |
                           v
                    OntologyAddress
                           |
                 +---------+---------+
                 |                   |
                 v                   v
          ontology relationship   repository
                 |                   |
                 v                   v
          meaning / mapping       artifact
                 |                   |
                 +---------+---------+
                           |
                           v
                      manifestation
                 (behavior / UI / render)
```

The important boundary is that the semantic address does not itself contain the implementation. It identifies meaning. A relationship can connect that meaning to an implementation owned elsewhere.



## Knowledge operations are not workflow execution

Ontology supplies semantic data and validation operations. It does not own a workflow loop, scheduler, task lifecycle, GUI, or execution policy.

The current public surface supports:
- validating and constructing ontology definitions and qualified addresses;
- registering and retrieving definitions through OntologySet;
- representing and validating semantic relationships;
- representing scalar/N-dimensional coordinates and optional dense-space conversions;
- validating half-open ranges and placement-neutral coverage metadata.

It does **not** currently expose a general query language, graph traversal engine, inference engine, distributed lookup service, or asynchronous query scheduler. Do not describe these as implemented merely because the data model can support future consumers.

The ownership split is:

| Responsibility | Owner | Ontology's role |
|---|---|---|
| Semantic definitions, addresses, indexes, relationships, ranges | Ontology | Owns the contracts and local validation |
| Workflow selection, ordering, retries, scheduling, execution lifecycle | FSM_Execution | May invoke an Ontology operation as one workflow capability |
| MicroBundle composition and runtime assembly | FSM_COS | Composes declared capabilities without interpreting domain meaning |
| Presentation and user interaction | Optional GUI selected by the host | May present ontology-derived information; is not an Ontology dependency |
| Artifact persistence and retrieval | MicroBundleRepository or another storage owner | May persist semantic metadata without moving storage behavior into Ontology |

A workflow can call an Ontology-owned operation and use its result in a later step. The workflow engine owns when and why that operation is invoked, what happens on failure, and which subsequent capability runs. Ontology owns only the meaning and behavior of its own contract operation. This is capability invocation, not a second execution runtime inside Ontology.

The project file currently references MicroBundleDomain for bundle integration. It contains no GUI, FSM_Execution, or FSM_COS package/project dependency. Preserve that dependency boundary unless a concrete semantic contract requires a narrowly scoped change.

## Domain contract inventory

The current package deliberately separates five kinds of semantic information:

| Contract | Answers | Does not answer |
|---|---|---|
| `OntologyDefinition` | Which ontology is this, which version is it, and what ordered layers does it declare? | What individual concepts mean |
| `OntologyAddress` | Where is a semantic concept located within a qualified ontology? | How that concept is implemented |
| `OntologyIndex` | What compact scalar or N-dimensional coordinate identifies the location? | How coordinates are stored physically |
| `OntologyIndexSpace` | What bounded coordinate space exists, and how can coordinates be mapped to dense offsets? | Whether dense storage should be used |
| `OntologyRange` | Which half-open semantic interval belongs to one ontology version? | Physical placement or storage |
| `OntologyCoverage` | Which opaque server identifier is associated with a semantic interval? | Server discovery, routing, availability, or persistence |
| `OntologyRelationship` | How are two semantic addresses related? | What the predicate means to a particular domain |

`OntologySet` provides the runtime registry for independently versioned definitions without declaring a universal ontology. `WorkshopOntologySchema` provides the canonical nine-layer names/order and validates nine-coordinate address creation without restricting generic ontology definitions.

### Definition invariants

An ontology definition requires:

- a non-zero ontology identifier;
- a non-empty name;
- a non-empty version;
- unique, non-zero layer identifiers;
- non-empty layer names;
- non-negative, unique layer ordering.

These invariants make the declared layer sequence unambiguous without imposing a fixed number of layers.

### Index invariants

An ontology index has at least one coordinate. A scalar index is the one-dimensional form; multiple coordinates represent an ordered N-dimensional location. A one-coordinate index has the same identity as its scalar equivalent.

An index space requires at least one positive dimension. Each dimension can contain at most the complete `ulong` coordinate domain, while total cardinality is represented with `BigInteger`.

### Boundary rule

The package defines **semantic structure**, not domain knowledge.

It can express an ontology containing chemistry, AEC, animal life, physics, rendering, or any other subject, but it does not embed those subjects into the contract. The eventual Workshop master ontology is therefore content built with these mechanisms, not additional authority inside this package.


## Ontological behavior associations

Classification alone answers where an asset belongs; it does not provide the behavior stack that should accompany it. The intended model adds ontology-owned associations from concepts (and, where appropriate, classifications) to behavior, control, and capability contracts. The implementation may live in provider MicroBundles, while ontology records the semantic association and its scope.

For example, a Dog concept can associate with locomotion, run behavior, and dog control contracts. A SuperDog specialization can inherit those associations and add flight and crime-fighting capabilities. The asset author declares the specialization and its genuinely novel capabilities; the platform resolves inherited associations instead of requiring repeated manual composition.

This must not be confused with putting executable code into a classification coordinate. The ontology describes *which* behaviors apply and *why*; provider MicroBundles describe *how* those behaviors are implemented; FSM_COS resolves compatible providers and composes them for an experience. The ontology package does not currently implement this resolution pipeline. The association, inheritance, precedence, conflict, override, and versioning contracts remain design work.

## Canonical nine-layer classification and architectural graphs

The Workshop's canonical ontology uses the ordered classification positions Paradigm, Domain, Kingdom, Phylum, Class, Order, Family, Genus, and Species. This is a canonical semantic profile, not a fixed-rank requirement on the generic library. Independent ontologies may declare other ranks and structures.

Keep the following graphs and structures separate:

| Structure | What its edges or positions mean | Owner |
|---|---|---|
| Nine-position classification | Broad-to-specific semantic placement | Workshop canonical ontology |
| Ontology relationship graph | Explicit semantic relationships, including cross-ontology links | Ontology/domain definitions |
| MicroBundle dependency graph | Required capabilities and nested content | MicroBundleDomain |
| Artifact lookup | Portable reference to stored content/blob | MicroBundleRepository |
| Runtime composition graph | Resolved local components and execution relationships | FSM_COS |

A tenth classification position is not a substitute for a relationship, a second membership, contextual data, a provider mapping, or a nested MicroBundle. The canonical model's anti-tenth-layer test is documented in [The Nine-Layer Ontology](NINE-LAYER-MODEL.md).

A nine-position semantic coordinate is also not a storage address or runtime handle. Portable identity and exact version are resolved through repository and runtime boundaries; blob indexes and runtime-local numeric handles remain implementation-specific.

## Portable ontology identity versus runtime integer indexing

The Workshop deliberately uses two address systems for two different jobs. The ontology's potentially enormous semantic address space is an **external reference space**. It gives concepts and MicroBundles portable identities that can be exchanged, stored, searched, and understood across Experiences and systems. It is not the representation that the runtime should repeatedly resolve on every hot-path operation.

An Experience declares the MicroBundles that participate in it and their load sequence. Once the Experience's bundle set and dependencies have been resolved, the ordered set is assigned a compact local integer index mapping. FSM_COS uses those integers for runtime access and composition. The mapping retains the portable string ontology identity as the external key:

```text
portable ontology identity (string / qualified address)
                  |
                  v
       MicroBundleRepository resolution
                  |
                  v
       Experience's ordered bundle manifest
                  |
                  v
       Experience-local integer index map
                  |
                  v
       FSM_COS: integer-first runtime
                  |
                  v
       external boundary: map index back
                  |
                  v
       portable ontology identity
```

The load sequence is part of the Experience's mapping contract. Given the same resolved bundle set, exact versions, and ordering rules, preparation should produce a deterministic mapping. A different Experience—or a changed load sequence—may assign different local integers to the same portable bundle. That is expected: local indexes are scoped to the prepared Experience and must never be mistaken for global semantic identities.

Consequently:

- Persisted or exchanged references use portable ontology identity and exact version, not a bare Experience-local integer.
- Runtime hot paths use the compact integer mapping, not repeated string parsing or ontology traversal.
- Diagnostics and external integrations translate local indexes back through the Experience mapping.
- The repository resolves portable identities to stored artifacts; its blob/storage indexes are not ontology identities and are not runtime indexes.
- FSM_COS owns the local runtime mapping and integer-first composition; Ontology owns portable semantic definitions and addresses; Experience owns the selected bundle set and load sequence.

This section records the intended ecosystem architecture. The Ontology package does not itself load Experiences, allocate FSM_COS runtime indexes, or execute MicroBundles.

## The upper ontology remains an evolving model

The Workshop's broadest classification position is intended to distinguish frames such as **Reality**, **Fiction**, **Abstraction**, and **Utility**. Each can branch into its own domains and nested semantic trees. The animal kingdom ontology and an ontology of things are examples of sibling branches in that larger space, not evidence that every subject must descend from biological taxonomy.

The precise semantic position immediately above those branches is not yet settled. The nine-position coordinate remains the current canonical shape, but the upper-level interpretation should remain explicit about what is established and what is still under exploration. Do not invent a definition for an unresolved position merely to fill a diagram.

## N-dimensional indexing

OntologyIndex is the compact semantic coordinate.

The default form is scalar:

```text
OntologyIndex(42)
```

A domain can choose N-dimensional coordinates:

```text
OntologyIndex.Create(2, 14, 7, 3)
```

For a dense space, OntologyIndexSpace additionally describes dimension extents and maps coordinates to linear offsets. Its Cardinality is calculated with arbitrary precision so the size of the theoretical space is not limited by the size of the integer used for an individual coordinate.

This gives three separate concepts:

1. **coordinate** — where something is in semantic N-space;
2. **cardinality** — how many coordinates the declared space can contain;
3. **linear offset** — an optional storage-oriented representation for dense arrays.

The package intentionally does not require that a semantic address ever be flattened or stored densely.

## Example: fish locomotion

A hypothetical ontology can declare:

```text
ontology://100/1.0.0/life/animal/fish
    |
    +-- locomotion
          |
          +-- swim
```

A separate rendering or animation ontology can declare:

```text
ontology://200/1.0.0/animation/swim
```

A relationship can connect them:

```text
fish/swim  --manifests-as-->  animation/swim
```

The fish ontology does not need to know how the animation is rendered, and the renderer does not need to encode the animal taxonomy.

## Coexisting structures

The architecture supports intentionally different structures:

```text
Application A                  Application B
     |                              |
  5 layers                      9 layers
     |                              |
  Ontology A                    Ontology B
     |                              |
     +---------------+--------------+
                     |
                 same runtime
```

An application may also use no ontology:

```text
MicroBundleDomain
       |
       +---- application content
```

This is a feature, not an incomplete state.

## Evolution

The preferred progression is:

1. stable identity and versioning;
2. semantic addressing;
3. scalar and N-dimensional indexing;
4. explicit address-space mathematics;
5. explicit relationships and cross-ontology mappings;
6. composition and mapping contracts proven by real applications;
7. optional tooling and visualization;
8. richer reasoning only when an actual consumer requires it.

That keeps the core small while leaving room for the larger Singularity Workshop semantic model.

## Related reading

- [Ontological behavior associations](BEHAVIOR-ASSOCIATIONS.md) — the intended classify-once/inherit-capabilities model and its implementation boundary.

- [Nine-layer model](NINE-LAYER-MODEL.md)
- [Ontology landscape](ONTOLOGY-LANDSCAPE.md)
- [Theory](THEORY.md)
- [Address-Space Mathematics](MATHEMATICS.md)
- [Ontology boundary visual](ontology-boundary.svg)
- [Semantic resolution visual](semantic-resolution.svg)
- [Address-space visual](address-space.svg)
