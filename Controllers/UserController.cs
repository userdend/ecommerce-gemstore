using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data;
using OnlineStore.Models;
using GemStore.Interfaces;
using GemStore.ViewModels;

namespace OnlineStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository<UserModel> userRepository;
        private readonly ICartRepository<CartViewModel> cartRepository;

        public UserController(
            IUserRepository<UserModel> userRepository,
            ICartRepository<CartViewModel> cartRepository) {
            this.userRepository = userRepository;
            this.cartRepository = cartRepository;
        }

        [Route("user/login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("user/login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginInfo)
        {
            if (ModelState.IsValid) {
                try {
                    var userInfo = await userRepository.FindByName(loginInfo);

                    if (userInfo != null)
                    {
                        // Authenticate successfully logged in user.
                        await userRepository.Authentication(HttpContext, loginInfo, userInfo);

                        /*
                         Retrieve the user's cart from the database.
                         Map the response into the cart view-model.
                         Finally save it into the session.
                         */
                        await cartRepository.Set(HttpContext, await cartRepository.GetById(userInfo.Id));

                        return RedirectToAction("Index", "Home");
                    }
                }

                catch (Exception) {
                    throw new NotImplementedException();
                }
            }

            TempData["Message"] = "Name or Password is incorrect.";
            return View(loginInfo);
        }

        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel user)
        {
            if (ModelState.IsValid) {
                try {
                    bool register = await userRepository.Add(user);
                    if (register)
                    {
                        TempData["Success"] = "Successfully registered.";
                    }
                    else
                    {
                        TempData["Failed"] = "Details already in used.";
                    }
                }

                catch (Exception) {
                    throw new NotImplementedException();
                }
            }

            return View();
        }

        public async Task<IActionResult> Logout() {
            await cartRepository.Save(HttpContext, User.Identity as ClaimsIdentity);
            await userRepository.SignOut(HttpContext);

            return RedirectToAction("Index", "Home");
        }
    }
}
