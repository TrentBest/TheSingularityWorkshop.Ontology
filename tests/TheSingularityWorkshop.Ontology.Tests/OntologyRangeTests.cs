using Xunit;

namespace TheSingularityWorkshop.Ontology.Tests;

public sealed class OntologyRangeTests
{
    [Fact]
    public void Range_is_half_open_and_accepts_ordered_multidimensional_endpoints()
    {
        var range = new OntologyRange(
            42,
            "1.0.0",
            OntologyAddress.Create(42, "1.0.0", "space/start", OntologyIndex.Create(1, 9)),
            OntologyAddress.Create(42, "1.0.0", "space/end", OntologyIndex.Create(2, 0)));

        range.Validate();
    }

    [Fact]
    public void Range_rejects_equal_or_reversed_endpoints()
    {
        var start = OntologyAddress.Create(42, "1.0.0", "space/start", OntologyIndex.Create(2, 0));
        var equal = OntologyAddress.Create(42, "1.0.0", "space/end", OntologyIndex.Create(2, 0));
        var earlier = OntologyAddress.Create(42, "1.0.0", "space/end", OntologyIndex.Create(1, ulong.MaxValue));

        Assert.Throws<ArgumentException>(() => new OntologyRange(42, "1.0.0", start, equal).Validate());
        Assert.Throws<ArgumentException>(() => new OntologyRange(42, "1.0.0", start, earlier).Validate());
    }

    [Fact]
    public void Range_rejects_mismatched_identity_version_and_rank()
    {
        var start = OntologyAddress.Create(42, "1.0.0", "start", OntologyIndex.Create(1, 0));
        var wrongOntology = OntologyAddress.Create(43, "1.0.0", "end", OntologyIndex.Create(2, 0));
        var wrongVersion = OntologyAddress.Create(42, "2.0.0", "end", OntologyIndex.Create(2, 0));
        var wrongRank = OntologyAddress.Create(42, "1.0.0", "end", new OntologyIndex(2));

        Assert.Throws<ArgumentException>(() => new OntologyRange(42, "1.0.0", start, wrongOntology).Validate());
        Assert.Throws<ArgumentException>(() => new OntologyRange(42, "1.0.0", start, wrongVersion).Validate());
        Assert.Throws<ArgumentException>(() => new OntologyRange(42, "1.0.0", start, wrongRank).Validate());
    }

    [Fact]
    public void Coverage_is_placement_metadata_and_validates_its_half_open_range()
    {
        var coverage = new OntologyCoverage(
            "server-west",
            42,
            "1.0.0",
            OntologyAddress.Create(42, "1.0.0", "start", OntologyIndex.Create(0, 0)),
            OntologyAddress.Create(42, "1.0.0", "end", OntologyIndex.Create(0, 10)));

        coverage.Validate();
        Assert.Equal("server-west", coverage.ServerId);
        Assert.Equal(42UL, coverage.OntologyId);
        Assert.Equal("1.0.0", coverage.OntologyVersion);
    }

    [Fact]
    public void Coverage_rejects_missing_server_identifier()
    {
        var coverage = new OntologyCoverage(
            " ",
            42,
            "1.0.0",
            OntologyAddress.Create(42, "1.0.0", "start", new OntologyIndex(0)),
            OntologyAddress.Create(42, "1.0.0", "end", new OntologyIndex(1)));

        Assert.Throws<ArgumentException>(() => coverage.Validate());
    }
}
