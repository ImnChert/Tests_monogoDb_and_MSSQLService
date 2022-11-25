namespace ApplicationCore.Services.Interfaces.Validations
{
	public interface IIsNotNull
	{
		public bool IsNotNull(object obj)
		{
			if (obj == null)
				throw new Exception($"{obj} is null");
			return true;
		}
	}
}
