using GemStore.ViewModels;
using OnlineStore.Models;

namespace GemStore.Interfaces
{
    public interface IUserRepository<TEntity>
    {
        Task<TEntity> FindByName(LoginViewModel obj);
        Task<bool> Add(RegisterViewModel obj);
        Task Authentication(HttpContext httpContext, LoginViewModel loginInfo, UserModel userInfo);
        Task SignOut(HttpContext httpContext);
    }
}
