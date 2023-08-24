using Data.Repositories;
using Entities;
using System.Collections.Generic;
using System.Linq;

namespace Services.DataInitializer
{
    public class CategoryDataInitializer : IDataInitializer
    {
        private readonly IRepository<Category> repository;

        public CategoryDataInitializer(IRepository<Category> repository)
        {
            this.repository = repository;
        }

        public void InitializeData()
        {

            if (repository.TableNoTracking.Any()) return;

            repository.AddRange(new List<Category> {
                new Category { Name = "Test Category 1" },
                new Category { Name = "Test Category 2" }
            });
        }
    }
}
