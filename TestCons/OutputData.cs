namespace TestConsole
{
	internal class OutputData<T>
	{
		public void OutputList(List<T> list)
			=> list.ForEach(x => Console.WriteLine(x));

		public void OutputObject(T obj)
			=> Console.WriteLine(obj);
	}
}
