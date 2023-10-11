using GemStore.Interfaces;
using GemStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace OnlineStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository<ProductViewModel> productRepository;

        public HomeController(ILogger<HomeController> logger, 
            ICategoryRepository categoryRepository,
            IProductRepository<ProductViewModel> productRepository)
        {
            _logger = logger;
            this.categoryRepository = categoryRepository;
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            try {
                var data = await productRepository.GetAllAsync();
                ViewBag.ListOfCategory = await categoryRepository.GetAll();
                return View(data);
            } 
            
            catch (Exception) {
                return RedirectToAction("Error");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/category/{item}")]
        public async Task<IActionResult> ProductBasedOnCategory(string item) {
            try { 
                var data = await productRepository.GetByCategoryAsync(item);
                return View(data);
            }

            catch (Exception) {
                return RedirectToAction("Error");
            }
        }

        [Route("/product/{item}-{id}")]
        public async Task<IActionResult> SingleProduct(int id)
        {
            try
            {
                var data = await productRepository.GetByIdAsync(id);

                ClaimsPrincipal claims = HttpContext.User;
                TempData["Authenticated"] = claims.Identity.IsAuthenticated;

                return View(data);
            }

            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}