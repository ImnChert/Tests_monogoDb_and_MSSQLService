namespace ApplicationCore.Domain.Core.Models.Cinema.Films
{
	public class Distributor : EntityBase
	{
		public string NameCompany { get; set; }

		public override bool Equals(object? obj)
		{
			return obj is Distributor distributor &&
				   NameCompany == distributor.NameCompany;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(NameCompany);
		}
	}
}
