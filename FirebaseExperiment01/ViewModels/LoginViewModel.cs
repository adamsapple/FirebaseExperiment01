using CommunityToolkit.Mvvm.Input;
using FirebaseExperiment01.Services;
using FirebaseExperiment01.Services.Auth;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reactive.Linq;
using System.Security.Cryptography;

using Plugin.Firebase.CloudMessaging;
using Plugin.Firebase.Auth.Google;
using Plugin.Firebase.Firestore;
using FirebaseExperiment01.Models.Firestore;
using Plugin.Firebase.Storage;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace FirebaseExperiment01.ViewModels;

/// <summary>
/// 
/// </summary>
public partial class LoginViewModel : ViewModelBase
{
    #region Properties.
    #endregion Properties.

    string uid = string.Empty;

    #region Commands.
    [RelayCommand]
    private async Task LoginWithGoogle(string email)
    {
        var auth = await CrossFirebaseAuthGoogle.Current.SignInWithGoogleAsync();

        if(auth is null)
        {
            Console.WriteLine("user is null");
            return;
        }

        Console.WriteLine($"user:{auth.Uid}");
        uid = auth.Uid;

        var usersCollection = CrossFirebaseFirestore.Current.GetCollection("users");

        if( usersCollection is null)
        {
            Console.WriteLine("usersCollection is null");
            return;
        }

        var userDocument = usersCollection.GetDocument(auth.Uid);

        if (userDocument is null)
        {
            
        }

        var now  = DateTime.Now;
        var user = await userDocument.GetDocumentSnapshotAsync<UserDocument>();
        var userData = user?.Data;

        if (userData is null)
        {
            /// 新規追加
            Console.WriteLine($"new user[userData.Id]");
            userData = new UserDocument();

            userData.CreatedAt   = now;
            userData.LastLoginAt = now;
            userData.NickName    = "devtester";
            userData.Comment     = "now commer";

            await userDocument.SetDataAsync(userData);
        }else
        {
            /// 更新
            Console.WriteLine($"user[{userData.Id}]:{userData.NickName}, comment:{userData.Comment}");

            userData.LastLoginAt = now;
            userData.Comment     = $"test{DateTime.Now.Second}";
            
            await userDocument.SetDataAsync(userData);
        }





        //var page = Helpers.ServiceHelper.GetService<Views.MainTabbedPage>();
        //var page = Helpers.ServiceHelper.GetService<AppShell>(); 
        //await Helpers.NavigationHelper.Shell.GoToAsync("///Home");
        //await Helpers.NavigationHelper.Shell.Navigation!.PopModalAsync(true);

        //App.Current.MainPage = page;
        //await Helpers.NavigationHelper.Navigation!.PushAsync(page);
    }

    [RelayCommand]
    private async Task GetActivityLogs()
    {
        if (string.IsNullOrEmpty(uid) )
        {
            return;
        }

        const long maxDownloadSize = 1024 * 1024 * 20;
        var storageRef = CrossFirebaseStorage.Current.GetRootReference();

        var cardsRef = storageRef.GetChild($"cards/{uid}");

        var cards = await cardsRef.ListAllAsync();      /// 一覧取得
        if (cards != null)
        {

            var hashProvider = MD5.Create();

            foreach (var card in cards.Items)
            {
                Console.WriteLine($"card.name: {card.Name}, url: {card.FullPath}");

                var metaData = await card.GetMetadataAsync();
                var bytes    = await card.GetBytesAsync(maxDownloadSize);  /// ダウンロード
                //ReadOnlyMemory<byte> memory = bytes.AsMemory();

                var hashArray = hashProvider.ComputeHash(bytes);
                var hash      = Convert.ToBase64String(hashArray);
                //var hash = BitConverter.ToString(hashArray).Replace("-", String.Empty);

                using (var stream = new MemoryStream(bytes))
                using (var sr = new StreamReader(stream))
                {
                    var result = sr.ReadToEnd();
                    Console.WriteLine($"{card.Name}: {result}, md5 check: { hash.Equals(metaData.MD5Hash) }");
                }

                bytes = null;
                //using (var stream = await card.GetStreamAsync(maxDownloadSize))
                //{
                //    //stream.Position = 0;
                //    using (var sr = new StreamReader(stream))
                //    {
                //        var result = sr.ReadToEnd();
                //        Console.WriteLine($"{card.Name}: {result}");
                //    }
                //}
            }
        }
        
        ////
        /// アップロード(新規、上書き兼用)
        //
        {
            var cardRef       = storageRef.GetChild($"cards/{uid}/card99.txt");
            var metadata      = await cardRef.GetMetadataAsync();
            //var customMetaDic = metadata.CustomMetadata;

            var data = Encoding.GetEncoding("UTF-8").GetBytes($"hello upload.{DateTime.Now.ToString()}");
            //customMetaDic.Add("location", "hoge");

            //var task = cardRef.PutBytes(data, metadata);
            var task = cardRef.PutBytes(data);

            await task.AwaitAsync();
        }

        ////
        /// Metadata取得/更新。※正しく動かないようだ。
        //
        {
            var cardRef = storageRef.GetChild($"cards/{uid}/card99.txt");
            var metadata      = await cardRef.GetMetadataAsync();
            var customMetaDic = metadata.CustomMetadata;
            
            customMetaDic.Add("location", "hoge");

            await cardRef.UpdateMetadataAsync(metadata);

        }
    }

    [RelayCommand]
    private async Task DeleteLogs()
    {
        var storageRef = CrossFirebaseStorage.Current.GetRootReference();
        var cardRef = storageRef.GetChild($"cards/{uid}/card99.txt");

        await cardRef.DeleteAsync();
    }
    #endregion Commands.


    #region DI.
    //private IAuthService authService;
    private FirebaseService firebaseService;
    #endregion DI.

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="firebaseService"></param>
    public LoginViewModel(FirebaseService firebaseService)
    {
        //this.authService     = authService;
        this.firebaseService = firebaseService;

        Title = "Hello World";
    }
}
