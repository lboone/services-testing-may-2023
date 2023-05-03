using Moq;
using System.Xml.Linq;

namespace SlugGenerators.UnitTests;

public class GeneratingSlug
{



    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public async Task InvalidEntriesThrows(string invalidSlugCandidate)
    {

        var alwaysUniqueChecker = new Mock<ICheckForUniqueValues>();
        alwaysUniqueChecker.Setup(u => u.IsUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
        var generator = new SlugGenerator(alwaysUniqueChecker.Object);

        await Assert.ThrowsAsync<InvalidSlugCandidateException>(() => generator.GenerateSlugForAsync(invalidSlugCandidate)).ConfigureAwait(false);
    }
    
    [Theory]
    [InlineData("Dandruff Shampoo", "dandruff-shampoo")]
    [InlineData("Taco Salad", "taco-salad")]
    [InlineData("Hamburger", "hamburger")]
    [InlineData("Dandruff    Shampoo", "dandruff-shampoo")]
    
    public async Task CanGenerateSlugs(string name, string expectedSlug)
    {
        var alwaysUniqueChecker = new Mock<ICheckForUniqueValues>();
        alwaysUniqueChecker.Setup(u => u.IsUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
        var generator = new SlugGenerator(alwaysUniqueChecker.Object);

        var slug = await generator.GenerateSlugForAsync(name);

        Assert.Equal(expectedSlug, slug);
    }


    [Theory]
    [InlineData("Coffee Beans", "coffee-beans-a")]
    [InlineData("Bird Seed", "bird-seed-d")]
    [InlineData("Burrito", "burrito-x")]
    public async Task DuplicatesHaveSuffix(string name, string expected)
    {
        var neverUniqueChecker = new Mock<ICheckForUniqueValues>();
        neverUniqueChecker.Setup(u => u.IsUniqueAsync(expected)).ReturnsAsync(true);

        //"bird-seed", "bird-seed-a", "bird-seed-c", "bird-seed-d"
        var generator = new SlugGenerator(neverUniqueChecker.Object);


        var slug = await generator.GenerateSlugForAsync(name);

        Assert.Equal(expected, slug);
    }

    [Fact]
    public async Task IfUniqueNameIsntAvailableGenerateGuid()
    {
        var neverUniqueChecker = new Mock<ICheckForUniqueValues>();


        var generator = new SlugGenerator(neverUniqueChecker.Object);

        var slug = await generator.GenerateSlugForAsync("Ba Da");

        Assert.StartsWith("ba-da", slug);
        Assert.Equal(41, slug.Length);
    }
}
