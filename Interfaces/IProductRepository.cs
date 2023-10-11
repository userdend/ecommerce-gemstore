namespace GemStore.Interfaces
{
    public interface IProductRepository<TEntity>
    {
        Task<TEntity> GetByIdAsync(int productId);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetByCategoryAsync(string category);
    }
}
