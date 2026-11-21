# 04 — Relationships, Coexistence, and Materialization

## 01 — What: meaning connects without collapsing

A semantic model rarely lives as one tree. A concept can relate to concepts in another ontology, and one concept can have several manifestations. Ontology relationships make those connections explicit while allowing the definitions on either side to remain independently owned.

## 02 — Why: avoid a universal schema and a storage-shaped ontology

An animal ontology and an animation ontology describe different aspects of one application. Requiring the animation system to adopt the animal taxonomy couples unrelated domains. Embedding a physical storage key into semantic identity creates another coupling: moving an artifact would appear to change what the concept means.

The useful separation is:
- ontology definition owns semantic identity and version;
- address owns a qualified semantic location;
- relationship owns a declared semantic connection;
- repository owns artifact storage and retrieval;
- runtime composition owns execution of selected capabilities;
- presentation systems own their manifestations.

These boundaries are complementary, not competing graphs.

## 03 — How: relationships and coexisting definitions

A relationship has a source address, a non-empty predicate, and a target address. Source and target may belong to different ontology IDs and versions; both addresses must independently validate.

OntologySet holds multiple definitions keyed by (Id, Version). Independent versions can coexist rather than silently treating a newer version as a replacement for an older one.

Ontology does not define a general-purpose inference engine. A predicate such as manifests-as is a declared label; the consumer that interprets it owns the behavior and validation appropriate to that use.

## 04 — See it: cross-ontology relation

~~~text
Animals@1.0.0                         Animation@2.1.0
ontology://100/.../fish/swim  --manifests-as-->  ontology://200/.../swim
~~~

The relationship is semantic metadata. It does not prove that an animation file exists, that a renderer supports it, or that the animation is available.

A placement record is another distinct concern. [Ontology ranges and coverage](../ONTOLOGY-RANGES.md) describe a half-open semantic interval and an opaque server identifier, but do not perform discovery or storage.

## 05 — Implementation and evidence

- [OntologyRelationship.cs](../../OntologyRelationship.cs) validates both endpoint addresses and the predicate.
- [OntologySet.cs](../../OntologySet.cs) registers definitions by identity and version.
- [OntologyTests.cs](../../tests/TheSingularityWorkshop.Ontology.Tests/OntologyTests.cs) covers cross-ontology relationships and coexistence.
- [OntologyCoverage.cs](../../OntologyCoverage.cs) and [OntologyRange.cs](../../OntologyRange.cs) define placement-neutral coverage metadata and validated half-open ranges.

The contracts are deliberately small. Tests demonstrate validation and registration behavior; they do not test a distributed server, repository, renderer, or runtime scheduler.

## Limits and open questions

The package does not decide predicate vocabularies, relationship cardinality rules, transitive closure, conflict resolution, trust, authorization, or whether a manifestation can be executed. Those require domain-specific contracts and consumers. Do not infer execution order from a relationship graph.

## Practice

1. Why should an ontology relationship graph remain separate from a MicroBundle dependency graph?
2. What does a relationship prove, and what does it not prove?
3. If a file moves to another server, which semantic identifiers should remain stable?

## Further reading

- [Architecture ownership map](../ARCHITECTURE.md)
- [Relationship contract](../../OntologyRelationship.cs)
- [Range and coverage contract](../ONTOLOGY-RANGES.md)
- [Repository and runtime boundaries](../ARCHITECTURE.md)
