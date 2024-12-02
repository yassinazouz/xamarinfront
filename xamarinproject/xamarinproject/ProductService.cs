using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace xamarinproject
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://192.168.1.13:8081/"); // Ensure this is correct
        }

        // Fetch all products
        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("products");
                Console.WriteLine($"API Response: {response}");
                return JsonConvert.DeserializeObject<List<Product>>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products: {ex.Message}");
                return new List<Product>();
            }
        }

        // Fetch a single product by ID
        public async Task<Product> GetProductByIdAsync(long id)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"products/{id}");
                return JsonConvert.DeserializeObject<Product>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching product by ID: {ex.Message}");
                return null;
            }
        }

        // Add a new product
        public async Task<Product> AddProductAsync(Product product)
        {
            try
            {
                var json = JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("products", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Product>(responseBody);
                }
                else
                {
                    Console.WriteLine($"Error adding product: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
                return null;
            }
        }

        // Update an existing product
        public async Task<Product> UpdateProductAsync(long id, Product product)
        {
            try
            {
                var json = JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"products/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Product>(responseBody);
                }
                else
                {
                    Console.WriteLine($"Error updating product: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                return null;
            }
        }

        // Delete a product
        public async Task<bool> DeleteProductAsync(long id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"products/{id}");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Product with ID {id} deleted successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error deleting product: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return false;
            }
        }
    }
}
