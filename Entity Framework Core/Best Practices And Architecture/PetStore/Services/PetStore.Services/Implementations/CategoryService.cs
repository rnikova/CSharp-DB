using PetStore.Data;
using PetStore.Data.Models;
using PetStore.Services.Models.Category;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly PetStoreDbContext data;

        public CategoryService(PetStoreDbContext data)
        {
            this.data = data;
        }

        public void Create(CreateCategoryServiceModel model)
        {
            var category = new Category
            {
                Name = model.Name,
                Description = model.Description
            };

            this.data.Categories.Add(category);
            this.data.SaveChanges();
        }

        public IEnumerable<AllCategoriesServiceModel> All()
        {
            return this.data.Categories
                .Select(c => new AllCategoriesServiceModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToArray();
        }

        public bool IsExist(int categoryId)
        {
            return this.data.Categories.Any(c => c.Id == categoryId);
        }

        public DetailsCategoryServiceModel GetById(int id)
        {
            var category = this.data.Categories.Find(id);

            var detailServiceModel = new DetailsCategoryServiceModel
            {
                Id = category?.Id,
                Name = category?.Name,
                Description = category?.Description
            };

            return detailServiceModel;
        }

        public void Edit(EditCategoryServiceModel model)
        {
            var category = this.data.Categories.Find(model.Id);

            category.Name = model.Name;
            category.Description = model.Decription;

            this.data.SaveChanges();
        }
    }
}
