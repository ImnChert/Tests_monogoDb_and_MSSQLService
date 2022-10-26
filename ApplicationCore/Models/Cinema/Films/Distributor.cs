using Infrastructure.Models;

namespace ApplicationCore.Models.Cinema.Films
{
	internal class Distributor : EntityBase
	{
		public string NameCompany { get; set; }

		public Distributor(string nameCompany)
		{
			NameCompany = nameCompany;
		}
	}
}
