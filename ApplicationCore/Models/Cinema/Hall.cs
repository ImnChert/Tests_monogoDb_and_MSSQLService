namespace ApplicationCore.Models.Cinema
{
	internal class Hall
	{
		public int Number { get; set; }
		public int Row { get; set; } = 10;
		public int Column { get; set; } = 10;

		public Hall(int number, int maxRow, int maxColumn)
		{
			Number = number;
			Row = maxRow;
			Column = maxColumn;
		}
	}
}
