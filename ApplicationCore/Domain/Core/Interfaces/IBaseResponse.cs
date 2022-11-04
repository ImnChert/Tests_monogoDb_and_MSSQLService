using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Domain.Core.Interfaces
{
	public interface IBaseResponse<T>
	{
		T Data { get; set; }
		public StatusCodeResult StatusCode { get; set; }
	}
}
