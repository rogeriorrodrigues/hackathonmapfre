using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace UXDivers.Artina.Grial
{
	public partial class CheckinListPage : ContentPage
	{
		CheckinViewModel vm;
		public CheckinListPage()
		{
			InitializeComponent();
			BindingContext = vm = new CheckinViewModel();
			ListViewCheckins.ItemTapped += (sender, e) =>
			{
				if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
					ListViewCheckins.SelectedItem = null;
			};

			if (Device.OS != TargetPlatform.iOS && Device.OS != TargetPlatform.Android)
			{
				ToolbarItems.Add(new ToolbarItem
				{
					Text = "Refresh",
					Command = vm.LoadCheckinsCommand
				});
			}


		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			CrossConnectivity.Current.ConnectivityChanged += ConnecitvityChanged;
			OfflineStack.IsVisible = !CrossConnectivity.Current.IsConnected;

			vm.LoadCheckinsCommand.Execute(null);
			/*if (vm.Checkins.Count == 0 && Settings.IsLoggedIn)
				vm.LoadCheckinsCommand.Execute(null);
			else
			{
				await vm.LoginAsync();
				if (Settings.IsLoggedIn)
					vm.LoadCoffeesCommand.Execute(null);
			}*/
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			CrossConnectivity.Current.ConnectivityChanged -= ConnecitvityChanged;
		}

		void ConnecitvityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
				{
					OfflineStack.IsVisible = !e.IsConnected;
				});
		}
	}
}
