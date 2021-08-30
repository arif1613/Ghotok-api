namespace QQuery.Helper
{
    public interface IRepositoryHelper<TEntity> where TEntity : class
    {
        //IQueryable<TEntity> GetAuditProjectQuery();
        //IQueryable<TEntity> SegmentFilter(IQueryable<TEntity> query, IEnumerable<int> segments);
        //IQueryable<TEntity> OfficeUnitQuery(IQueryable<TEntity> query, IEnumerable<KeyValuePair<int, string>> roleOfficeUnits);
        //IQueryable<TEntity> FilterQuery(IQueryable<TEntity> query, IEnumerable<KeyValuePair<AuditFilter, string>> filters);
        //IQueryable<TEntity> TeamMemberRoleFilter(IQueryable<TEntity> query, IEnumerable<KeyValuePair<AuditFilter, string>> filterCriteria);
        //IEnumerable<Expression<Func<TEntity, bool>>> SearchQuery(IQueryable<TEntity> query, IEnumerable<KeyValuePair<AuditSearchCriteria, string>> filters);
        //IQueryable<TEntity> OrderQuery(IQueryable<TEntity> query, IEnumerable<AuditOrder> orders);
        //IQueryable<TEntity> GroupJoin(IEnumerable<TEntity> materializedlist);
    }
}