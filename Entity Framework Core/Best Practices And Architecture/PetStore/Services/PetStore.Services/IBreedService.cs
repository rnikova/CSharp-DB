namespace PetStore.Services
{
    public interface IBreedService
    {
        void Add(string name);

        bool IsExist(int breedId);
    }
}
