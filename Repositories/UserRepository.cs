using GemStore.Interfaces;
using GemStore.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;
using System.Security.Claims;

namespace GemStore.Repositories
{
    public class UserRepository : IUserRepository<UserModel>
    {
        private readonly ApplicationDatabaseContext databaseContext;

        public UserRepository(ApplicationDatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<bool> Add(RegisterViewModel obj)
        {
            var findUser = await databaseContext.User.FirstOrDefaultAsync(i => i.Name == obj.Name);

            if (findUser == null)
            {
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(obj.Password);
                var newUser = new UserModel() {
                    Name = obj.Name,
                    Password = hashPassword,
                };
                await databaseContext.User.AddAsync(newUser);
                await databaseContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<UserModel> FindByName(LoginViewModel loginInfo)
        {
            var userInfo = await databaseContext.User.SingleOrDefaultAsync(i => i.Name == loginInfo.Name);

            if (userInfo != null) {
                bool validPassword = BCrypt.Net.BCrypt.Verify(loginInfo.Password, userInfo.Password);
            }

            return userInfo;
        }

        public async Task Authentication(HttpContext httpContext, LoginViewModel loginInfo, UserModel userInfo)
        {
            List<Claim> claims = new() 
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
                new Claim(ClaimTypes.Name, loginInfo.Name)
            };

            ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new() 
            { 
                AllowRefresh = true,
                IsPersistent = loginInfo.RememberMe
            };

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                properties);
        }

        public async Task SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
