namespace ApplicationCore.Interfaces
{
	internal interface IFullName
	{
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string FullName => $"{FirstName} {MiddleName} {LastName}";
	}
}
