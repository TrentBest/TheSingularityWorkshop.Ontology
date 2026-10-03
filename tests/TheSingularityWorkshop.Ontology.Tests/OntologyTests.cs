using Xunit;

namespace TheSingularityWorkshop.Ontology.Tests;

public sealed class OntologyTests
{
    [Fact]
    public void Ontology_allows_arbitrary_layer_count()
    {
        var ontology = new OntologyDefinition(
            100,
            "FiveLayerExample",
            "1.0.0",
            [
                new(1, "One", 0),
                new(2, "Two", 1),
                new(3, "Three", 2),
                new(4, "Four", 3),
                new(5, "Five", 4)
            ]);

        ontology.Validate();

        Assert.Equal(5, ontology.Layers.Count);
    }

    [Fact]
    public void Ontologies_can_coexist_by_identity_and_version()
    {
        var set = new OntologySet();

        set.Add(new OntologyDefinition(1, "Animals", "1.0.0", []));
        set.Add(new OntologyDefinition(2, "AEC", "1.0.0", []));
        set.Add(new OntologyDefinition(1, "Animals", "2.0.0", []));

        Assert.Equal(3, set.Definitions.Count);
        Assert.True(set.TryGet(1, "2.0.0", out var animals));
        Assert.Equal("Animals", animals!.Name);
    }

    [Fact]
    public void Address_is_qualified_by_ontology_identity()
    {
        var address = OntologyAddress.Create(42, "1.0.0", "/life/animal/fish/locomotion/swim");

        Assert.Equal("ontology://42/1.0.0/life/animal/fish/locomotion/swim", address.ToString());
    }

    [Fact]
    public void Relationships_can_cross_ontology_boundaries()
    {
        var source = OntologyAddress.Create(1, "1.0.0", "life/animal/fish");
        var target = OntologyAddress.Create(2, "1.0.0", "animation/swim");

        var relationship = new OntologyRelationship(source, "manifests-as", target);

        relationship.Validate();

        Assert.Equal(target, relationship.Target);
    }
}
