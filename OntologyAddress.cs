namespace TheSingularityWorkshop.Ontology;

/// <summary>
/// Stable semantic location qualified by ontology identity, version, and an
/// optional compact integer index.
/// </summary>
public readonly record struct OntologyAddress(
    ulong OntologyId,
    string OntologyVersion,
    string Path)
{
    /// <summary>
    /// Gets the compact index for this address. The default is the scalar
    /// zero index, preserving the original path-only address form.
    /// </summary>
    public OntologyIndex Index { get; init; }

    /// <summary>Creates a normalized semantic address.</summary>
    public static OntologyAddress Create(
        ulong ontologyId,
        string ontologyVersion,
        string path,
        OntologyIndex? index = null)
    {
        if (ontologyId == 0)
            throw new ArgumentOutOfRangeException(nameof(ontologyId));

        if (string.IsNullOrWhiteSpace(ontologyVersion))
            throw new ArgumentException("Ontology version is required.", nameof(ontologyVersion));

        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Ontology path is required.", nameof(path));

        return new OntologyAddress(ontologyId, ontologyVersion, path.Trim('/'))
        {
            Index = index ?? new OntologyIndex(0)
        };
    }

    /// <summary>Returns the canonical address representation.</summary>
    public override string ToString() =>
        $"ontology://{OntologyId}/{OntologyVersion}/{Path.Trim('/')}";
}
