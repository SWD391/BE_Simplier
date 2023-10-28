public interface IRepository<T> where T : class
{
    public Task<T?> GetByIdAsync(string id);
    public Task<List<T>> GetAllAsync();
    public Task DeleteAsync(string id);
    public Task CreateAsync(T entity);
    public Task<int> Count();
    public Task UpdateAsync(T entity);
    public Task<List<T>> GetPaginationAsync(int pageNumber, int pageSize);
    public Task<List<T>> GetPaginationWithSearchKeyAsync(string searchKey, int pageNumber, int pageSize);
}