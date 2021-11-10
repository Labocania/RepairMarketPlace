using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepairMarketPlace.ApplicationCore.Entities;
using RepairMarketPlace.Infrastructure.Data;
using RepairMarketPlace.Infrastructure.Identity;
using RepairMarketPlace.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RepairMarketPlace.Infrastructure.Extensions
{
    public static class SeedDatabaseExtensions
    {
        public static async Task SeedDatabaseIfNoComponentTypeAsync(this AppDbContext context, string dataDirectory)
        {
            string[] filePaths = GetJsonFiles(dataDirectory);
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath).Replace(".json", "");
                context.ComponentTypes.Add(new ComponentType() { Name = fileName });
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedDatabaseIfNoComponentsAsync(this AppDbContext context, string dataDirectory)
        {
            string[] filePaths = GetJsonFiles(dataDirectory);
            FileDeserializer fileDeserializer = new();

            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath).Replace(".json", "");
                int componentTypeId = context.ComponentTypes.AsNoTracking().Where(x => x.Name == fileName).Select(x => x.Id).FirstOrDefault();

                HashSet<Component> components = fileDeserializer.DeserializeFile(filePath);

                foreach (Component component in components)
                {
                    component.ComponentTypeId = componentTypeId;
                }
                await context.Components.AddRangeAsync(components);
            }

            await context.SaveChangesAsync();
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
