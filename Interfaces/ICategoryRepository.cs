namespace GemStore.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<string>> GetAll();
    }
}
