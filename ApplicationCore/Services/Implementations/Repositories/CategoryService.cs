using ApplicationCore.Domain.Core.Models;
using ApplicationCore.Domain.Core.Models.Cinema;
using ApplicationCore.Domain.Interfaces.Interfaces;
using ApplicationCore.Services.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Services.Implementations.Repositories
{
	public class CategoryService : IRepositoryBaseResponse<Category>
	{
		private IRepository<Category> _categoryRepository;

		public CategoryService(IRepository<Category> categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}

		public async Task<BaseResponse<bool>> DeleteAsync(Category entity)
		{
			try
			{
				if (entity == null)
				{
					return new BaseResponse<bool>()
					{
						Data = false,
						StatusCode = new NotFoundResult(),
						Description = "Null object"
					};
				}

				await _categoryRepository.DeleteAsync(entity);

				return new BaseResponse<bool>()
				{
					Data = true,
					StatusCode = new OkResult()
				};
			}
			catch
			{
				return new BaseResponse<bool>()
				{
					Data = false,
					StatusCode = new BadRequestResult(),
					Description = "Inevitable error"
				};
			}
		}

		public async Task<BaseResponse<List<Category>>> GetAllAsync()
		{
			try
			{
				var users = await _categoryRepository.GetAllAsync();

				return new BaseResponse<List<Category>>()
				{
					Data = users,
					Description = "Data received successfully.",
					StatusCode = new OkResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<Category>>()
				{
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}

		public async Task<BaseResponse<Category>> GetById(int id)
		{
			try
			{
				var role = await _categoryRepository.GetById(id);

				if (role == null)
				{
					return new BaseResponse<Category>()
					{
						StatusCode = new BadRequestResult(),
						Description = "A user with this id doesn't exist"
					};
				}

				return new BaseResponse<Category>()
				{
					Data = role,
					Description = "The user was successfully found.",
					StatusCode = new BadRequestResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<Category>()
				{
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}

		public async Task<BaseResponse<bool>> InsertAsync(Category entity)
		{
			try
			{
				var user = await _categoryRepository.GetById(entity.Id);

				if (user != null)
				{
					return new BaseResponse<bool>()
					{
						Data = false,
						StatusCode = new BadRequestResult(),
						Description = "A user with this name already exists"
					};
				}

				await _categoryRepository.InsertAsync(entity);

				return new BaseResponse<bool>()
				{
					Data = true,
					Description = "The user was successfully added.",
					StatusCode = new OkResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Data = false,
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}

		public async Task<BaseResponse<bool>> UpdateAsync(Category entity)
		{
			try
			{
				var data = _categoryRepository.GetById(entity.Id);

				if (data == null)
				{
					return new BaseResponse<bool>()
					{
						Data = false,
						StatusCode = new BadRequestResult(),
						Description = "A user with this id doesn't exist"
					};
				}

				await _categoryRepository.UpdateAsync(entity);

				return new BaseResponse<bool>()
				{
					Data = true,
					Description = "The user information was successfully updates.",
					StatusCode = new OkResult()
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool>()
				{
					Data = false,
					Description = ex.Message,
					StatusCode = new BadRequestResult()
				};
			}
		}
	}
}
