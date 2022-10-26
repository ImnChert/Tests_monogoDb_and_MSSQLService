namespace ApplicationCore.Models.Cinema
{
	internal class Hall
	{
		public int Number { get; set; }
		public int Row { get; set; } = 10;
		public int Column { get; set; } = 10;

		public Hall() 
		{ }
		public Hall(int number)
		{
			Number = number;
		}
		public Hall(int number, int maxRow, int maxColumn)
		{
			Number = number;
			Row = maxRow;
			Column = maxColumn;
		}

	}
}
