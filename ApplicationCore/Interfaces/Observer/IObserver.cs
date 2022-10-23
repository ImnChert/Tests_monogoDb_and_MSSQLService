namespace ApplicationCore.Interfaces.Observer
{
	internal interface IObserver
	{
		// TODO: паттерн наблюдатель
		void Update(ISubject subject, Exception exception);
	}
}
