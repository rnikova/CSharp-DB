using PetStore.Services.Models.Category;
using System.Collections.Generic;

namespace PetStore.Services
{
    public interface ICategoryService
    {
        void Create(CreateCategoryServiceModel model);

        void Edit(EditCategoryServiceModel model);

        bool Remove(int id);

        bool IsExist(int categoryId);

        IEnumerable<AllCategoriesServiceModel> All();

        DetailsCategoryServiceModel GetById(int id);
    }
}
