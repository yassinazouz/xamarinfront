using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace xamarinproject
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://192.168.1.13:8081/api/users/"); // Change to your backend's URL
        }

        // The LoginResult class to hold the response
        public class LoginResult
        {
            public long UserId { get; set; }
            public string Message { get; set; }
        }

        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            var loginData = new { username, password };
            var jsonContent = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("login", content);

            string responseData = await response.Content.ReadAsStringAsync(); // Get raw response

            Console.WriteLine("Response: " + responseData); // Log or display the response content

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    // Deserialize the response into LoginResponse object
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseData);
                    return new LoginResult
                    {
                        UserId = loginResponse.UserId,
                        Message = loginResponse.Message
                    };
                }
                catch (JsonException ex)
                {
                    Console.WriteLine("JSON Parsing Error: " + ex.Message);
                    return new LoginResult { UserId = -1, Message = "Invalid JSON response" };
                }
            }
            else
            {
                return new LoginResult
                {
                    UserId = -1,  // You can set an invalid userId in case of failure
                    Message = "Invalid username or password"
                };
            }

        }


        public async Task<string> SignupAsync(string username, string email, string password)
        {
            var signupData = new { username, email, password };
            var jsonContent = JsonConvert.SerializeObject(signupData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("signup", content);

            if (response.IsSuccessStatusCode)
            {
                return "Signup successful";
            }
            else
            {
                return "Signup failed";
            }
        }
    }
}
public class LoginResponse
{
    [JsonProperty("userId")]
    public long UserId { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }
}

public class LoginResult
{
    public long UserId { get; set; }
    public string Message { get; set; }
}
