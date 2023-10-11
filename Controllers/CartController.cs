using GemStore.Interfaces;
using GemStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository<CartViewModel> cartRepository;

        public CartController(ICartRepository<CartViewModel> cartRepository) {
            this.cartRepository = cartRepository;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View(cartRepository.Get(HttpContext));
        }

        [HttpPost]
        public ActionResult AddIntoCart(ProductViewModel productInfo) {
            cartRepository.Insert(HttpContext, productInfo);
            return NoContent();
        }

        [HttpPost]
        public ActionResult RemoveFromCart(CartViewModel obj) {
            cartRepository.Remove(HttpContext, obj);
            return RedirectToAction("Index");
        }
    }
}
