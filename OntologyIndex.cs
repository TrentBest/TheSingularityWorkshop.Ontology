namespace TheSingularityWorkshop.Ontology;

/// <summary>
/// Represents an ontology location as either one integer index or an ordered
/// collection of integer coordinates.
/// </summary>
public readonly struct OntologyIndex : IEquatable<OntologyIndex>
{
    private readonly ulong _value;
    private readonly ulong[]? _coordinates;

    /// <summary>Creates a scalar, single-dimension ontology index.</summary>
    public OntologyIndex(ulong value)
    {
        _value = value;
        _coordinates = null;
    }

    private OntologyIndex(ulong[] coordinates)
    {
        if (coordinates.Length == 0)
            throw new ArgumentException("At least one index coordinate is required.", nameof(coordinates));

        _value = 0;
        _coordinates = coordinates;
    }

    /// <summary>Gets the number of dimensions represented by this index.</summary>
    public int Rank => _coordinates?.Length ?? 1;

    /// <summary>Gets whether this is the default single-integer form.</summary>
    public bool IsScalar => _coordinates is null;

    /// <summary>Gets the scalar value. Throws when the index is multidimensional.</summary>
    public ulong Value =>
        IsScalar
            ? _value
            : throw new InvalidOperationException("A multidimensional index does not have a single Value.");

    /// <summary>Gets an index coordinate by dimension.</summary>
    public ulong this[int dimension] =>
        IsScalar
            ? dimension == 0
                ? _value
                : throw new IndexOutOfRangeException()
            : _coordinates![dimension];

    /// <summary>Gets a defensive copy of the coordinates.</summary>
    public IReadOnlyList<ulong> Coordinates =>
        IsScalar
            ? [_value]
            : _coordinates!.ToArray();

    /// <summary>Creates an ontology index from ordered coordinates.</summary>
    public static OntologyIndex Create(params ulong[] coordinates)
    {
        ArgumentNullException.ThrowIfNull(coordinates);

        if (coordinates.Length == 0)
            throw new ArgumentException("At least one index coordinate is required.", nameof(coordinates));

        return coordinates.Length == 1
            ? new OntologyIndex(coordinates[0])
            : new OntologyIndex(coordinates.ToArray());
    }

    /// <inheritdoc />
    public bool Equals(OntologyIndex other)
    {
        if (IsScalar && other.IsScalar)
            return _value == other._value;

        if (Rank != other.Rank)
            return false;

        for (var dimension = 0; dimension < Rank; dimension++)
        {
            if (this[dimension] != other[dimension])
                return false;
        }

        return true;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        obj is OntologyIndex other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new HashCode();
        for (var dimension = 0; dimension < Rank; dimension++)
            hash.Add(this[dimension]);

        return hash.ToHashCode();
    }

    /// <summary>Compares two ontology indexes.</summary>
    public static bool operator ==(OntologyIndex left, OntologyIndex right) => left.Equals(right);

    /// <summary>Compares two ontology indexes for inequality.</summary>
    public static bool operator !=(OntologyIndex left, OntologyIndex right) => !left.Equals(right);

    /// <inheritdoc />
    public override string ToString() =>
        IsScalar
            ? _value.ToString()
            : $"[{string.Join(",", _coordinates!)}]";
}
