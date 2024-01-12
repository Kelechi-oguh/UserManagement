using Newtonsoft.Json;
using UserManagement.Models;

namespace UserManagement.Services.ProductService
{
    public class ProductService
    {
        public ProductService()
        {
            
        }

        public async Task<Product> GetProduct(int productId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://dummyjson.com/products/{productId}");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            if (content == null)
                return null;

            var myDeserializedClass = JsonConvert.DeserializeObject<Product>(content);

            return myDeserializedClass;
        }

        public async Task<AllProducts> GetAllProducts()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://dummyjson.com/products");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            if (content == null)
                return null;

            var myDeserializedClass = JsonConvert.DeserializeObject<AllProducts>(content);

            return myDeserializedClass;
        }
    }
}
