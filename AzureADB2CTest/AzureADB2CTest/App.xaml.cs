using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using azure_ad_test.Services;
using azure_ad_test.Views;
using Microsoft.Identity.Client;

namespace azure_ad_test
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address
        public static string AzureBackendUrl =
            DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
        public static bool UseMockDataStore = true;

        public static IPublicClientApplication AuthenticationClient { get; private set; }
        public static object UIParent { get; set; } = null;

        public App()
        {
            InitializeComponent();

            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<AzureDataStore>();
           

            AuthenticationClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
                .WithIosKeychainSecurityGroup(Constants.IosKeychainSecurityGroups)
                .WithB2CAuthority(Constants.AuthoritySignin)
                .WithRedirectUri($"msal{Constants.ClientId}://auth")
                .Build();

            MainPage = new MainPage();


        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
    public static class Constants
    {
        // set your tenant name, for example "contoso123tenant"
        static readonly string tenantName = "";

        // set your tenant id, for example: "contoso123tenant.onmicrosoft.com"
        static readonly string tenantId = "";

        // set your client/application id, for example: aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"
        static readonly string clientId = "";

        // set your sign up/in policy name, for example: "B2C_1_signupsignin"
        static readonly string policySignin = "";

        // set your forgot password policy, for example: "B2C_1_passwordreset"
        static readonly string policyPassword = "";

        // set to a unique value for your app, such as your bundle identifier. Used on iOS to share keychain access.
        static readonly string iosKeychainSecurityGroup = "";



        // The following fields and properties should not need to be changed
        static readonly string[] scopes = { "" };
        static readonly string authorityBase = $"https://{tenantName}.b2clogin.com/tfp/{tenantId}/";
        public static string ClientId
        {
            get
            {
                return clientId;
            }
        }
        public static string AuthoritySignin
        {
            get
            {
                return $"{authorityBase}{policySignin}";
            }
        }
        public static string AuthorityPasswordReset
        {
            get
            {
                return $"{authorityBase}{policyPassword}";
            }
        }
        public static string[] Scopes
        {
            get
            {
                return scopes;
            }
        }
        public static string IosKeychainSecurityGroups
        {
            get
            {
                return iosKeychainSecurityGroup;
            }
        }
    }
}
