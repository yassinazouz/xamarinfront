using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace xamarinproject
{
    public class CartService
    {
        private readonly HttpClient _httpClient;

        public CartService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://192.168.1.13:8081/"); // Your Spring Boot API base URL
        }

        // Add a product to the cart via the API
        public async Task AddToCartAsync(CartRequest cartRequest)

        {
            // Retrieve userId from SecureStorage
            string userIdString = await SecureStorage.GetAsync("userId");
            if (string.IsNullOrEmpty(userIdString))
            {
                Console.WriteLine("User ID is not found in SecureStorage");
                return;
            }

            // Convert userId to long
            long userId = long.Parse(userIdString);


            var content = new StringContent(JsonConvert.SerializeObject(cartRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("cart/add", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Product added to cart successfully.");
            }
            else
            {
                Console.WriteLine("Failed to add product to cart.");
            }
        }

        // Get cart items for the currently logged-in user
        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            // Retrieve userId from SecureStorage
            string userIdString = await SecureStorage.GetAsync("userId");
            if (string.IsNullOrEmpty(userIdString))
            {
                Console.WriteLine("User ID is not found in SecureStorage");
                return null;
            }

            // Convert userId to long
            long userId = long.Parse(userIdString);

            // Retrieve cart items using the userId
            var response = await _httpClient.GetStringAsync($"cart/{userId}");
            return JsonConvert.DeserializeObject<List<CartItem>>(response);
        }
    }
}
