using System.Numerics;

namespace TheSingularityWorkshop.Ontology;

/// <summary>
/// Describes a dense, row-major N-dimensional coordinate space and provides
/// reversible mapping between coordinates and a linear offset.
/// </summary>
public sealed class OntologyIndexSpace
{
    private static readonly BigInteger MaxCoordinateExtent = BigInteger.One << 64;
    private readonly BigInteger[] _extents;

    /// <summary>Creates a bounded N-dimensional space.</summary>
    public OntologyIndexSpace(params BigInteger[] extents)
    {
        ArgumentNullException.ThrowIfNull(extents);

        if (extents.Length == 0)
            throw new ArgumentException("At least one dimension is required.", nameof(extents));

        _extents = extents.ToArray();

        foreach (var extent in _extents)
        {
            if (extent <= 0)
                throw new ArgumentOutOfRangeException(nameof(extents), "Dimension extents must be positive.");

            if (extent > MaxCoordinateExtent)
                throw new ArgumentOutOfRangeException(
                    nameof(extents),
                    "A dimension cannot contain more than 2^64 addressable ulong coordinates.");
        }

        var cardinality = BigInteger.One;
        foreach (var extent in _extents)
            cardinality *= extent;

        Cardinality = cardinality;
    }

    /// <summary>Creates a space from conventional unsigned-integer dimension sizes.</summary>
    public static OntologyIndexSpace Create(params ulong[] extents)
    {
        ArgumentNullException.ThrowIfNull(extents);
        return new OntologyIndexSpace(extents.Select(static extent => new BigInteger(extent)).ToArray());
    }

    /// <summary>Creates a space with the complete ulong coordinate domain in every dimension.</summary>
    public static OntologyIndexSpace FullUInt64(int rank)
    {
        if (rank <= 0)
            throw new ArgumentOutOfRangeException(nameof(rank));

        return new OntologyIndexSpace(
            Enumerable.Repeat(MaxCoordinateExtent, rank).ToArray());
    }

    /// <summary>Gets the number of dimensions.</summary>
    public int Rank => _extents.Length;

    /// <summary>Gets the size of each dimension.</summary>
    public IReadOnlyList<BigInteger> Extents => _extents.ToArray();

    /// <summary>
    /// Gets the total number of addressable coordinates:
    /// the product of all dimension extents.
    /// </summary>
    public BigInteger Cardinality { get; }

    /// <summary>
    /// Converts an N-dimensional coordinate into a zero-based row-major linear offset.
    /// </summary>
    public BigInteger Flatten(OntologyIndex index)
    {
        if (index.Rank != Rank)
            throw new ArgumentException(
                $"Expected an index of rank {Rank}, but received rank {index.Rank}.",
                nameof(index));

        var offset = BigInteger.Zero;

        for (var dimension = 0; dimension < Rank; dimension++)
        {
            var coordinate = new BigInteger(index[dimension]);

            if (coordinate >= _extents[dimension])
                throw new ArgumentOutOfRangeException(
                    nameof(index),
                    $"Coordinate {coordinate} exceeds dimension {dimension} extent {_extents[dimension]}.");

            offset = (offset * _extents[dimension]) + coordinate;
        }

        return offset;
    }

    /// <summary>
    /// Converts a zero-based row-major linear offset back into an N-dimensional coordinate.
    /// </summary>
    public OntologyIndex Unflatten(BigInteger offset)
    {
        if (offset < 0 || offset >= Cardinality)
            throw new ArgumentOutOfRangeException(nameof(offset));

        var coordinates = new ulong[Rank];
        var remainder = offset;

        for (var dimension = Rank - 1; dimension >= 0; dimension--)
        {
            var coordinate = remainder % _extents[dimension];

            if (coordinate > ulong.MaxValue)
                throw new InvalidOperationException(
                    "The coordinate cannot be represented by the ulong coordinate model.");

            coordinates[dimension] = (ulong)coordinate;
            remainder /= _extents[dimension];
        }

        return OntologyIndex.Create(coordinates);
    }
}
