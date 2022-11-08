using ApplicationCore.Domain.Core.Models.Roles.Staff;
using ApplicationCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.MSSQLServerRepository.Implementations
{
	internal class PositionRepository : IManyToManyRepository<Position>
	{
		public Position GetManyToMany(int id)
		{
			throw new NotImplementedException();
		}

		public void SetManyToMany(int id, List<Position> values)
		{
			throw new NotImplementedException();
		}
	}
}
