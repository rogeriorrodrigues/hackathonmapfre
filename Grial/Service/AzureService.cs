using System;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Diagnostics;
using Xamarin.Forms;

using System.IO;
using Plugin.Connectivity;
using UXDivers.Artina.Grial;

[assembly: Dependency(typeof(AzureService))]
namespace UXDivers.Artina.Grial
{
	public class AzureService
	{

		public MobileServiceClient Client { get; set; } = null;
		IMobileServiceSyncTable<Checkin> checkinTable;
		public static bool UseAuth { get; set; } = false;

		public async Task Initialize()
		{
			if (Client?.SyncContext?.IsInitialized ?? false)
				return;


			var appUrl = "http://viagemsegura.azurewebsites.net";

#if AUTH
            Client = new MobileServiceClient(appUrl, new AuthHandler());

            if (!string.IsNullOrWhiteSpace (Settings.AuthToken) && !string.IsNullOrWhiteSpace (Settings.UserId)) {
                Client.CurrentUser = new MobileServiceUser (Settings.UserId);
                Client.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;
            }
#else
			//Create our client

			Client = new MobileServiceClient(appUrl);

#endif

			//InitialzeDatabase for path
			var path = "syncstore.db";
			path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);

			//setup our local sqlite store and intialize our table
			var store = new MobileServiceSQLiteStore(path);

			//Define table
			store.DefineTable<Checkin>();


			//Initialize SyncContext
			await Client.SyncContext.InitializeAsync(store);

			//Get our sync table that will call out to azure
			checkinTable = Client.GetSyncTable<Checkin>();


		}

		public async Task SyncChekin()
		{
			try
			{
				if (!CrossConnectivity.Current.IsConnected)
					return;

				await checkinTable.PullAsync("allCheckins", checkinTable.CreateQuery());

				await Client.SyncContext.PushAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to sync Checkin, that is alright as we have offline capabilities: " + ex);
			}

		}

		public async Task<IEnumerable<Checkin>> GetCheckins()
		{
			//Initialize & Sync
			await Initialize();
			await SyncChekin();

			return await checkinTable.OrderBy(c => c.DataInsert).ToEnumerableAsync();

		}

		public async Task<Checkin> AddCheckin(Checkin check)
		{
			await Initialize();

			var checkin = new Checkin
			{
				latitute = check.latitute,
				longitude = check.longitude,
				cidade = check.cidade,
				estado = check.estado,
				DataInsert = DateTime.Now.Date,
				Local = check.Local,
				Endereco = check.Endereco

			};

			await checkinTable.InsertAsync(checkin);

			await SyncChekin();
			//return coffee
			return checkin;
		}



		/*public async Task<bool> LoginAsync()
		{

			await Initialize();

			var auth = DependencyService.Get<IAuthentication>();
			var user = await auth.LoginAsync(Client, MobileServiceAuthenticationProvider.Twitter);

			if (user == null)
			{
				Settings.AuthToken = string.Empty;
				Settings.UserId = string.Empty;
				Device.BeginInvokeOnMainThread(async () =>
				{
					await App.Current.MainPage.DisplayAlert("Login Error", "Unable to login, please try again", "OK");
				});
				return false;
			}
			else
			{
				Settings.AuthToken = user.MobileServiceAuthenticationToken;
				Settings.UserId = user.UserId;
			}

			return true;
		}*/
	}
}
