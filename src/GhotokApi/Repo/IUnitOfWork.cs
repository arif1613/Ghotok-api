using Ghotok.Data.DataModels;

namespace GhotokApi.Repo
{
    public interface IUnitOfWork
    {
        IRepository<AppUser> AppUseRepository { get; }
        IRepository<User> UserRepository { get; }
        void Commit();
    }
}