using ApplicationCore.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Domain.Core.Models
{
	public class BaseResponse<T> : IBaseResponse<T>
	{
		public T Data { get; set; }
		public required StatusCodeResult StatusCode { get; set; }
		public required string Description { get; set; }
	}
}
