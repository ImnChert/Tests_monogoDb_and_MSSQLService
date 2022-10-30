namespace ApplicationCore.Domain.Core.Interfaces.Observer
{
	public interface IObserver
	{
		// TODO: паттерн наблюдатель
		void Update(ISubject subject, Exception exception);
	}
}
