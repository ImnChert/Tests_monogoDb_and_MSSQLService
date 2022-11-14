namespace ApplicationCore.Domain.Interfaces
{
	public interface IGetAllById<T>
	{
		public Task<List<T>> GetAllByIdOneToMany(int id);
	}
}
