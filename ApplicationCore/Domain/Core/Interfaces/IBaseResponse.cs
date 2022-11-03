using ApplicationCore.Domain.Core.Enum;

namespace ApplicationCore.Domain.Core.Interfaces
{
	public interface IBaseResponse<T>
	{
		T Data { get; set; }
		public string Description { get; set; }
		public StatusCode StatusCode { get; set; }
	}
}
