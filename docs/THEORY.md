# Ontology Theory

## Why an ontology exists

A MicroBundle can contain meaningful structure without being forced into a single global taxonomy.

The purpose of this package is to provide a place for **meaning to become addressable** when an application chooses to model it.

> **MicroBundleDomain defines how a MicroBundle participates in a system. Ontology defines what a chosen semantic structure means.**

Ontology is therefore an optional semantic layer, not a rule imposed on every MicroBundle.

## The core idea: meaning before manifestation

A system becomes brittle when behavior is selected by implementation-specific conditionals:

````text
if (thing is Fish)
    playSwimAnimation();
````

An ontology allows the relationship to be expressed semantically instead:

````text
life / animal / fish / locomotion / swim
                    |
                    | manifests-as
                    v
             animation / swim
````

The renderer, UI, simulation, or other manifestation can resolve that semantic relationship without owning the taxonomy that produced it.

This does **not** mean that every behavior must be ontology-driven. It means that an application can choose semantic addressing where it provides value.

## Ontology is not a hierarchy requirement

The word *ontology* often suggests a fixed tree. This package intentionally does not make that assumption.

A developer may define:

- five layers;
- nine layers;
- a flat vocabulary;
- several branches with different depths;
- relationships that cross branches;
- multiple ontologies with different structures;
- or no ontology at all.

Layer definitions are therefore data, not framework law.

## Identity and version are part of meaning

An address is qualified by ontology identity and version:

``ontology://<ontology-id>/<version>/<semantic-path>``

For example:

``ontology://42/1.0.0/life/animal/fish/locomotion/swim``

The same semantic path in another ontology is not silently assumed to mean the same thing.

## Multiple ontologies can coexist

There is no universal ontology registry hidden inside this package.

An application can load:

````text
                    OntologySet
                       |
          +------------+------------+
          |            |            |
       Animals        AEC        Physics
       1.0.0         1.0.0         3.2.0
          |            |            |
       taxonomy      spaces       materials
````

A relationship may explicitly connect addresses from different ontologies.

## The master ontology is content, not authority

The Singularity Workshop may eventually publish a broad or “master” ontology containing families such as animal life, chemistry and materials, physics, AEC, rendering, interaction, and other domain knowledge.

That ontology is still **content**.

A developer using the Workshop's MicroBundle infrastructure is not required to adopt nine layers, the master ontology, or any particular taxonomy. A smaller application can select only the semantic structure it needs.

## Ontology and storage are separate concerns

Semantic identity should not require one enormous physical data blob.

````text
Semantic address
      |
      v
ontology://42/1.0.0/life/animal/fish/locomotion/swim
      |
      +----> behavior provider
      +----> animation asset
      +----> simulation rule
      +----> documentation
      +----> remote repository location
````

The ontology package does not become a storage engine, and the repository does not need to understand the ontology's meaning merely to store bytes.

## Ontology and MicroBundleDomain

The dependency boundary is intentionally directional:

````text
                    MicroBundleDomain
                    generic mechanism
                           ^
                           |
                    Ontology package
                    optional semantics
                           |
              +------------+------------+
              |            |            |
           Animals        AEC       Application
````

The ontology package may use MicroBundleDomain because ontology itself can be delivered as a MicroBundle.

The reverse dependency would be architectural leakage: a generic MicroBundle mechanism should not need to understand ontology.

## Ontology and FSM_COS

FSM_COS is the composition boundary.

It may compose ontology MicroBundles, but it should not contain hard-coded branches such as:

````text
if ontology == Animals ...
if ontology == AEC ...
if ontology == Physics ...
````

Instead, FSM_COS should compose the declared runtime pieces and leave semantic interpretation to the packages that own that meaning.

## What this package deliberately does not decide

The initial contract does not attempt to solve every ontology problem.

It does not yet prescribe:

- a universal ontology language;
- a universal reasoning engine;
- a mandatory graph database;
- a fixed number of layers;
- a single global taxonomy;
- a universal behavior-provider interface;
- a physical storage layout;
- or a particular renderer.

Those are higher-level design choices that should be introduced when concrete use cases demonstrate the need.

## Design principle

> **Define semantic structure without making semantic structure mandatory.**

That lets a developer start with five layers instead of nine, use two unrelated ontologies together, map a semantic concept to a behavior without teaching the renderer taxonomy, or skip ontology entirely when it adds no value.

The package exists to make those choices composable rather than to make one choice inevitable.