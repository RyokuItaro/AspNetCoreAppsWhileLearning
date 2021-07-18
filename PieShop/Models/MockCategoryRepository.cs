using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PieShop.Models
{
    public class MockCategoryRepository : ICategoryRepository
    {
        public IEnumerable<Category> AllCategories =>
            new List<Category>
            {
                new Category{CategoryId=1, CategoryName="Sernik", Description="ser" },
                new Category{CategoryId=2, CategoryName="Owocowe", Description="owoce"},
                new Category{CategoryId=3, CategoryName="Sezonowe", Description="sezon" }
            };
    }
}
