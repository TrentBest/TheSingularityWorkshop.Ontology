namespace TheSingularityWorkshop.Ontology;

/// <summary>
/// Defines the canonical nine-position classification profile used by The
/// Singularity Workshop without restricting independent ontologies to nine layers.
/// </summary>
public static class WorkshopOntologySchema
{
    /// <summary>The number of ordered positions in the canonical Workshop profile.</summary>
    public const int LayerCount = 9;

    private static readonly string[] Names =
    [
        "Paradigm",
        "Domain",
        "Kingdom",
        "Phylum",
        "Class",
        "Order",
        "Family",
        "Genus",
        "Species"
    ];

    /// <summary>Gets the canonical layer names in semantic order.</summary>
    public static IReadOnlyList<string> LayerNames { get; } = Array.AsReadOnly(Names);

    /// <summary>
    /// Creates canonical layer definitions using caller-supplied stable layer IDs.
    /// IDs are deliberately not generated from array positions.
    /// </summary>
    /// <param name="layerIds">Nine unique, non-zero, stable layer identifiers in canonical order.</param>
    public static IReadOnlyList<OntologyLayerDefinition> CreateLayers(params ulong[] layerIds)
    {
        ArgumentNullException.ThrowIfNull(layerIds);

        if (layerIds.Length != LayerCount)
        {
            throw new ArgumentException(
                $"The canonical Workshop ontology requires exactly {LayerCount} layer identifiers.",
                nameof(layerIds));
        }

        if (layerIds.Any(static id => id == 0))
            throw new ArgumentOutOfRangeException(nameof(layerIds), "Layer identifiers must be non-zero.");

        if (layerIds.Distinct().Count() != LayerCount)
            throw new ArgumentException("Canonical layer identifiers must be unique.", nameof(layerIds));

        var layers = Names
            .Select((name, order) => new OntologyLayerDefinition(layerIds[order], name, order))
            .ToArray();

        return Array.AsReadOnly(layers);
    }

    /// <summary>Creates and validates an ontology definition using the canonical nine layers.</summary>
    public static OntologyDefinition CreateDefinition(
        ulong ontologyId,
        string name,
        string version,
        params ulong[] layerIds)
    {
        var definition = new OntologyDefinition(
            ontologyId,
            name,
            version,
            CreateLayers(layerIds));

        definition.Validate();
        return definition;
    }

    /// <summary>Creates a semantic address whose coordinate matches the canonical nine-layer rank.</summary>
    public static OntologyAddress CreateAddress(
        OntologyDefinition definition,
        string path,
        OntologyIndex index)
    {
        ArgumentNullException.ThrowIfNull(definition);

        if (!IsCanonical(definition))
            throw new ArgumentException("The ontology definition does not use the canonical Workshop schema.", nameof(definition));

        if (index.Rank != LayerCount)
        {
            throw new ArgumentException(
                $"The canonical Workshop schema requires an index of rank {LayerCount}; received rank {index.Rank}.",
                nameof(index));
        }

        return definition.Address(path, index);
    }

    /// <summary>Returns whether a valid definition has the canonical layer names and ordering.</summary>
    public static bool IsCanonical(OntologyDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);

        if (definition.Layers is null || definition.Layers.Count != LayerCount)
            return false;

        if (definition.Layers.Any(static layer => layer is null))
            return false;

        try
        {
            definition.Validate();
        }
        catch (ArgumentException)
        {
            return false;
        }

        var orderedLayers = definition.Layers.OrderBy(static layer => layer.Order).ToArray();

        for (var order = 0; order < LayerCount; order++)
        {
            var layer = orderedLayers[order];
            if (layer.Order != order ||
                !string.Equals(layer.Name, Names[order], StringComparison.Ordinal))
            {
                return false;
            }
        }

        return true;
    }
}
