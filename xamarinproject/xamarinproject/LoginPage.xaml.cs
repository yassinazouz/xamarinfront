using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamarinproject
{
    public partial class LoginPage : ContentPage
    {
        private readonly UserService _userService;

        public LoginPage()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = usernameEntry.Text;
            string password = passwordEntry.Text;

            var loginResult = await _userService.LoginAsync(username, password);

            if (loginResult != null && loginResult.UserId > 0)
            {
                long userId = loginResult.UserId;
                await Navigation.PushAsync(new MainPage(userId));
            }
            else
            {
                await DisplayAlert("Login Failed", "Invalid credentials", "OK");
            }
        }


        private async void OnSignupClicked(object sender, EventArgs e)
        {
            // Navigate to the signup page
            await Navigation.PushAsync(new SignupPage());
        }
    }
}
