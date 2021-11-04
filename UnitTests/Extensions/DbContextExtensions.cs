using Microsoft.EntityFrameworkCore;
using System;
using TestSupport.Helpers;

namespace UnitTests.Extensions
{
    public static class DbContextExtensions
    {
        public static DbContextOptions<T>CreateUniqueClassOptions<T>(this object callingClass, Action<DbContextOptionsBuilder<T>> builder = null)
        where T : DbContext
        {
            return CreateOptionWithDatabaseName<T>(callingClass, builder);
        }
        private static DbContextOptions<T>CreateOptionWithDatabaseName<T>(object callingClass, Action<DbContextOptionsBuilder<T>> extraOptions, string callingMember = null)
        where T : DbContext
        {
            var connectionString = "Server=127.0.1.1;Port=5432;Database=RepairMarketPlaceTestDb;User Id=postgres;Password=praise4DaSun!";
            var builder = new DbContextOptionsBuilder<T>();
            builder.UseNpgsql(connectionString);
            //builder.ApplyOtherOptionSettings();
            extraOptions?.Invoke(builder);
            return builder.Options;
        }
    }
}
