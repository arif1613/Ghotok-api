using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;

namespace Ghotok.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<AppUser> AppUseRepository { get; }
        IRepository<User> UserRepository { get; }
        void Commit();
    }
}