using System;
using RepairMarketPlace.Infrastructure.Services;
using RepairMarketPlace.ApplicationCore.Entities;
using Xunit;
using System.Collections.Generic;
using TestSupport.Helpers;
using System.IO;

namespace UnitTests
{
    public class FileDeserializerTest
    {
        [Theory]
        [InlineData(@"\Case.json")]
        public void DeserializeFileHappyPath(string fileName)
        {
            string fileDir = TestData.GetTestDataDir();
            List<Component> idealObject = new()
            {
                new Component()
                {
                    Name = "Corsair 4000D Airflow",
                    Type = ComponentType.Case
                },
                new Component()
                {
                    Name = "Phanteks Eclipse P300A Mesh",
                    Type = ComponentType.Case
                }
            };

            FileDeserializer fileDeserializer = new();
            List<Component> testObject = fileDeserializer.DeserializeFile(fileDir + fileName);

            Assert.NotNull(testObject);
            Assert.NotEmpty(testObject);
            Assert.Equal(idealObject.Count, testObject.Count);

            for (int i = 0; i < idealObject.Count; i++)
            {
                Assert.NotNull(testObject[i].Name);
                Assert.IsType<string>(testObject[i].Name);

                Assert.Equal(idealObject[i].Name, testObject[i].Name);
                Assert.Equal(idealObject[i].Type, testObject[i].Type);
            }
        }
    }
}
