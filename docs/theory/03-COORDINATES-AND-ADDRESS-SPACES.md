# 03 — Coordinates, Addresses, and Address Spaces

## 01 — What: an address has identity; a coordinate has position

A coordinate such as [2, 14, 7, 3] is not self-describing. Its dimensions only make sense in the context of the ontology that defines them. Ontology distinguishes a compact index from a portable semantic address qualified by ontology identity and version.

## 02 — Why: separate semantic identity from representation

A scalar integer is compact and often sufficient. Other domains need several independent coordinate dimensions. A generic model should support both without declaring that every ontology has the same rank.

A street address and a grid coordinate can both locate something, but their interpretation depends on a shared reference system. Neither is a storage blob index or a runtime-local handle.

## 03 — How: the mathematical model

OntologyIndex is either one ulong value or an ordered list of ulong coordinates. Its rank is at least one. A one-coordinate index is equal to its scalar form.

For dimension extents D₀, D₁, …, Dₙ₋₁, the bounded Cartesian address-space cardinality is:

[
|A| = prod_{i=0}^{N-1} D_i
]

When each dimension spans the complete ulong value domain, there are 2⁶⁴ values per dimension and the N-dimensional cardinality is 2⁶⁴ᴺ. This counts possible coordinates; it is not an allocation request.

For dense row-major storage with extents [D₀,D₁,D₂], a consumer may map a coordinate to one linear offset:

[
((i_0D_1)+i_1)D_2+i_2
]

This mapping is an optional dense-array convenience. Semantic addresses do not have to be flattened.

## 04 — See it: scalar, N-D, and dense mapping

~~~csharp
var scalar = new OntologyIndex(42);
var coordinate = OntologyIndex.Create(2, 14, 7, 3);

var space = OntologyIndexSpace.Create(3, 20, 10, 5);
var offset = space.Flatten(coordinate);       // 2738
var roundTrip = space.Unflatten(offset);      // [2,14,7,3]
~~~

Verified example: these values are asserted in the test suite. The bounded space is small enough that the row-major offset can be inspected by hand.

## 05 — Implementation and evidence

- [OntologyIndex.cs](../../OntologyIndex.cs) implements scalar and N-dimensional forms, defensive coordinate copies, rank, equality, and formatting.
- [OntologyAddress.cs](../../OntologyAddress.cs) qualifies a path by ontology ID and version and can carry an optional index.
- [OntologyIndexSpace.cs](../../OntologyIndexSpace.cs) validates positive extents, computes cardinality with BigInteger, and implements Flatten and Unflatten.
- [OntologyTests.cs](../../tests/TheSingularityWorkshop.Ontology.Tests/OntologyTests.cs) covers scalar/multi-coordinate equality, full-ulong cardinality, round-trip flattening, and invalid coordinates.

## Limits and open questions

An index is not automatically a globally unique concept ID. Its meaning depends on ontology identity, version, and coordinate convention. OntologyIndexSpace models dense rectangular extents; it does not provide sparse allocation, a database, a distributed index, or coordinate migration between ontology versions.

A ulong coordinate can represent values through ulong.MaxValue, but the number of possible values in one full dimension is 2⁶⁴. Keep maximum coordinate and dimension extent/cardinality distinct.

## Practice

1. Why can a nine-dimensional address space have cardinality greater than ulong.MaxValue?
2. What additional information is needed to interpret an integer coordinate?
3. Which parts of the example are semantic identity, and which are dense-storage convenience?

## Further reading

- [Address-space mathematics](../MATHEMATICS.md)
- [Address contract](../../OntologyAddress.cs)
- [Index implementation](../../OntologyIndex.cs)
- [Index-space implementation](../../OntologyIndexSpace.cs)
