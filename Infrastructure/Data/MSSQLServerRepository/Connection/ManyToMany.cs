namespace Infrastructure.Data.MSSQLServerRepository.Connection
{
	public class ManyToMany<T>
	{
		public int Id { get; set; }
		public List<T> ManyList { get; set; } = new List<T>();
	}
}
