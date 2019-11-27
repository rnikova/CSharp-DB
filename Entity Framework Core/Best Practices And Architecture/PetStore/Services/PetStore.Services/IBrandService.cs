namespace PetStore.Services
{
    public interface IBrandService
    {
        void Create();

        IEnumerable<> SearchByName(string name);
    }
}
