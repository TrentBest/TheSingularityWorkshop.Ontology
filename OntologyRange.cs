namespace TheSingularityWorkshop.Ontology;

/// <summary>
/// Describes a half-open, placement-independent interval in one versioned
/// ontology. The start is included and the end is excluded.
/// </summary>
public sealed record OntologyRange(
    ulong OntologyId,
    string OntologyVersion,
    OntologyAddress Start,
    OntologyAddress End)
{
    /// <summary>Validates ontology identity, endpoint qualification, rank, and ordering.</summary>
    public void Validate()
    {
        if (OntologyId == 0)
            throw new ArgumentOutOfRangeException(nameof(OntologyId));

        if (string.IsNullOrWhiteSpace(OntologyVersion))
            throw new ArgumentException("Ontology version is required.", nameof(OntologyVersion));

        Start.Validate();
        End.Validate();

        if (Start.OntologyId != OntologyId || End.OntologyId != OntologyId)
            throw new ArgumentException("Range endpoints must belong to the declared ontology.");

        if (!string.Equals(Start.OntologyVersion, OntologyVersion, StringComparison.Ordinal) ||
            !string.Equals(End.OntologyVersion, OntologyVersion, StringComparison.Ordinal))
            throw new ArgumentException("Range endpoints must use the declared ontology version.");

        if (Start.Index.Rank != End.Index.Rank)
            throw new ArgumentException("Range endpoints must have the same coordinate rank.");

        if (Compare(Start.Index, End.Index) >= 0)
            throw new ArgumentException("The inclusive start coordinate must precede the exclusive end coordinate.");
    }

    /// <summary>Returns whether an address is inside this half-open semantic interval.</summary>
    public bool Contains(OntologyAddress address)
    {
        Validate();
        address.Validate();

        if (address.OntologyId != OntologyId ||
            !string.Equals(address.OntologyVersion, OntologyVersion, StringComparison.Ordinal) ||
            address.Index.Rank != Start.Index.Rank)
            return false;

        return Compare(Start.Index, address.Index) <= 0 &&
               Compare(address.Index, End.Index) < 0;
    }

    private static int Compare(OntologyIndex left, OntologyIndex right)
    {
        for (var dimension = 0; dimension < left.Rank; dimension++)
        {
            var comparison = left[dimension].CompareTo(right[dimension]);
            if (comparison != 0)
                return comparison;
        }

        return 0;
    }
}
