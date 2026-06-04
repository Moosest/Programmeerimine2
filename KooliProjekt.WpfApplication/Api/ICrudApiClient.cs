namespace KooliProjekt.WpfApplication
{
    public interface ICrudApiClient<TEntity>
    {
        Task<OperationResult<PagedResult<TEntity>>> List(int page, int pageSize);
        Task<OperationResult> Save(TEntity entity);
        Task<OperationResult> Delete(int id);
    }
}
