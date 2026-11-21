# Ontology Theory: The Broader Landscape

This document places the Workshop's ontology model beside established work in formal and foundational ontology. The goal is not to claim that the Workshop replaces these systems. It is to distinguish the problems they solve, identify ideas worth borrowing, and make precise what this repository currently implements.

## 1. Ontology is more than a taxonomy

A taxonomy organizes concepts by a classification relation such as *is-a* or *subclass-of*. A formal ontology generally goes further: it makes a conceptualization explicit, defines entities and relations, and may add axioms or constraints that restrict valid interpretations.

This distinction matters to the Workshop. The nine-position sequence is a **classification coordinate**. It is not, by itself, the complete ontology. The wider semantic model also needs relationships, definitions, identity, context, and (where justified) formal constraints.

A useful separation is:

| Concern | Question |
|---|---|
| Vocabulary | What names or tokens are used? |
| Taxonomy / classification | How is a concept classified relative to another? |
| Ontology | What entities, properties, relations, and constraints are asserted to exist in the modeled domain? |
| Knowledge base | Which particular assertions or facts are currently recorded? |
| Runtime composition | Which executable capabilities are assembled to act on the modeled information? |

The Workshop's nine-layer model belongs primarily to classification. The relationship graph carries connections that do not fit a single parent/child hierarchy. MicroBundle composition is a separate operational graph.

## 2. Established foundational ontologies

Foundational or top-level ontologies provide high-level categories and principles intended to support more specialized domain ontologies. They differ in philosophical commitments, category systems, formalization choices, and intended use. Their structures should be compared by what they model—not by counting the levels in a taxonomy.

### Basic Formal Ontology (BFO)

