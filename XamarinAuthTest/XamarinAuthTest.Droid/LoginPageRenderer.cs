using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using XamarinAuthTest;
using Xamarin.Auth;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

[assembly: ExportRenderer(typeof(Login), typeof(XamarinAuthTest.Droid.LoginPageRenderer))]
namespace XamarinAuthTest.Droid
{
    public class User
    {
        public string Sub { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }

    public class LoginPageRenderer : PageRenderer
    {
        public LoginPageRenderer()
        {
            var activity = this.Context as Activity;

            var auth = new OAuth2Authenticator(
                "innplusxamarin",
                "xamarinsecret",
                "openid profile roles",
                new Uri("https://innplus-identityserver-test.azurewebsites.net/identity/connect/authorize"),
                new Uri("https://innplus-identityserver-test.azurewebsites.net/identity"),
                new Uri("https://innplus-identityserver-test.azurewebsites.net/identity/connect/token"));

            auth.Completed += async (sender, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    var accessToken = eventArgs.Account.Properties["access_token"].ToString();
                    // var name = eventArgs.Account.Properties["name"].ToString();

                    User user = null;
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://innplus-identityserver-test.azurewebsites.net");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        var response = await client.GetAsync("/identity/connect/userinfo");

                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            user = JsonConvert.DeserializeObject<User>(json);
                        }
                    }

                    await App.NavigateToProfile(string.Format("Olá, {0}", user.Name), user.Role);
                }
                else
                {
                    await App.NavigateToProfile("Usuário cancelou o login", null);
                }
            };

            activity.StartActivity(auth.GetUI(activity));
        }
    }
}