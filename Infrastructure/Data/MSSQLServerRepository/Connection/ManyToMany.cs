namespace Infrastructure.Data.MSSQLServerRepository.Connection
{
	internal class ManyToMany<T>
	{
		public int Id { get; set; }
		public List<T> ManyList { get; set; } = new List<T>();
	}
}
