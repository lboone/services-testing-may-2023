using SlugGenerators;

namespace ProductsApi.Products;

public interface IGenerateSlugs
{
     Task<String> GenerateSlugForAsync(string name);
}

public class SlugGeneratorFacade : IGenerateSlugs
{
    private readonly ICheckForUniqueValues _unique;

    public SlugGeneratorFacade(ICheckForUniqueValues unique)
    {
        _unique = unique;
    }

    public async Task<string> GenerateSlugForAsync(string name)
    {
        var sg = new SlugGenerators.SlugGenerator(_unique);

        return await sg.GenerateSlugForAsync(name);

    }
}