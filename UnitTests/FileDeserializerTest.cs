using RepairMarketPlace.Infrastructure.Services;
using RepairMarketPlace.ApplicationCore.Entities;
using Xunit;
using System.Collections.Generic;
using TestSupport.Helpers;

namespace UnitTests
{
    public class FileDeserializerTest
    {
        [Theory]
        [InlineData(@"\Case.json")]
        public void DeserializeFileHappyPath(string fileName)
        {
            string fileDir = TestData.GetTestDataDir();
            HashSet<Component> idealObject = new()
            {
                new Component()
                {
                    Name = "Corsair 4000D Airflow",
                    //Type = ComponentType.Case
                },
                new Component()
                {
                    Name = "Phanteks Eclipse P300A Mesh",
                    //Type = ComponentType.Case
                }
            };

            FileDeserializer fileDeserializer = new();
            HashSet<Component> testObject = fileDeserializer.DeserializeFile(fileDir + fileName);

            Assert.NotNull(testObject);
            Assert.NotEmpty(testObject);
            Assert.Equal(idealObject.Count, testObject.Count);
            Assert.True(testObject.SetEquals(idealObject));
        }
    }
}
