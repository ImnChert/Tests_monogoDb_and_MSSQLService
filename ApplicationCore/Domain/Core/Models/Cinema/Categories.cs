namespace ApplicationCore.Domain.Core.Models.Cinema
{
	public class Category : EntityBase
	{
		public string Name	{ get; set; }
		public decimal Price { get; set; }

		public Category(string name, decimal price)
		{
			Name = name;
			Price = price;
		}
	}
}
