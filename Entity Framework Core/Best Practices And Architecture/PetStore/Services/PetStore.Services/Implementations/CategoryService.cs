using PetStore.Data;
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

        public bool IsExist(int categoryId)
        {
            return this.data.Categories.Any(c => c.Id == categoryId);
        }
    }
}
