using System.Collections.Generic;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace xamarinproject
{
    public partial class MainPage : ContentPage
    {
        private readonly ProductService _productService;
        private readonly CartService _cartService;
        private readonly long _userId; // Assuming you have a way to track the logged-in user ID

        public MainPage(long userId)
        {
            InitializeComponent();
            _productService = new ProductService();
            _cartService = new CartService();
            _userId = userId;
            LoadProducts();
        }

        private async void LoadProducts()
        {
            try
            {
                List<Product> products = await _productService.GetProductsAsync(); // Ensure you use await

                foreach (var product in products)
                {
                    if (string.IsNullOrEmpty(product.ImageUrl))
                    {
                        product.ImageUrl = "https://via.placeholder.com/150"; // Default placeholder image
                    }
                }

                productListView.ItemsSource = products;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadProducts: {ex.Message}");
                await DisplayAlert("Error", "Could not load products.", "OK");
            }
        }

        // Handle Add to Cart button click
        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            try
            {
                // Get the button and its bound product
                var button = sender as Button;
                var product = button?.BindingContext as Product;

                if (product == null)
                {
                    await DisplayAlert("Error", "Product not found.", "OK");
                    return;
                }

                // Find the quantity entry within the button's parent layout
                var stackLayout = button.Parent as StackLayout;
                var quantityEntry = stackLayout?.FindByName<Entry>("quantityEntry");

                // Validate quantity input
                if (quantityEntry == null || string.IsNullOrEmpty(quantityEntry.Text) || !int.TryParse(quantityEntry.Text, out int quantity) || quantity <= 0)
                {
                    await DisplayAlert("Error", "Please enter a valid quantity.", "OK");
                    return;
                }

                // Prepare the cart request
                var userId = await SecureStorage.GetAsync("userId");
                if (string.IsNullOrEmpty(userId))
                {
                    await DisplayAlert("Error", "User not authenticated.", "OK");
                    return;
                }

                var cartRequest = new CartRequest
                {
                    UserId = long.Parse(userId),
                    ProductId = product.Id,
                    Quantity = quantity
                };

                // Add product to the cart
                await _cartService.AddToCartAsync(cartRequest);
                await DisplayAlert("Success", $"{product.Name} has been added to your cart.", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnAddToCartClicked: {ex.Message}");
                await DisplayAlert("Error", "Unable to add the product to the cart.", "OK");
            }
        }


        // View Cart button click
        private async void OnViewCartClicked(object sender, EventArgs e)
        {
            var cartItems = await _cartService.GetCartItemsAsync();

            if (cartItems != null && cartItems.Count > 0)
            {
                var cartPage = new CartPage(cartItems);
                await Navigation.PushAsync(cartPage);
            }
            else
            {
                await DisplayAlert("Cart", "Your cart is empty.", "OK");
            }
        }
        private async void OnCrudLogoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CrudPage());
        }
    }
}
