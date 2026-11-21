# 02 — Classification Without a Universal Taxonomy

## 01 — What: classification is one semantic tool

Classification places a concept along ordered semantic axes. It helps answer what broad family something belongs to and how specifically it has been classified.

Classification is not the whole ontology. Relationships, context, behavior, runtime composition, and storage placement answer different questions and must not be forced into extra classification positions.

## 02 — Why: stable Workshop coordinates, flexible domain models

The Workshop has a canonical nine-position profile:

1. Paradigm
2. Domain
3. Kingdom
4. Phylum
5. Class
6. Order
7. Family
8. Genus
9. Species

The names borrow a familiar broad-to-specific progression. This is an engineering classification profile, not a claim that every subject is biological taxonomy.

The generic library remains open to independent models. One ontology may declare one layer, another five, another nine, and another none. The canonical profile is Workshop content, not a rank restriction on every consumer.

## 03 — How: distinguish rank from graph structure

Rank is the number of ordered coordinate positions in a classification tuple. A nine-position tuple has rank nine.

A relationship graph connects semantic addresses using explicit predicates. Its number of edges does not change classification rank. A MicroBundle dependency graph expresses capability dependencies and composition. A runtime composition graph expresses resolved runtime pieces. Neither is a taxonomy.

A tenth classification position is justified only by a real independent axis that cannot be represented by an existing position, multiple membership, another ontology, a relationship, context, provider mapping, or composition. This is an engineering sufficiency test, not a mathematical proof that nine positions describe all knowledge.

## 04 — See it: canonical and independent models

Implemented canonical profile:

~~~text
Paradigm → Domain → Kingdom → Phylum → Class
         → Order → Family → Genus → Species
~~~

Conceptual independent ontology examples:

~~~text
AEC ontology:       [discipline, asset, system, component, property]
Animal ontology:    [concept]
Physics ontology:   [field, model, quantity]
~~~

The independent examples illustrate that rank belongs to the ontology using it. They are not built-in schemas.

## 05 — Implementation and evidence

- [WorkshopOntologySchema.cs](../../WorkshopOntologySchema.cs) declares the nine names and creates layer definitions from caller-supplied stable, non-zero IDs.
- Its CreateAddress method requires a canonical definition and an index of rank nine.
- [OntologyDefinition.cs](../../OntologyDefinition.cs) validates generic layers without imposing nine.
- [OntologyTests.cs](../../tests/TheSingularityWorkshop.Ontology.Tests/OntologyTests.cs) covers canonical order, unique IDs, invalid declarations, and a five-layer independent ontology.

The canonical helper creates and recognizes the Workshop profile while generic definitions remain arbitrary-rank. The library does not assign globally meaningful layer IDs, infer a layer from a concept, or prove that a classification is true in the real world.

## Limits and open questions

The canonical names and semantics are a Workshop convention. Their suitability for a particular domain must be evaluated by the ontology author. A concept may need multiple memberships or cross-ontology relationships rather than one path through one taxonomy.

## Practice

1. Is “has a swimming behavior” a classification position or a relationship? Explain.
2. Why should a layer ID not be silently derived from its array index?
3. Propose a counterexample that would justify a tenth position, then test whether a relationship or separate ontology handles it better.

## Further reading

- [Full canonical nine-layer model](../NINE-LAYER-MODEL.md)
- [Canonical schema implementation](../../WorkshopOntologySchema.cs)
- [Architecture graph distinctions](../ARCHITECTURE.md)
