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
        // a const for helping us know whether or not the app has been wired to an Azure App Service.
		const string UNCONFIGURED_URL = "https://CONFIGURE-THIS-URL.azurewebsites.net";

		//  a flag indicating whether or not the app has been wired up to an Azure App Service instance.
        public static bool AzureNeedsSetup => AzureMobileAppUrl == UNCONFIGURED_URL;

		// The Azure App Service URL.
		// MUST use HTTPS, neglecting to do so will result in runtime errors on iOS.
        public static string AzureMobileAppUrl = UNCONFIGURED_URL; // Replace UNCONFIGURED_URL with your url, i.e. "https://my-service.azurewebsites.net".

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