[BFO](https://github.com/BFO-ontology/BFO-2020) is standardized in [ISO/IEC 21838-2:2021](https://www.iso.org/standard/74572.html). It provides general categories and relations intended to support information interchange among heterogeneous systems and to serve as a top-level hub for suites of domain ontologies.

**Lesson for the Workshop:** explicit category definitions, relations, axiomatization, and disciplined modularity matter more than a large vocabulary. BFO is not a nine-rank classification scheme and does not establish a universal layer count.

### DOLCE

[DOLCE](https://www.loa.istc.cnr.it/dolce/overview.html), the Descriptive Ontology for Linguistic and Cognitive Engineering, is a foundational ontology developed by the Laboratory for Applied Ontology. It is formally specified in first-order logic and was designed as a descriptive account informed by linguistic and cognitive engineering.

**Lesson for the Workshop:** document the conceptual distinctions and the intended interpretation of terms. Similar-looking labels must not be assumed to mean the same thing across different models.

### General Formal Ontology (GFO)

[GFO](https://www.onto-med.de/ontologies/gfo/) is a top-level ontology for conceptual modeling. Its work includes categories for objects, processes, time, space, properties, relations, roles, functions, facts, and situations. Its modular artifacts and ongoing development illustrate that a foundational ontology is not merely a single tree.

**Lesson for the Workshop:** model objects, processes, time, space, roles, and functions as distinct semantic concerns when the domain requires them. Do not force all of them into classification depth.

### Unified Foundational Ontology (UFO)

[UFO](https://nemo.inf.ufes.br/projetos/ufo/) organizes foundational theories into complementary strata, commonly described as UFO-A (endurants/structural aspects), UFO-B (perdurants/events and processes), and UFO-C (social and intentional entities). It provides formal distinctions used in conceptual modeling, including types, taxonomic structures, relations, roles, events, and commitments.

**Lesson for the Workshop:** orthogonal theories can be composed without turning each theory into another rank in one universal hierarchy. The Workshop's relationships, contexts, manifestations, and MicroBundle dependencies should remain distinct from the nine classification positions.

### Suggested Upper Merged Ontology (SUMO)

[SUMO](https://ontologyportal.org/) is a broad formal upper ontology accompanied by domain ontologies. Its terms are formally defined and it has mappings to WordNet, with tools and axiomatizations supporting formal reasoning.

**Lesson for the Workshop:** a broad ontology can be useful as shared content, but vocabulary mappings, formal definitions, and inference semantics must be explicit. A list of labels or integer IDs alone does not provide those guarantees.

## 3. What the Workshop model is—and is not

The Workshop's canonical nine positions are:

**Paradigm → Domain → Kingdom → Phylum → Class → Order → Family → Genus → Species**

They define a stable, broad-to-specific classification coordinate for the Workshop's canonical ontology. They do not claim to reproduce BFO, DOLCE, GFO, UFO, or SUMO; those systems organize foundational categories and formal relations in different ways.

This repository currently provides reusable ontology infrastructure:

- independently identified and versioned ontology definitions;
- ordered layer definitions;
- semantic addresses;
- scalar and N-dimensional coordinates;
- bounded address spaces and reversible dense-array indexing;
- cross-ontology relationships;
- half-open semantic ranges and server coverage metadata;
- coexistence of independently versioned ontologies;
- a canonical Workshop nine-layer schema helper.

It does **not yet** implement a complete axiomatized foundational ontology, a universal ontology language, a general-purpose theorem prover, or a complete master ontology of all Workshop domains. Those should not be implied by the package name or by the presence of nine layer names.

The Workshop model is therefore best understood as a **portable semantic-address and classification framework**, with room for domain ontologies and stronger formal semantics to be layered on later.

## 4. Design consequences

The comparison above leads to several practical rules:

1. **Do not confuse rank with reality.** The nine positions organize one chosen classification coordinate; they are not a complete inventory of all entity types, processes, properties, or relations.
2. **Keep relations explicit.** A relation such as part-of, participates-in, causes, manifests-as, or depends-on has meaning beyond its source and target coordinates. Its predicate semantics need an owner and a definition.
3. **Keep ontology identity versioned.** A coordinate only has meaning under the ontology definition and version that declare its positions and vocabulary.
4. **Keep classification separate from composition.** A nested MicroBundle is a capability/dependency relationship, not a child taxonomy rank.
5. **Keep portable identity separate from local optimization.** Repository blob indexes and FSM_COS runtime handles are local resolution artifacts, not globally portable semantic identities.
6. **Make formal commitments explicit.** If the Workshop later adopts disjointness, completeness, identity, part-whole, temporal, causal, or modal axioms, those commitments must be versioned and tested rather than hidden in naming conventions.
7. **Permit multiple ontologies.** Different domains may reuse, map, or deliberately disagree with one another. Cross-ontology mappings should be explicit rather than silently merging incompatible concepts.

## 5. The nine-versus-ten question in context

Foundational ontologies demonstrate that there is no single universally accepted category system from which a nine-position classification can be deduced. They do not refute the Workshop's choice either.

The mathematical product space tells us how many coordinates a chosen number of positions can represent. It cannot decide whether a proposed distinction deserves its own semantic position. That is a modeling question answered by definitions, use cases, and counterexamples.

The Workshop's anti-tenth-layer test is therefore:

- identify the proposed new distinction precisely;
- show why it cannot be represented by an existing position or value;
- rule out multiple membership, another ontology, a relationship, context, manifestation, or MicroBundle capability;
- demonstrate a real use case where omitting it causes ambiguity or loss;
- define its semantics and compatibility/versioning consequences.

Until a concrete counterexample passes that test, nine remains the canonical Workshop classification arity. This is a disciplined engineering commitment, not a claim that all possible knowledge can be exhaustively classified in nine ranks.

## References

- [ISO/IEC 21838-2:2021 — Basic Formal Ontology](https://www.iso.org/standard/74572.html)
- [BFO-2020 artifacts](https://github.com/BFO-ontology/BFO-2020)
- [DOLCE — Laboratory for Applied Ontology](https://www.loa.istc.cnr.it/dolce/overview.html)
- [GFO — Onto-Med Research Group](https://www.onto-med.de/ontologies/gfo/)
- [UFO — NEMO Research Group](https://nemo.inf.ufes.br/projetos/ufo/)
- [SUMO — Ontology Portal](https://ontologyportal.org/)
- [Guarino, *What Is an Ontology?*](https://iaoa.org/isc2012/docs/Guarino2009_What_is_an_Ontology.pdf)
- [Noy and McGuinness, *Ontology Development 101*](https://protege.stanford.edu/publications/ontology_development/ontology101-noy-mcguinness.html)
