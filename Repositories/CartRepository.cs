using OnlineStore.Data;
using OnlineStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Security.Claims;
using GemStore.Interfaces;
using GemStore.ViewModels;

namespace GemStore.Repositories
{
    public class CartRepository : ICartRepository<CartViewModel>
    {
        private readonly ApplicationDatabaseContext databaseContext;

        public CartRepository(ApplicationDatabaseContext databaseContext) {
            this.databaseContext = databaseContext;
        }

        public async Task Add(IEnumerable<CartViewModel> items, int userId)
        {
            /*
             Map the data from CartViewModel into CartModel
             */

            var model = new List<CartModel>();

            foreach (var item in items) 
            {
                model.Add(new CartModel {
                    ProductId = item.ProductId,
                    ProductImage = item.ProductImage,
                    ProductName = item.ProductName,
                    ProductPrice = item.ProductPrice,
                    UserId = userId,
                    Quantity = item.Quantity,
                });
            }

            /*
             Check if the product in the cart exist in the database.
             If exist, update the database, else add into the database.
             */

            foreach (var item in model)
            {
                var getItem = await databaseContext.Cart.FirstOrDefaultAsync(i => i.ProductId == item.ProductId);

                if (getItem != null)
                {
                    getItem.Quantity = item.Quantity;
                }
                else
                {
                    await databaseContext.Cart.AddAsync(item);    
                }
            }

            await databaseContext.SaveChangesAsync();
        }

        public List<CartViewModel> Get(HttpContext httpContext)
        {
            List<CartViewModel> cart = new();

            byte[] bytes = httpContext.Session.Get("UserShoppingCart");

            if (bytes != null) {
                string encoded = Encoding.UTF8.GetString(bytes);
                cart = JsonSerializer.Deserialize<List<CartViewModel>>(encoded);
            }

            return cart;
        }

        public void Delete(CartViewModel item)
        {
            var getItem = databaseContext.Cart.Find(item.Id);

            if (getItem != null) {
                databaseContext.Cart.Remove(getItem);
                databaseContext.SaveChanges();
            }
        }

        public async Task<IEnumerable<CartViewModel>> GetById(int userId)
        {
            return await databaseContext.Cart
                .Where(i => i.UserId == userId)
                .Select(i => new CartViewModel {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductImage = i.ProductImage,
                    ProductName = i.ProductName,
                    ProductPrice = i.ProductPrice,
                    Quantity = i.Quantity,
                }).ToListAsync();

            throw new NotImplementedException();
        }

        public async Task Save(HttpContext httpContext, ClaimsIdentity claimsIdentity)
        {
            byte[] cart = httpContext.Session.Get("UserShoppingCart");

            if (cart != null) {
                string encoded = Encoding.UTF8.GetString(cart);
                var deserialized = JsonSerializer.Deserialize<IEnumerable<CartViewModel>>(encoded);
                var userId = Int32.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await Add(deserialized, userId);
                httpContext.Session.Remove("UserShoppingCart");
            }
        }

        public async Task Set(HttpContext httpContext, IEnumerable<CartViewModel> cartInfo) {
            var toString = JsonSerializer.Serialize(cartInfo);
            httpContext.Session.Set("UserShoppingCart", Encoding.UTF8.GetBytes(toString));
        }

        public void Insert(HttpContext httpContext, ProductViewModel productInfo)
        {
            byte[] cart = httpContext.Session.Get("UserShoppingCart");
            
            if (cart != null) {
                string encoded = Encoding.UTF8.GetString(cart);
                var deserialized = JsonSerializer.Deserialize<List<CartViewModel>>(encoded);
                var product = deserialized.FirstOrDefault(i => i.ProductId == productInfo.Id);

                if (product != null)
                {
                    product.Quantity++;
                }
                else {
                    deserialized.Add(new CartViewModel {
                        ProductId = productInfo.Id,
                        ProductImage = productInfo.Image,
                        ProductName = productInfo.Title,
                        ProductPrice = productInfo.Price,
                        Quantity = 1
                    });
                }

                var serialize = JsonSerializer.Serialize(deserialized);
                httpContext.Session.Set("UserShoppingCart", Encoding.UTF8.GetBytes(serialize));
            }
        }

        public void Remove(HttpContext httpContext, CartViewModel cartInfo)
        {
            byte[] cart = httpContext.Session.Get("UserShoppingCart");

            if (cart != null) {
                string encoded = Encoding.UTF8.GetString(cart);
                var deserialize = JsonSerializer.Deserialize<List<CartViewModel>>(encoded);
                var product = deserialize.FirstOrDefault(i => i.Id == cartInfo.Id);

                if (product != null) {
                    deserialize.Remove(product);
                }

                var serialize = JsonSerializer.Serialize(deserialize);
                httpContext.Session.Set("UserShoppingCart", Encoding.UTF8.GetBytes(serialize));
            }

            Delete(cartInfo);
        }
    }
}
