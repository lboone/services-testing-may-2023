using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlugGenerators;


public class SlugGenerator
{
    private readonly ICheckForUniqueValues _uniquenessChecker;

    public SlugGenerator(ICheckForUniqueValues uniquenessChecker)
    {
        _uniquenessChecker = uniquenessChecker;
    }

    public async Task<string> GenerateSlugForAsync(string name)
    {

        if(name.Trim() == string.Empty)
        {
            throw new InvalidSlugCandidateException();
        }
        var config = new Slugify.SlugHelperConfiguration
        {
            CollapseWhiteSpace = true,

        };

        var slugger = new Slugify.SlugHelper(config);
        var attempt = slugger.GenerateSlug(name);

        bool isUnique = await _uniquenessChecker.IsUniqueAsync(attempt);

        if (isUnique)
        {

            return attempt;
        }
        else
        {
            var letters = "abcdefhijklmnopqrstuvwxyz";
            var idx = 0;
            while (idx < letters.Length)
            {
                var retryAttempt = attempt + "-" + letters[idx];
                isUnique = await _uniquenessChecker.IsUniqueAsync(retryAttempt);
                if (isUnique)
                {
                    return retryAttempt;
                }
                else
                {
                    idx++;
                }
            }
            return attempt + Guid.NewGuid().ToString(); // discovered this.
        }
    }
}
