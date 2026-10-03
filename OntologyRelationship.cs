namespace TheSingularityWorkshop.Ontology;

/// <summary>
/// Declares a relationship between two semantic addresses.
/// </summary>
public sealed record OntologyRelationship(
    OntologyAddress Source,
    string Predicate,
    OntologyAddress Target)
{
    /// <summary>Validates the relationship.</summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Predicate))
            throw new ArgumentException("Relationship predicates are required.", nameof(Predicate));
    }
}
