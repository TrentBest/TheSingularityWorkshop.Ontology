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
        var address = new OntologyAddress(
            ontologyId,
            ontologyVersion,
            path.Trim('/'))
        {
            Index = index ?? new OntologyIndex(0)
        };

        address.Validate();
        return address;
    }

    /// <summary>
    /// Validates the qualified semantic identity carried by this address.
    /// </summary>
    public void Validate()
    {
        if (OntologyId == 0)
            throw new ArgumentOutOfRangeException(nameof(OntologyId));

        if (string.IsNullOrWhiteSpace(OntologyVersion))
            throw new ArgumentException("Ontology version is required.", nameof(OntologyVersion));

        if (string.IsNullOrWhiteSpace(Path))
            throw new ArgumentException("Ontology path is required.", nameof(Path));
    }

    /// <summary>Returns the canonical address representation.</summary>
    public override string ToString() =>
        $"ontology://{OntologyId}/{OntologyVersion}/{Path.Trim('/')}";
}
