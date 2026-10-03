namespace TheSingularityWorkshop.Ontology;

/// <summary>
/// Stable semantic location qualified by ontology identity and version.
/// </summary>
public readonly record struct OntologyAddress(
    ulong OntologyId,
    string OntologyVersion,
    string Path)
{
    /// <summary>Creates a normalized semantic address.</summary>
    public static OntologyAddress Create(ulong ontologyId, string ontologyVersion, string path)
    {
        if (ontologyId == 0)
            throw new ArgumentOutOfRangeException(nameof(ontologyId));

        if (string.IsNullOrWhiteSpace(ontologyVersion))
            throw new ArgumentException("Ontology version is required.", nameof(ontologyVersion));

        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Ontology path is required.", nameof(path));

        return new OntologyAddress(ontologyId, ontologyVersion, path.Trim('/'));
    }

    /// <summary>Returns the canonical address representation.</summary>
    public override string ToString() =>
        $"ontology://{OntologyId}/{OntologyVersion}/{Path.Trim('/')}";
}
