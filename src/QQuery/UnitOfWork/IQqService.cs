using QQuery.Repo;

namespace QQuery.UnitOfWork
{
    public interface IQqService<TEntity> where  TEntity:class
    {
        IQuickQueryRepository<TEntity> QqRepository { get; }
        void Commit();
    }
}