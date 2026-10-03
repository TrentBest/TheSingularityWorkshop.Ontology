# The Singularity Workshop — Ontology

**Ontology is a MicroBundle, not a constraint on MicroBundles.**

This package provides independent contracts for defining, versioning, addressing, and relating ontological information. It deliberately does **not** require every MicroBundle to use The Singularity Workshop's ontology.

## Design boundary

```
MicroBundleDomain
    defines the generic MicroBundle mechanism
             |
             +-------------------+
             |                   |
          Ontology          application content
             |
      defines its own
      semantic structure
```

A developer using `TheSingularityWorkshop.MicroBundleDomain` may build a five-layer model, a nine-layer model, no ontology at all, or an entirely different structure. Ontology exists to make semantic structure available when a developer chooses it.

## Coexisting ontologies

An application may load multiple independently defined ontologies at the same time. Ontologies are identified by an explicit ID and version; no ontology is silently promoted to a universal schema.

The Singularity Workshop can publish a master ontology as **content** that contains domains such as the animal kingdom, physics, materials, AEC, or other knowledge families. That master ontology remains an ontology definition, not a requirement imposed on unrelated MicroBundles.

## Semantic addresses

Ontology addresses allow behavior and data to be located by meaning rather than by renderer-specific conditionals:

`ontology://<ontology-id>/<version>/<semantic-path>`

For example, an ontology could define a fish locomotion concept and a manifestation could associate that address with a swim behavior or animation. The renderer does not need to become an animal taxonomy engine.

## Dependency direction

- `TheSingularityWorkshop.Ontology` may depend on `TheSingularityWorkshop.MicroBundleDomain`.
- `MicroBundleDomain` must remain independent of Ontology.
- `MicroBundleRepository` stores/retrieves content without requiring ontology knowledge.
- `FSM_COS` may compose ontology MicroBundles but does not hard-code ontology-specific meaning.

This keeps the mechanism reusable while allowing richer semantic systems to grow above it.

## Status

This package is an early public contract. The initial release establishes the architectural boundary and intentionally keeps the model small.

The next work should add concrete ontology composition and mapping contracts only where demonstrated by real use cases.
