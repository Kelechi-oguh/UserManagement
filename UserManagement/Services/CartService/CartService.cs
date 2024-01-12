using Newtonsoft.Json;
using UserManagement.Models;

namespace UserManagement.Services.CartService
{
    public class CartService
    {
        public CartService()
        {
            
        }

        public async Task<CartRoot> GetAllCarts()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://dummyjson.com/carts");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();

            if (content == null)
                return null;

            var myDeserializedClass = JsonConvert.DeserializeObject<CartRoot>(content);
            return myDeserializedClass;
        }

        public async Task<Cart> GetCartById(int cartId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://dummyjson.com/carts/{cartId}");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            if (content == null)
                return null;

            var myDeserializedClass = JsonConvert.DeserializeObject<Cart>(content);
            return myDeserializedClass;
        }
    }
}
