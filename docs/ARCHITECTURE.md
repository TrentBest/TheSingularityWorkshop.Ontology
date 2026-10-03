# Ontology Architecture

![Ontology boundary](ontology-boundary.svg)

The package sits above the generic MicroBundle mechanism and beside application/domain content. It does not become the authority for every MicroBundle.

## Boundary map

````text
MicroBundleDomain
       |
       +---- Ontology ---- semantic definitions
       |
       +---- Domain MicroBundles ---- application meaning

FSM_COS composes the resulting pieces without owning their ontology.
````

| Concern | Owner | Ontology dependency |
|---|---|---|
| MicroBundle lifecycle and contracts | MicroBundleDomain | None |
| Semantic definitions and addresses | Ontology | MicroBundleDomain is allowed |
| Physical storage and retrieval | MicroBundleRepository | None required |
| Runtime composition | FSM_COS | May compose ontology bundles |
| Domain meaning | Application/domain MicroBundles | Optional |
| Manifestation | Renderer/UI/simulation packages | Optional semantic resolution |

## Semantic resolution flow

````text
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
````

The important boundary is that the semantic address does not itself contain the implementation. It identifies meaning. A relationship can connect that meaning to an implementation owned elsewhere.

## Example: fish locomotion

A hypothetical ontology can declare:

````text
ontology://100/1.0.0/life/animal/fish
    |
    +-- locomotion
          |
          +-- swim
````

A separate rendering or animation ontology can declare:

````text
ontology://200/1.0.0/animation/swim
````

A relationship can connect them:

````text
fish/swim  --manifests-as-->  animation/swim
````

The fish ontology does not need to know how the animation is rendered, and the renderer does not need to encode the animal taxonomy.

## Coexisting structures

The architecture supports intentionally different structures:

````text
Application A                  Application B
     |                              |
  5 layers                      9 layers
     |                              |
  Ontology A                    Ontology B
     |                              |
     +---------------+--------------+
                     |
                 same runtime
````

An application may also use no ontology:

````text
MicroBundleDomain
       |
       +---- application content
````

This is a feature, not an incomplete state.

## Evolution

The package should grow from demonstrated use cases.

The preferred progression is:

1. stable identity and versioning;
2. semantic addressing;
3. explicit relationships;
4. composition and mapping contracts proven by real applications;
5. optional tooling and visualization;
6. richer reasoning only when an actual consumer requires it.

That keeps the core small while leaving room for the larger Singularity Workshop semantic model.