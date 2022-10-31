using ApplicationCore.Domain.Core.Models;

namespace ApplicationCore.Domain.Interfaces.Interfaces
{
	public interface IRepository<T> where T : EntityBase
	{
		public Task<List<T>> GetAll();
		public Task Insert(T entity);
		public Task Update(T entity);
		public Task DeleteAsync(T entity);
	}
}
