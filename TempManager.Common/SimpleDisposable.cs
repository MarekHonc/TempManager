namespace TempManager.Common
{
	/// <summary>
	/// Jednoduchý <see cref="IDisposable"/> objekt, kterému lze definovat akci na začátku a na konci.
	/// </summary>
	public class SimpleDisposable : IDisposable
	{
		private readonly Action end;

		/// <summary>
		/// Vrací, zda už tento objekt zaniknul.
		/// </summary>
		public bool IsDisposed
		{
			get;
			private set;
		}

		/// <summary>
		/// Konstruktor definující akci, která se má spustit při dispose tohoto objektu.
		/// </summary>
		public SimpleDisposable(Action end)
		{
			// uložím si akci, kterou budu volat při ukončení
			this.end = end;
		}

		/// <summary>
		/// Konstruktor s akcí, která se spustí v době vytvoření tohoto objektu a druhou akcí pro dispose.
		/// </summary>
		public SimpleDisposable(Action begin, Action end)
		{
			// uložím si akci, kterou budu volat při ukončení
			this.end = end;
			// zavolám počáteční akci
			begin();
		}

		public void Dispose()
		{
			// zavolám konečnou akci
			end();
			// nastavím příznak zániku
			IsDisposed = true;
		}
	}
}
