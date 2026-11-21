# Ontological Behavior Associations

## Purpose

An ontology should not stop at classifying an entity. It should also tell a consumer which behavior contracts, controls, and capabilities are associated with that entity by virtue of what it is.

The authoring goal is **classify once; inherit the applicable behavior foundation**. Creating another Dog asset should not require the author to rebuild the ordinary dog locomotion, running, and control setup. A specialization such as SuperDog should receive the Dog foundation and add only its genuine extensions, such as flight or crime-fighting.

This document defines the intended semantic model. It does not claim that the current Ontology package already performs behavior inheritance, provider resolution, or runtime composition.

## 1. Separate the semantic association from its implementation

Four responsibilities must remain distinct:

| Concern | Responsibility |
|---|---|
| Ontology concept/classification | Identifies what the entity is and the semantic generalizations it participates in |
| Behavior association | Declares which behavior, control, or capability contract applies, and under what conditions |
| Provider MicroBundle | Implements a behavior/control contract and declares its own dependencies |
| FSM_COS | Resolves compatible providers and composes the runtime capability set for an experience |

An ontology association is not executable code. It is a semantic declaration that a capability is applicable. The provider is the implementation. The runtime is the composed result.

## 2. Inheritance and specialization

The intended resolution model is:

```text
Dog asset
   |
   +--> classified as Dog
   |       |
   |       +--> locomotion behavior
   |       +--> run behavior
   |       +--> dog control contract
   |
   +--> specialized as SuperDog
           |
           +--> flight capability
           +--> crime-fighting capability
   |
   v
resolved applicable associations
   |
   v
provider MicroBundles
   |
   v
FSM_COS composition for an Experience
```

The inherited set should be derived from explicit semantic generalization/specialization relationships, not from comparing raw integer coordinates or assuming that every adjacent layer automatically implies inheritance. Classification answers where a concept is placed; a typed relationship or declared rule must establish which concepts inherit from which others.

An asset instance should normally reference its concept and explicit specializations. It should not copy every inherited behavior association into its own data. This avoids duplicated declarations and lets a corrected or extended ontology association benefit all applicable assets.

## 3. Association kinds

The ontology will need a controlled vocabulary for association semantics. Candidate kinds include:

- **has-behavior** — associates a concept with a behavior contract;
- **has-control** — associates a concept with an interaction/control contract;
- **has-capability** — associates a concept with a capability contract;
- **requires-capability** — declares a dependency needed for a behavior to function;
- **specializes** — declares a more specific concept that inherits applicable associations;
- **overrides** — replaces a specific inherited association under explicit scope;
- **excludes** — declares an incompatibility or deliberate removal where policy permits it;
- **manifests-as** — maps a semantic behavior or capability to a concrete provider or manifestation.

These are proposed semantic terms, not a claim that this exact predicate vocabulary is already implemented or frozen. Their domains, ranges, cardinality, and versioning rules must be formalized before becoming public contracts.

## 4. Resolution requirements

A future resolver must define and test at least the following:

1. **Inheritance source:** which explicit relationships contribute inherited associations.
2. **Scope:** whether an association applies to a concept, a classification, an instance, an environment, or an experience.
3. **Precedence:** how direct associations, inherited associations, and specializations interact.
4. **Override and disable:** how a consumer deliberately replaces or suppresses an inherited behavior without silently corrupting the ontology.
5. **Conflict:** what happens when two applicable providers claim incompatible implementations or controls.
6. **Dependencies:** how required and optional capabilities are distinguished and closed over.
7. **Version compatibility:** how exact ontology, behavior-contract, provider, and dependency versions are checked.
8. **Determinism:** the same ontology versions and context must produce the same resolved association set.
9. **Failure behavior:** unresolved, incompatible, or ambiguous required behavior must fail closed rather than silently selecting an arbitrary provider.
10. **Explainability:** a resolver should be able to explain why each behavior was included, inherited, overridden, or rejected.

Do not assume that the most specific classification always wins. Precedence is a semantic policy and must be explicit.

## 5. Resolution is not execution

Ontology can answer: *which behavior contracts apply to this concept?*

A provider MicroBundle answers: *how is a particular behavior contract implemented?*

FSM_COS answers: *which compatible providers are assembled into this runtime?*

FSM_Execution or another execution owner may then govern workflow/task progression where needed. Ontology should not become a scheduler, a game loop, an input system, or a second runtime.

The runtime composition may include many capabilities that are not themselves ontology layers. The behavior graph and MicroBundle dependency graph are also distinct from the nine-position classification coordinate.

## 6. Example: Dog and SuperDog

A normal Dog concept might declare associations to:

- locomotion;
- run;
- dog movement controls;
- ordinary sensory or interaction capabilities as defined by the relevant domain ontology.

A SuperDog concept specializes Dog and adds:

- flight;
- cape-specific behavior, if the cape is functional rather than decorative;
- crime-fighting capabilities.

A new SuperDog asset then identifies its concept and its asset-specific data. It does not manually recompose run, locomotion, and dog controls. The resolver obtains those from the ontology's applicable associations, adds SuperDog's extensions, checks provider compatibility, and presents the resulting requirements to FSM_COS for composition.

This example illustrates the intended model. It does not imply that every Dog must have identical controls in every experience: context and experience-specific policy may select or override controls without changing the underlying concept.

## 7. Implementation boundary and next proof

The current package provides ontology definitions, addresses, indexes, relationships, and local validation. It does not yet provide the complete behavior-association schema or an automatic inheritance/provider-resolution engine.

The next implementation should proceed from explicit contracts and tests, not from a premature runtime implementation:

1. define stable identities for behavior and control contracts;
2. define typed association records and their ontology/version scope;
3. define explicit specialization/inheritance semantics;
4. specify precedence, override, conflict, and required/optional rules;
5. define provider references with exact versions;
6. implement a deterministic, explainable resolver;
7. validate the resolved set before FSM_COS composes it.

Keep generic ontology infrastructure generic. The Workshop's canonical nine-layer profile can supply classification context, while independent ontologies remain free to define other structures and behavior semantics.
