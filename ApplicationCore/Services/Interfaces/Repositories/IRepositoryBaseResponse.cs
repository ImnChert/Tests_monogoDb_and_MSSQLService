using ApplicationCore.Domain.Core.Models;

namespace ApplicationCore.Services.Interfaces.Repositories
{
	public interface IRepositoryBaseResponse<T>
	{
		public Task<BaseResponse<List<T>>> GetAllAsync();
		public Task<BaseResponse<T>> GetById(int id);
		public Task<BaseResponse<bool>> InsertAsync(T entity);
		public Task<BaseResponse<bool>> UpdateAsync(T entity);
		public Task<BaseResponse<bool>> DeleteAsync(T entity);
	}
}
