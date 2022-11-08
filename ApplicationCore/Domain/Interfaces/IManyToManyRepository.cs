namespace ApplicationCore.Domain.Interfaces
{
	public interface IManyToManyRepository<T>
	{
		public T GetManyToMany(int id);
		public void SetManyToMany(int id, List<T> values);
	}
}
