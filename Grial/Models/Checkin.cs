using System;
namespace UXDivers.Artina.Grial
{
	public class Checkin
	{
		[Newtonsoft.Json.JsonProperty("Id")]
		public string Id { get; set; }

		[Microsoft.WindowsAzure.MobileServices.Version]
		public string AzureVersion { get; set; }

		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>The user identifier.</value>
		//[Newtonsoft.Json.JsonProperty("userId")]
		//public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the date UTC.
		/// </summary>
		/// <value>The date UTC.</value>
		public string Local { get; set; }
		public string Endereco { get; set; }

		public string cidade { get; set; }
		public string estado { get; set; }


		public double latitute { get; set; }
		public double longitude { get; set; }


		public string alerta { get; set; }
		public DateTime DataInsert { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="CoffeeCups.CupOfCoffee"/> made at home.
		/// </summary>
		/// <value><c>true</c> if made at home; otherwise, <c>false</c>.</value>



		[Newtonsoft.Json.JsonIgnore]
		public string DateDisplay { get { return DateTime.Now.ToString("d"); } }

		[Newtonsoft.Json.JsonIgnore]
		public string TimeDisplay { get { return DateTime.Now.ToString("t") + " " + AzureVersion.ToString(); } }


	}
}
