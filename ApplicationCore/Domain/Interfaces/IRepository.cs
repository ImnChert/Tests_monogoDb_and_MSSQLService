using ApplicationCore.Domain.Core.Models;

namespace ApplicationCore.Domain.Interfaces.Interfaces
{
	public interface IRepository<T> where T : EntityBase
	{
		public Task<List<T>> GetAllAsync();
		public Task<T> GetById(int id);
		public Task<bool> InsertAsync(T entity);
		public Task<bool> UpdateAsync(T entity);
		public Task<bool> DeleteAsync(T entity);
	}
}
