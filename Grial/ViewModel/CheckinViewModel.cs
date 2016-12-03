using System;
using MvvmHelpers;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;

using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;

namespace UXDivers.Artina.Grial
{
	public class CheckinViewModel : BaseViewModel
	{
		AzureService azureService;
		public CheckinViewModel()
		{
			azureService = DependencyService.Get<AzureService>();
		}

		public ObservableRangeCollection<Checkin> Checkins { get; } = new ObservableRangeCollection<Checkin>();
		public ObservableRangeCollection<Grouping<string, Checkin>> CheckinsGrouped { get; } = new ObservableRangeCollection<Grouping<string, Checkin>>();

		string loadingMessage;
		public string LoadingMessage
		{
			get { return loadingMessage; }
			set { SetProperty(ref loadingMessage, value); }
		}

		ICommand loadCheckinsCommand;
		public ICommand LoadCheckinsCommand =>
			loadCheckinsCommand ?? (loadCheckinsCommand = new Command(async () => await ExecuteLoadCheckinsCommandAsync()));

		async Task ExecuteLoadCheckinsCommandAsync()
		{
			//if (IsBusy || !(await LoginAsync()))
			//	return;


			try
			{
				LoadingMessage = "Loading Checkins...";
				IsBusy = true;
				var coffees = await azureService.GetCheckins();
				Checkins.ReplaceRange(coffees);


				SortCheckins();


			}
			catch (Exception ex)
			{
				Debug.WriteLine("OH NO!" + ex);

				await Application.Current.MainPage.DisplayAlert("Sync Error", "Unable to sync Checkins, you may be offline", "OK");
			}
			finally
			{
				IsBusy = false;
			}


		}

		void SortCheckins()
		{
			var groups = from checkin in Checkins
				orderby checkin.DataInsert descending
						 group checkin by checkin.DateDisplay
				into checkinGroup
						 select new Grouping<string, Checkin>($"{checkinGroup.Key} ({checkinGroup.Count()})", checkinGroup);


			CheckinsGrouped.ReplaceRange(groups);
		}

		bool atHome;
		public bool AtHome
		{
			get { return atHome; }
			set { SetProperty(ref atHome, value); }
		}

		string local;
		string endereco;
		string cidade;
		string estado;
		string alerta;
	

		public string Local 
		{
			get { return local; }
			set { SetProperty(ref local, value); }
		}

		public string Endereco
		{
			get { return endereco; }
			set { SetProperty(ref endereco, value); }
		}

		public string Cidade
		{
			get { return cidade; }
			set { SetProperty(ref cidade, value); }
		}

		public string Estado
		{
			get { return estado; }
			set { SetProperty(ref estado, value); }
		}

		public string Alerta
		{
			get { return alerta; }
			set { SetProperty(ref alerta, value); }
		}


		ICommand addCheckinCommand;
		public ICommand AddCheckinCommand =>
			addCheckinCommand ?? (addCheckinCommand = new Command(async () => await ExecuteAddCheckinCommandAsync()));

		async Task ExecuteAddCheckinCommandAsync()
		{
			Geocoder geoCoder;

			geoCoder = new Geocoder();

			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 100;

			var position = await locator.GetPositionAsync(1000);

			string latitude = position.Latitude.ToString();
			string longitude = position.Longitude.ToString();
			//if (IsBusy || !(await LoginAsync()))
			//	return;
			 var check = new Checkin
			{
				latitute = double.Parse(latitude),
				longitude = double.Parse(longitude),
				cidade = Cidade,
				estado = Estado,
				DataInsert = DateTime.Now.Date,
				Local = Local,
				Endereco = Endereco,
				alerta = Alerta

			};

			try
			{
				LoadingMessage = "Adding Checkin...";
				IsBusy = true;



				var checkin = await azureService.AddCheckin(check);
				Checkins.Add(checkin);
				SortCheckins();
			}
			catch (Exception ex)
			{
				Debug.WriteLine("OH NO!" + ex);
			}
			finally
			{
				LoadingMessage = string.Empty;
				IsBusy = false;
			}

		}

		/*public Task<bool> LoginAsync()
		{
			if (Settings.IsLoggedIn)
				return Task.FromResult(true);


			return azureService.LoginAsync();
		}*/

	}
}
