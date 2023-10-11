using GemStore.Interfaces;

namespace GemStore.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly HttpClient httpClient;

        public CategoryRepository(HttpClient httpClient) {
            this.httpClient = httpClient;
            this.httpClient.BaseAddress = new Uri("https://fakestoreapi.com");
        }

        public async Task<IEnumerable<string>> GetAll()
        {
            HttpResponseMessage response = await httpClient.GetAsync("/products/categories");

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<IEnumerable<string>>();
            }

            throw new NotImplementedException();
        }
    }
}
