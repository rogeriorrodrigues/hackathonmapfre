using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UXDivers.Artina.Grial
{
	//[XamlCompilation (XamlCompilationOptions.Skip)]
	public partial class App : Application
	{
		public static DateTime MinimumDate = DateTime.Parse("Jan 1 2000");
		public static DateTime MaximumDate = DateTime.Parse("Dec 31 2050");

		public static MasterDetailPage MasterDetailPage;

		public App ()
		{
			InitializeComponent ();

			MainPage = GetMainPage();

			MainPage.SetValue (NavigationPage.BarTextColorProperty, Color.White);
		}
			
		public static Page GetMainPage()
		{
			return new RootPage(true);
		}
	}
}