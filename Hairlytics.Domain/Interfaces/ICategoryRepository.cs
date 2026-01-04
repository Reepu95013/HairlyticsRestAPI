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
       Task <IEnumerable<Category>>GetCategories();
       Task <IEnumerable<Category>> GetCategoriesByVendor(int vendorId);
       Task<IEnumerable<Category>> GetCategoriesByVendorAndDefault(int vendorId);
       Task <bool> DeleteCategory(int categoryId);






    }
}
