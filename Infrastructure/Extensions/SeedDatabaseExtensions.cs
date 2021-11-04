using RepairMarketPlace.ApplicationCore.Entities;
using RepairMarketPlace.Infrastructure.Data;
using RepairMarketPlace.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairMarketPlace.Infrastructure.Extensions
{
    public static class SeedDatabaseExtensions
    {
        public static async Task<int> SeedDatabaseIfNoComponentsAsync(this AppDbContext context, string dataDirectory)
        {
            int numComponents = context.Components.Count();
            if (numComponents == 0)
            {
                string[] filePaths = GetJsonFiles(dataDirectory);
                FileDeserializer fileDeserializer = new();

                foreach (string filePath in filePaths)
                {
                    await context.Components.AddRangeAsync(fileDeserializer.DeserializeFile(filePath));
                    numComponents++;
                }

                await context.SaveChangesAsync();
            }
            return numComponents;
        }

        static string[] GetJsonFiles(string dataDirectory)
        {
            string[] fileList = Directory.GetFiles(dataDirectory);
            if (fileList.Length == 0)
            {
                throw new FileNotFoundException($"Could not find a file in directory {dataDirectory}");
            }

            return fileList;
        }
    }
}
