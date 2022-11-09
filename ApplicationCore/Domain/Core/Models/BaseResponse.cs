using ApplicationCore.Domain.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Domain.Core.Models
{
	public class BaseResponse<T> : IBaseResponse<T>
	{
		public required T Data { get; set; }
		public required StatusCodeResult StatusCode { get; set; }
		public string Description { get; set; }
	}
}
