using ApplicationCore.Domain.Core.Models;

namespace ApplicationCore.Domain.Interfaces
{
	public interface IShortRepository<T> where T : EntityBase
	{
		public Task<bool> InsertAsync(T entity);
		public Task<bool> UpdateAsync(T entity);
		public Task<bool> DeleteAsync(T entity);
	}
}
