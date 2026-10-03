# The Singularity Workshop — Ontology

![Ontology architecture](docs/ontology-boundary.svg)

**Ontology is a MicroBundle, not a constraint on MicroBundles.**

This package provides independent contracts for defining, versioning, addressing, and relating ontological information. It deliberately does **not** require every MicroBundle to use The Singularity Workshop's ontology.

## Why this exists

The Workshop needs semantic structure that can scale beyond renderer-specific conditionals without turning that structure into a mandatory global schema.

A useful mental model is:

````text
meaning
   |
   v
semantic address
   |
   v
relationship
   |
   v
manifestation
````

For example, an application could express a fish's swimming concept as an ontology address and relate it to a swimming behavior or animation. The renderer can consume the relationship without becoming an animal-taxonomy engine.

Read the deeper design rationale in [docs/THEORY.md](docs/THEORY.md) and the implementation boundary in [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md).

## Design boundary

````text
MicroBundleDomain
    defines the generic MicroBundle mechanism
             |
             +-------------------+
             |                   |
          Ontology          application content
             |
      defines its own
      semantic structure
````

A developer using `TheSingularityWorkshop.MicroBundleDomain` may build a five-layer model, a nine-layer model, no ontology at all, or an entirely different structure. Ontology exists to make semantic structure available when a developer chooses it.

## Coexisting ontologies

An application may load multiple independently defined ontologies at the same time. Ontologies are identified by an explicit ID and version; no ontology is silently promoted to a universal schema.

The Singularity Workshop can publish a master ontology as **content** that contains domains such as the animal kingdom, physics, materials, AEC, or other knowledge families. That master ontology remains an ontology definition, not a requirement imposed on unrelated MicroBundles.

## Semantic addresses

Ontology addresses allow behavior and data to be located by meaning rather than by renderer-specific conditionals:

``ontology://<ontology-id>/<version>/<semantic-path>``

For example:

``ontology://42/1.0.0/life/animal/fish/locomotion/swim``

An ontology relationship can connect that semantic concept to a manifestation such as a behavior, animation, simulation rule, or UI affordance.

## Dependency direction

- `TheSingularityWorkshop.Ontology` may depend on `TheSingularityWorkshop.MicroBundleDomain`.
- `MicroBundleDomain` must remain independent of Ontology.
- `MicroBundleRepository` stores/retrieves content without requiring ontology knowledge.
- `FSM_COS` may compose ontology MicroBundles but does not hard-code ontology-specific meaning.

This keeps the mechanism reusable while allowing richer semantic systems to grow above it.

## Documentation

- **[Theory](docs/THEORY.md)** — the reasoning and design principles behind optional, addressable semantics.
- **[Architecture](docs/ARCHITECTURE.md)** — package boundaries, semantic resolution, coexistence, and evolution.
- **[Architecture visual](docs/ontology-boundary.svg)** — a visual map of the boundary and semantic flow.

## Status

This package is an early public contract. The initial release establishes the architectural boundary and intentionally keeps the model small.

The next work should add concrete ontology composition and mapping contracts only where demonstrated by real use cases.