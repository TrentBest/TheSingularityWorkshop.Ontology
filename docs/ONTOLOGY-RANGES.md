# Ontology Ranges and Coverage

## Purpose

An ontology range names a bounded portion of one exact ontology version. A coverage record associates that semantic portion with a server identifier without making Ontology responsible for finding servers, selecting routes, or reading/writing storage.

These are metadata contracts. They do not imply that the covered portion is densely materialized, physically contiguous, or currently available.

## Half-open range semantics

`OntologyRange` carries:

- `OntologyId` and `OntologyVersion` — the exact ontology identity;
- `Start` — the inclusive semantic address;
- `End` — the exclusive semantic address.

The interval is **[Start, End)**. The start is included; the end is not. Adjacent ranges can therefore share a boundary without overlapping:

```text
[0, 10) and [10, 20)
```

Both endpoints must be valid addresses belonging to the declared ontology identity and version. Their N-dimensional indexes must have equal rank, and the start coordinate must precede the end coordinate in lexicographic coordinate order (dimension 0 first).

Use OntologyRange.Contains(address) to test membership. It includes the start, excludes the end, and returns false for addresses qualified by another ontology/version or with a different coordinate rank.

The range does not infer dimension extents or flatten coordinates. If a consumer needs bounded-space membership or row-major offsets, it can use `OntologyIndexSpace` separately. The range's ordering contract is coordinate ordering, not physical placement ordering.

## Placement-neutral coverage

`OntologyCoverage` exposes the portable metadata shape:

| Field | Meaning |
|---|---|
| `ServerId` | Opaque identifier assigned by the system that owns server identity |
| `OntologyId` | Ontology identity |
| `OntologyVersion` | Exact ontology version |
| `Start` | Inclusive semantic address |
| `End` | Exclusive semantic address |

The server identifier is intentionally an opaque, non-empty string. Ontology does not prescribe its format, resolve it, discover the server, or establish whether that server is online.

A repository or runtime may store and consume these records later. Such a consumer owns placement policy, availability, replication, authorization, transport, and physical storage.

## Example

```csharp
var coverage = new OntologyCoverage(
    ServerId: "server-west",
    OntologyId: 42,
    OntologyVersion: "1.0.0",
    Start: OntologyAddress.Create(
        42, "1.0.0", "world/region/start", OntologyIndex.Create(0, 0)),
    End: OntologyAddress.Create(
        42, "1.0.0", "world/region/end", OntologyIndex.Create(0, 100)));

coverage.Validate();
```

The record says only that the named server is associated with that semantic interval. It does not claim that Ontology can contact the server or that any particular artifact has been stored there.

## Deliberate boundaries

- No fixed layer count or nine-layer dependency.
- No dependency on MicroBundleRepository, FSM_COS, ProtocolAI, Azure, or a storage provider.
- No server discovery, routing, replication, or persistence behavior.
- No dense allocation or flattening requirement.
- No claim that semantic coverage implies physical contiguity.

The distinction remains: Ontology describes meaning and semantic address ranges; infrastructure decides where those ranges are materialized and how they are reached.
