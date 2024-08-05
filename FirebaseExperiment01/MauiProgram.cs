using CommunityToolkit.Maui;
using Plugin.Firebase.Auth;
#if IOS
using Plugin.Firebase.Core.Platforms.iOS;
#elif ANDROID
//using Plugin.Firebase.Core.Platforms.Android;
using Plugin.Firebase.Bundled.Platforms.Android;
using Plugin.Firebase.Auth.Google;
#endif
using Plugin.Firebase.Crashlytics;


using FirebaseExperiment01.Helpers;
using FirebaseExperiment01.Services;
using FirebaseExperiment01.Services.Auth;
using FirebaseExperiment01.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using FirebaseExperiment01.Services.Preferences;
using Plugin.Firebase.Bundled.Shared;
using System.Text.Json;


namespace FirebaseExperiment01;

public static class MauiProgram
{

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            // Initialize the .NET MAUI Community Toolkit by adding the below line of code
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .BuildSettings()
            .RegisterFirebaseServices()
            .RegisterServices()
            .RegisterViewModels();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        ServiceHelper.Initialize(app.Services);

        if (app.Services.GetService<NavigationPage>() is NavigationPage navi)
        {
            NavigationHelper.Initialize(navi.Navigation);
        }

        return app;
    }

    /// <summary>
    /// DIコンテナへService群を登録
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<IPreferencesService, PreferencesService>()
            .AddSingleton(_ => CrossFirebaseAuthGoogle.Current)

            .AddSingleton<FirebaseService>();
          //  .AddSingleton<AuthService>();

        return builder;
    }

    /// <summary>
    /// DIコンテナへViewModelをViewに関連付けて登録
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<Views.BootstrapPage, ViewModels.BootstrapViewModel>()
            .AddSingleton<Views.BlankPage>()
            .AddSingleton<Views.LoginPage, ViewModels.LoginViewModel>()
            .AddSingleton<NavigationPage>()
            .AddSingleton<Views.MainTabbedPage, ViewModels.MainTabbedViewModel>()
            .AddSingleton<AppShell>()
            .AddSingleton<Views.HomePage, ViewModels.HomeViewModel>()
            .AddSingleton<Views.OptionPage, ViewModels.OptionViewModel>()
            .AddSingleton<Views.LogoutedPage, ViewModels.LogoutedViewModel>();

        return builder;
    }

    /// <summary>
    /// Firebaseの初期設定
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
    {

        builder.ConfigureLifecycleEvents(events =>
        {
#if IOS
            events.AddiOS(iOS => iOS.WillFinishLaunching((_,__) => {
                CrossFirebase.Initialize();
                FirebaseAuthGoogleImplementation.Initialize();
                CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
                return false;
            }));
#elif ANDROID
            events.AddAndroid(android => android.OnCreate((activity, _) =>
            {
                var fbSettings = CreateCrossFirebaseSettings();
                CrossFirebase.Initialize(activity, fbSettings);
                FirebaseAuthGoogleImplementation.Initialize(fbSettings.GoogleRequestIdToken);
                CrossFirebaseCrashlytics.Current.SetCrashlyticsCollectionEnabled(true);
            }));
#endif
        });

        builder.Services.AddSingleton(_ => CrossFirebaseAuth.Current);

        return builder;
    }

    private static MauiAppBuilder BuildSettings(this MauiAppBuilder builder)
    {
        using var stream = FileSystem.OpenAppPackageFileAsync("settings.json").Result;
        using var reader = new StreamReader(stream);
        var contents = reader.ReadToEnd();

        var settings = JsonSerializer.Deserialize<Settings>(contents);

        builder.Services.AddSingleton<Settings>( _ => settings!);

        return builder;
    }

    private static CrossFirebaseSettings CreateCrossFirebaseSettings()
    {
        var settings = ServiceHelper.GetService<Settings>()!;

        return new CrossFirebaseSettings(
            isAnalyticsEnabled: true,
            isAuthEnabled: true,
            isCloudMessagingEnabled: false,
            isDynamicLinksEnabled: false,
            isFirestoreEnabled: true,
            isFunctionsEnabled: false,
            isRemoteConfigEnabled: false,
            isStorageEnabled: true,
            googleRequestIdToken: settings.GoogleWebApplicationClientId);
    }
}