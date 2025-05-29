using System.Reflection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PMLERP.Helpers;
using PMLERP.IServices;
using PMLERP.IServices.Master;
using PMLERP.Services;
using PMLERP.Services.Master;
using Syncfusion.Blazor;

namespace PMLERP
{
    public static class MauiProgram
    {
         
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            var getAssembly = Assembly.GetExecutingAssembly();
            var resourceName = "PMLERP.appsettings.json";
            using var stream = getAssembly.GetManifestResourceStream(resourceName) ??
                                throw new FileNotFoundException($"Resource '{resourceName}' not found in assembly '{getAssembly.FullName}'.");
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();

            // Register configuration as a service
            builder.Services.AddSingleton<IConfiguration>(config);

            // Initialize global access
            AppConfig.Init(config);

            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Playfair-VariableFont_opsz-wdth-wght.ttf", "Playfair Display");
                    fonts.AddFont("Playfair-Italic-VariableFont_opsz-wdth-wght.ttf", "Playfair Display Italic");
                    fonts.AddFont("Montserrat-VariableFont_wght.ttf", "Montserrat");
                    fonts.AddFont("Montserrat-Italic-VariableFont_wght.ttf", "MontserratItalic");
                });

            builder.Services.AddMauiBlazorWebView();

            //Add syncfusion Blazor
            builder.Services.AddSyncfusionBlazor();

            // Add authentication support
            builder.Services.AddAuthorizationCore();

            // Register secure storage
            //builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // Register HTTP client
            var baseUrl = DeviceInfo.Platform == DevicePlatform.Android
                ? AppConfig.Configuration?["API:Android_BaseUrl"]
                    : AppConfig.Configuration?["API:BaseUrl"];
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            });

            // Register authentication services (ORDER MATTERS)
            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
                provider.GetRequiredService<CustomAuthStateProvider>());

            // Register auth service (removed duplicate)
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Register toast service
            builder.Services.AddScoped<IToastService, ToastService>();

            return builder.Build();
        }
    }
}