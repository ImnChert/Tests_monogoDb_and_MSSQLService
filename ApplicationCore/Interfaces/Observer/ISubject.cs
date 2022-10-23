namespace ApplicationCore.Interfaces.Observer
{
	internal interface ISubject
	{
		void Attach(IObserver observer);

		// Отсоединяет наблюдателя от издателя.
		void Detach(IObserver observer);

		// Уведомляет всех наблюдателей о событии.
		void Notify(Exception ex);
	}
}
