using ApplicationCore.Domain.Core.Models;

namespace ApplicationCore.Domain.Interfaces.Interfaces
{
	public interface IRepository<T> where T : EntityBase
	{
		public List<T> GetAll();
		public void Insert(T entity);
		public void Update(T entity);
		public void Delete(T entity);
	}
}
