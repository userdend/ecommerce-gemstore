using GemStore.Interfaces;
using GemStore.ViewModels;

namespace GemStore.Repositories
{
    public class ProductRepository : IProductRepository<ProductViewModel>
    {
        private readonly HttpClient httpClient;

        public ProductRepository(HttpClient httpClient) {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri("https://fakestoreapi.com");
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllAsync()
        {
            HttpResponseMessage response = await httpClient.GetAsync("/products");
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<IEnumerable<ProductViewModel>>();
            }
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductViewModel>> GetByCategoryAsync(string category)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/products/category/{category}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<ProductViewModel>>();
            }
            throw new NotImplementedException();
        }

        public async Task<ProductViewModel> GetByIdAsync(int productId)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"/products/{productId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<ProductViewModel>();
            }
            throw new NotImplementedException();
        }
    }
}
