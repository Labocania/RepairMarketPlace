using RepairMarketPlace.ApplicationCore.Entities;
using RepairMarketPlace.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using RepairMarketPlace.ApplicationCore.Extensions;

namespace RepairMarketPlace.Infrastructure.Services
{
    public class FileDeserializer : IDeserializeFile<List<Component>>
    {
        public List<Component> DeserializeFile(string filePath)
        {
            List<Component> components = new();

            using (FileStream openStream = File.OpenRead(filePath))
            {
                JsonDocument document = JsonDocument.Parse(openStream);

                using (document)
                {
                    foreach (var element in document.RootElement.EnumerateArray())
                    {
                        Component component = element.ToObject<Component>();
                        component.Type = FileNameToEnumConverter(Path.GetFileName(filePath));
                        components.Add(component);
                    }
                }
            }

            return components;
        }

        ComponentType FileNameToEnumConverter(string filename)
        {
            string name = filename.Replace(".json", "");
            return Enum.Parse<ComponentType>(name);
        }
    }
}
