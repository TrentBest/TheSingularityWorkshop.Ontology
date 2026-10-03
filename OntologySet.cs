namespace TheSingularityWorkshop.Ontology;

/// <summary>
/// Holds multiple coexisting ontology definitions without selecting one as globally authoritative.
/// </summary>
public sealed class OntologySet
{
    private readonly Dictionary<(ulong Id, string Version), OntologyDefinition> _definitions = new();

    /// <summary>Adds an ontology definition.</summary>
    public void Add(OntologyDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);
        definition.Validate();

        var key = (definition.Id, definition.Version);
        if (!_definitions.TryAdd(key, definition))
            throw new InvalidOperationException(
                $"Ontology '{definition.Name}' with identity {definition.Id}@{definition.Version} is already registered.");
    }

    /// <summary>Gets a registered ontology by identity.</summary>
    public bool TryGet(ulong id, string version, out OntologyDefinition? definition) =>
        _definitions.TryGetValue((id, version), out definition);

    /// <summary>Gets all registered ontology definitions.</summary>
    public IReadOnlyCollection<OntologyDefinition> Definitions => _definitions.Values;
}
