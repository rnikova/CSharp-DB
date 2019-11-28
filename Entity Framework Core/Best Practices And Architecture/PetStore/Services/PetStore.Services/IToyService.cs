using PetStore.Services.Models.Toy;

namespace PetStore.Services
{
    public interface IToyService
    {
        void ByToyByDistributor(string name, string description, decimal distributorPrice, decimal profit, int brandId, int categoryId);

        void ByToyByDistributor(AddingToyServiceModel model);
    }
}
