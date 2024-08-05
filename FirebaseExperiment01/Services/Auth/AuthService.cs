using System.ComponentModel.Design;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;

using Plugin.Firebase.Auth;
using Plugin.Firebase.Auth.Google;

using FirebaseExperiment01.Services.Preferences;
using System;


namespace FirebaseExperiment01.Services.Auth;

public sealed class AuthService //: IAuthService
{
    private readonly IFirebaseAuth _firebaseAuth;
    private readonly IFirebaseAuthGoogle _firebaseAuthGoogle;
    private readonly IPreferencesService _preferencesService;
    private readonly BehaviorSubject<IFirebaseUser> _currentUserSubject;
    private readonly ISubject<bool> _isSignInRunningSubject;

    public AuthService(
        IFirebaseAuth firebaseAuth,
        IFirebaseAuthGoogle firebaseAuthGoogle,
        IPreferencesService preferencesService)
    {
        _firebaseAuth = firebaseAuth;
        _firebaseAuthGoogle = firebaseAuthGoogle;
        _preferencesService = preferencesService;

        _currentUserSubject = new BehaviorSubject<IFirebaseUser>(null);
        _isSignInRunningSubject = new BehaviorSubject<bool>(false);

        _currentUserSubject.OnNext(_firebaseAuth.CurrentUser);
    }

    private IObservable<Unit> RunAuthTask(Task<IFirebaseUser> task, bool signOutWhenFailed = false)
    {
        _isSignInRunningSubject.OnNext(true);
        return Observable
            .FromAsync(_ => task)
            .Do(_currentUserSubject.OnNext)
            .ToUnit()
            //.Catch<Unit, Exception>(e => (signOutWhenFailed ? SignOut() : IObservable.Unit).SelectMany(Observable.Throw<Unit>(e)))
            .Finally(() => _isSignInRunningSubject.OnNext(false));
    }

    public IObservable<Unit> SignInWithGoogle()
    {
        return RunAuthTask(
            _firebaseAuthGoogle.SignInWithGoogleAsync(),
            signOutWhenFailed: true);
    }



    public IObservable<Unit> LinkWithGoogle()
    {
        return RunAuthTask(_firebaseAuthGoogle.LinkWithGoogleAsync());
    }

    public IObservable<Unit> UnlinkProvider(string providerId)
    {
        return RunAuthTask(CurrentUser
            .UnlinkAsync(providerId)
            .ToObservable()
            .Select(_ => _firebaseAuth.CurrentUser)
            .ToTask());
    }

    public IObservable<Unit> SendSignInLink(string toEmail)
    {
        return _firebaseAuth
            .SendSignInLink(toEmail, CreateActionCodeSettings())
            .ToObservable()
            .Do(_ => _preferencesService.Set(PreferenceKeys.SignInLinkEmail, toEmail));
    }

    private static ActionCodeSettings CreateActionCodeSettings()
    {
        var settings = new ActionCodeSettings();
        settings.Url = "https://playground-24cec.firebaseapp.com";
        settings.HandleCodeInApp = true;
        settings.IOSBundleId = "com.tobishiba.playground";
        settings.SetAndroidPackageName("com.tobishiba.playground", true, "21");
        return settings;
    }

    public IObservable<Unit> SignOut()
    {
        return Task
            .WhenAll(_firebaseAuth.SignOutAsync(), _firebaseAuthGoogle.SignOutAsync())
            .ToObservable()
            .Do(_ => HandleUserSignedOut());
    }

    public IObservable<string[]> FetchSignInMethods(string email)
    {
        return _firebaseAuth.FetchSignInMethodsAsync(email).ToObservable();
    }

    public IObservable<Unit> SendPasswordResetEmail()
    {
        return _firebaseAuth.SendPasswordResetEmailAsync().ToObservable();
    }

    private void HandleUserSignedOut()
    {
        _currentUserSubject.OnNext(null);
        _preferencesService.Remove(PreferenceKeys.SignInLinkEmail);
    }

    public bool IsSignInWithEmailLink(string link)
    {
        return _firebaseAuth.IsSignInWithEmailLink(link);
    }

    public IFirebaseUser CurrentUser => _currentUserSubject.Value;
    public IObservable<IFirebaseUser> CurrentUserTicks => _currentUserSubject.AsObservable();
    public IObservable<bool> IsSignedInTicks => CurrentUserTicks.Select(x => x != null);
    public IObservable<bool> IsSignInRunningTicks => _isSignInRunningSubject.AsObservable();
}