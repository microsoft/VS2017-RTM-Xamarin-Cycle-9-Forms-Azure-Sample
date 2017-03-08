using System.Collections.Generic;
using MyItems.Helpers;
using MyItems.Services;
using MyItems.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MyItems
{
    public partial class App : Application
    {
        // Replace the empty string with your Azure App Service URL, like: http://my-service.azurewebsites.net
        // MUST be a https URL, otherwise iOS will throw errors.
        public static string AzureMobileAppUrl = "";

        //  A flag indicating whether or not the app has been wired up to an Azure App Service instance.
        public static bool AzureNeedsSetup => string.IsNullOrWhiteSpace(AzureMobileAppUrl);

        public static IDictionary<string, string> LoginParameters => null;

        public App()
        {
            InitializeComponent();

			// If no Azure App Service url has been set, then use a local mock data store.
            if (AzureNeedsSetup)
                DependencyService.Register<MockDataStore>(); // This will use a local mock data store, and will not be synced to Azure.
            else
                DependencyService.Register<AzureDataStore>(); // This will use a synced Azure data store.

            SetMainPage();
        }

        public static void SetMainPage()
        {
            if (!AzureNeedsSetup && !Settings.IsLoggedIn && AzureDataStore.UseAuthentication)
            {
                Current.MainPage = new NavigationPage(new LoginPage())
                {
                    BarBackgroundColor = (Color)Current.Resources["Primary"],
                    BarTextColor = Color.White
                };
            }
            else
            {
                GoToMainPage();
            }
        }

        public static void GoToMainPage()
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Current.MainPage = new TabbedPage
                {
                    Children =
                    {
                        new NavigationPage(new ItemsPage())
                        {
                            Title = "Browse",
                            Icon = Device.OnPlatform("tab_feed.png",null,null)
                        },
                        new NavigationPage(new AboutPage())
                        {
                            Title = "About",
                            Icon = Device.OnPlatform("tab_about.png",null,null)
                        },
                    }
                };
            }
            else
            {
                Current.MainPage = new NavigationPage(new TabbedPage
                {
                    Title = "My Items",
                    Children =
                    {
                        new ItemsPage()
                        {
                            Title = "Browse",
                            Icon = Device.OnPlatform("tab_feed.png",null,null)
                        },
                        new AboutPage()
                        {
                            Title = "About",
                            Icon = Device.OnPlatform("tab_about.png",null,null)
                        },
                    }
                });
            }


        }
    }
}
