using ApplicationCore.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Domain.Interfaces
{
	public interface IGetAllById<T>
	{
		public List<T> GetAllById(EntityBase entity);
	}
}
