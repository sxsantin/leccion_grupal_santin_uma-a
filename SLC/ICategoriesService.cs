using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLC
{
    public interface ICategoriesService
    {
        Categories CreateCategory(Categories categories);
        bool Delete(int id);
    }
}
