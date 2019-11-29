namespace PetStore.Services
{
    public interface IUserService
    {
        void Register(string name, string email);

        bool IsExist(int userId);
    }
}
