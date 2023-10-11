using GemStore.ViewModels;
using System.Security.Claims;

namespace GemStore.Interfaces
{
    public interface ICartRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetById(int userId);
        Task Add(IEnumerable<TEntity> items, int userId);
        void Delete(TEntity item);
        Task Set(HttpContext httpContext, IEnumerable<CartViewModel> cartInfo);
        Task Save(HttpContext httpContext, ClaimsIdentity claimsIdentity);
        List<CartViewModel> Get(HttpContext httpContext);
        void Insert(HttpContext httpContext, ProductViewModel productInfo);
        void Remove(HttpContext httpContext, CartViewModel cartInfo);
    }
}
