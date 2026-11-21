namespace TheSingularityWorkshop.Ontology;

/// <summary>
/// Declares that a server covers a half-open semantic range. This is metadata
/// only; it does not discover servers, route requests, or perform storage.
/// </summary>
public sealed record OntologyCoverage(
    string ServerId,
    ulong OntologyId,
    string OntologyVersion,
    OntologyAddress Start,
    OntologyAddress End)
{
    /// <summary>Validates the server reference and covered semantic range.</summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ServerId))
            throw new ArgumentException("A server identifier is required.", nameof(ServerId));

        new OntologyRange(OntologyId, OntologyVersion, Start, End).Validate();
    }
}
