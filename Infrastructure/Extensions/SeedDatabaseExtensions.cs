using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using RepairMarketPlace.Infrastructure.Data;
using RepairMarketPlace.Infrastructure.Identity;
using RepairMarketPlace.Infrastructure.Services;
using System.IO;
using System.Linq;
using System.Security.Claims;
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

        public static async Task SeedAdminUser(this UserManager<User> userManager, IConfiguration config)
        {
            User adminUser = new() 
            { 
                UserName = config["AdminCredentials:Login"], 
                Email = config["AdminCredentials:Login"],
                EmailConfirmed = true,
                Name = "Jackson Bright", 
                Birthday = new System.DateTime(1990, 1, 1) 
            };
            IdentityResult result = await userManager.CreateAsync(adminUser, config["AdminCredentials:Password"]);
            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(adminUser, new Claim("Role", "Admin"));
            }
        }
    }
}
