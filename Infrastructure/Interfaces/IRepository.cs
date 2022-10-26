using Infrastructure.Models;

namespace Infrastructure.Interfaces
{
	public interface IRepository<T> where T : EntityBase
	{
		public List<T> GetAll();
		public void Insert(T entity);
		public void Update(T entity);
		public void Delete(T entity);
	}
}
