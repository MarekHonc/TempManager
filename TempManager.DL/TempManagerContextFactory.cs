using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TempManager.DL
{
	/// <summary>
	/// Designer factory pro připojení k databázi kvůli migracím.
	/// </summary>
	public class TempManagerContextFactory : IDesignTimeDbContextFactory<TempManagerContext>
	{
		public TempManagerContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<TempManagerContext>();
			optionsBuilder.UseNpgsql("Host=localhost;Database=temp.manager;Username=postgres;Password=root");
			var context = new TempManagerContext(optionsBuilder.Options);

			return context;
		}
	}
}
