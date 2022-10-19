namespace ApplicationCore.Models.Cinema
{
	internal class Category
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
