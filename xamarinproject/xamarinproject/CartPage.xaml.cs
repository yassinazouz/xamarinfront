using Xamarin.Forms;
using System.Collections.Generic;
using System;

namespace xamarinproject
{
    public partial class CartPage : ContentPage
    {
        public CartPage(List<CartItem> cartItems)
        {
            InitializeComponent();
            cartListView.ItemsSource = cartItems;
        }

        // Back to the product list page
        private async void OnBackToProductsClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

}
