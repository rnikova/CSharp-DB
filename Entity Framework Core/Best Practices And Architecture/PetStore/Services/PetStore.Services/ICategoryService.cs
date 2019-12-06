using PetStore.Services.Models.Category;
using System.Collections.Generic;

namespace PetStore.Services
{
    public interface ICategoryService
    {
        void Create(CreateCategoryServiceModel model);

        bool IsExist(int categoryId);

        IEnumerable<AllCategoriesServiceModel> All();
    }
}
