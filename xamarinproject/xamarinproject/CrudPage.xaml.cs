using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamarinproject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrudPage : ContentPage
    {
        private readonly ProductService _productService;

        public CrudPage()
        {
            InitializeComponent();
            _productService = new ProductService();
            LoadProducts();
        }

        private async void LoadProducts()
        {
            var products = await _productService.GetProductsAsync();
            crudProductListView.ItemsSource = products;
        }

        private async void OnAddProductClicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Add Product", "Enter product name:");
            if (string.IsNullOrWhiteSpace(name)) return;

            string priceInput = await DisplayPromptAsync("Add Product", "Enter product price:");
            if (!decimal.TryParse(priceInput, out decimal price)) return;

            var product = new Product { Name = name, Price = (double)price };
            await _productService.AddProductAsync(product);

            LoadProducts();
        }

        private async void OnEditProductClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var product = button?.CommandParameter as Product;

            if (product != null)
            {
                string name = await DisplayPromptAsync("Edit Product", "Enter new name:", initialValue: product.Name);
                string priceInput = await DisplayPromptAsync("Edit Product", "Enter new price:", initialValue: product.Price.ToString());

                if (!decimal.TryParse(priceInput, out decimal price)) return;

                product.Name = name;
                product.Price = (double)price;

                await _productService.UpdateProductAsync(product.Id, product);
                LoadProducts();
            }
        }

        private async void OnDeleteProductClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var product = button?.CommandParameter as Product;

            if (product != null)
            {
                bool confirm = await DisplayAlert("Delete Product", $"Are you sure you want to delete {product.Name}?", "Yes", "No");
                if (confirm)
                {
                    await _productService.DeleteProductAsync(product.Id);
                    LoadProducts();
                }
            }
        }
    }

}