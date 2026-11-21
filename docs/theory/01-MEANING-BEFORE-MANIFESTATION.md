# 01 — Meaning Before Manifestation

## 01 — What: name the problem

Imagine an application that displays a fish, animates it swimming, simulates its movement, and explains its biology. A fragile design makes every consumer recognize the fish independently. The renderer, simulation, and UI repeat domain conditionals, and those copies can drift.

Ontology provides optional contracts for naming a concept and relating it to other concepts or manifestations. It gives meaning a stable, version-qualified address without requiring a renderer or simulation to own the taxonomy.

## 02 — Why: separate meaning from implementation

A library catalogue identifier tells us which work is being discussed; it is not the book, shelf, building, or delivery service. Semantic relationships are more flexible than a catalogue, but the analogy captures the boundary: identity is not implementation or physical placement.

~~~text
semantic meaning
      |
      v
qualified address
      |
      +---- relationship ----> manifestation
                                  +--> animation
                                  +--> behavior
                                  +--> simulation rule
                                  +--> UI affordance
~~~

The address identifies meaning. A relationship expresses a declared connection. A consumer decides what that connection means operationally.

This boundary is useful when several consumers need to agree on identity while retaining independent implementations. It is a poor fit when an application has no reusable semantic structure to share.

## 03 — How: vocabulary and invariants

- Ontology: an independently identified and versioned semantic model.
- Ontology definition: identity, name, version, and optional ordered layer declarations.
- Semantic address: a path qualified by ontology ID and version.
- Index: an optional compact scalar or N-dimensional coordinate carried by an address.
- Relationship: a declared predicate connecting two semantic addresses.

The key invariant is optionality: the generic MicroBundle mechanism does not require Ontology, and Ontology does not become a global taxonomy authority.

## 04 — See it: a small example

Conceptual example — not a runnable provider integration:

~~~text
ontology://100/1.0.0/life/animal/fish/locomotion/swim
                         |
                         | manifests-as
                         v
ontology://200/1.0.0/animation/swim
~~~

The first address belongs to a life-science ontology; the second belongs to an animation ontology. A renderer may consume the relationship, but neither address contains animation code.

## 05 — Implementation and evidence

- [OntologyDefinition.cs](../../OntologyDefinition.cs) validates independently versioned definitions and layer declarations.
- [OntologyAddress.cs](../../OntologyAddress.cs) creates normalized addresses qualified by ID and version.
- [OntologyRelationship.cs](../../OntologyRelationship.cs) connects source and target addresses with a required predicate.
- [OntologySet.cs](../../OntologySet.cs) allows definitions to coexist by identity and version.

[OntologyTests.cs](../../tests/TheSingularityWorkshop.Ontology.Tests/OntologyTests.cs) covers coexistence, qualified addresses, and cross-ontology relationships.

These tests establish the contract behavior. They do not establish a universal reasoning engine, automatic semantic discovery, or automatic manifestation selection.

## Limits and open questions

The contracts do not define a universal ontology language, inference rules, authorization policy, or provider execution protocol. A consumer must define how it interprets a predicate such as manifests-as. A relationship alone does not prove that a target artifact exists or can be executed.

## Practice

1. Find a renderer conditional that repeats domain meaning. What stable concept would it refer to?
2. Could two independent ontologies describe the same subject for different purposes? Which relationship would connect them?
3. Give an example where introducing Ontology would add unnecessary complexity.

## Further reading

- [Architecture boundaries](../ARCHITECTURE.md)
- [Relationship contract](../../OntologyRelationship.cs)
- [Theory overview](../THEORY.md)
- [MicroBundleDomain](https://github.com/TrentBest/TheSingularityWorkshop.MicroBundleDomain)
