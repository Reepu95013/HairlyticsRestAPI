using Hairlytics.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Domain.Interfaces
{
    public interface ICategoryRepository
    {
       Task AddCategory(Category category);
       Task UpdateCategory(Category category);
       Task<int> GetCategoryCountAsync();
       Task<List<Category>> GetCategoryList(int pageNumber, int pageSize);
       Task<List<Category>>GetCategoryList();
       Task<Category?> GetCategory(int categoryId);
       Task DeleteCategory(int categoryId);

    }
}
