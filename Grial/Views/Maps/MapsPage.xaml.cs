using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Xamarin.Forms;

//Usado para geolocalização
using Xamarin.Forms.Maps;
using Plugin.Geolocator;

namespace UXDivers.Artina.Grial
{
	public partial class MapsPage : ContentPage
	{
		Geocoder geoCoder;
		static string end;
		public MapsPage()
		{
			InitializeComponent();

			geoCoder = new Geocoder();
			geoloc();
		}

		async void geoloc()
		{
			//transformar lat e long em endereço
			var enderecoPossivel = await geoCoder.GetAddressesForPositionAsync(
				new Position(-23.6099883, -46.699434));


			//Pegar o endereço possivel e exibir na label
			foreach (var endereco in enderecoPossivel)
			{
				end += endereco + "\n";
			}
			//Centralizando mapa na localização
			map.MoveToRegion(MapSpan.FromCenterAndRadius(
				new Position(-23.6099883, -46.699434),
				Distance.FromMiles(1)));

			//Criando Pin
			var pin = new Pin
			{
				Type = PinType.Place,
				Position = new Position(-23.6099883, -46.699434),
				Label = "Alerta! Zumbi!",
				Address = "The Walking Dead"
			};
			var pin2 = new Pin
			{
				Type = PinType.SavedPin,
				Position = new Position(-23.6098003, -46.6979977),
				Label = "Minha Localização",
				Address = end
			};

			var pin3 = new Pin
			{
				Type = PinType.SearchResult,
				Position = new Position(-23.6110543, -46.696787),
				Label = "Minha Localização",
				Address = end
			};

			var pin4 = new Pin
			{
				Type = PinType.SearchResult,
				Position = new Position(-23.6007426, -46.7023545),
				Label = "Minha Localização",
				Address = end
			};

			var pin5 = new Pin
			{
				Type = PinType.SearchResult,
				Position = new Position(-23.6107692, -46.7036964),
				Label = "Minha Localização",
				Address = end
			};

			var pin6 = new Pin
			{
				Type = PinType.SearchResult,
				Position = new Position(-23.615478, -46.704426),
				Label = "Minha Localização",
				Address = end
			};

			var pin7 = new Pin
			{
				Type = PinType.SearchResult,
				Position = new Position(-23.6145441, -46.6998662),
				Label = "Minha Localização",
				Address = end
			};



			//-23.6098003,-46.6979977
			//Add Pin ao mapa
			map.Pins.Add(pin);
			map.Pins.Add(pin2);
			map.Pins.Add(pin3);
			map.Pins.Add(pin4);
			map.Pins.Add(pin5);
			map.Pins.Add(pin6);
			map.Pins.Add(pin7);
		}
}
}
