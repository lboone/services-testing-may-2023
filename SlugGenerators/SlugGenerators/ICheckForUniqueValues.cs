using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlugGenerators;

public interface ICheckForUniqueValues
{
    Task<bool> IsUniqueAsync(string attempt);
}