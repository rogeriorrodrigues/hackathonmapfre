using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Plugin.Connectivity;

namespace UXDivers.Artina.Grial
{
	public partial class CheckinInsertPage : ContentPage
	{
		CheckinViewModel vm;

		public CheckinInsertPage()
		{
			InitializeComponent();
			BindingContext = vm = new CheckinViewModel();


		}

		void Handle_Clicked(object sender, System.EventArgs e)
		{
			throw new NotImplementedException();
		}
		
	}
}
