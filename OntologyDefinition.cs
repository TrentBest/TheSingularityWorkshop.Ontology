namespace TheSingularityWorkshop.Ontology;

/// <summary>
/// Defines an independently versioned ontology and its semantic address space.
/// </summary>
public sealed record OntologyDefinition(
    ulong Id,
    string Name,
    string Version,
    IReadOnlyList<OntologyLayerDefinition> Layers)
{
    /// <summary>Gets a semantic address within this ontology.</summary>
    public OntologyAddress Address(string path, OntologyIndex? index = null) =>
        OntologyAddress.Create(Id, Version, path, index);

    /// <summary>Validates the definition's identity and layer declarations.</summary>
    public void Validate()
    {
        if (Id == 0)
            throw new ArgumentOutOfRangeException(nameof(Id), "Ontology identifiers must be non-zero.");

        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Ontology names are required.", nameof(Name));

        if (string.IsNullOrWhiteSpace(Version))
            throw new ArgumentException("Ontology versions are required.", nameof(Version));

        ArgumentNullException.ThrowIfNull(Layers);

        var duplicateIds = Layers.GroupBy(layer => layer.Id).Where(group => group.Count() > 1);
        if (duplicateIds.Any())
            throw new ArgumentException("Ontology layer identifiers must be unique.", nameof(Layers));
    }
}

/// <summary>Describes one layer in an ontology without imposing a global layer count.</summary>
public sealed record OntologyLayerDefinition(
    ulong Id,
    string Name,
    int Order);
