using ApplicationCore.Domain.Core.Models;

namespace ApplicationCore.Domain.Interfaces.Interfaces
{
	public interface IRepository<T> where T : EntityBase
	{
		public Task<List<T>> GetAllAsync();
		public Task<T> GetById(int id);
		public Task InsertAsync(T entity);
		public Task UpdateAsync(T entity);
		public Task DeleteAsync(T entity);
	}
}
