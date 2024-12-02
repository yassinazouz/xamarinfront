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
    public partial class SignupPage : ContentPage
    {
        private readonly UserService _userService;
        public SignupPage()
        {
            InitializeComponent();
            _userService = new UserService();
        }
        private async void OnSignupClicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string email = emailEntry.Text;
            string password = passwordEntry.Text;

            var result = await _userService.SignupAsync(username, email, password);

            await DisplayAlert("Signup Result", result, "OK");

            if (result == "Signup successful")
            {
                // Navigate to the login page after successful signup
                await Navigation.PopAsync();  // Go back to the login page
            }
        }
    }
}