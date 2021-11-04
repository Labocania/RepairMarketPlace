using RepairMarketPlace.ApplicationCore.Entities;
using RepairMarketPlace.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using RepairMarketPlace.ApplicationCore.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace RepairMarketPlace.Infrastructure.Services
{
    public class FileDeserializer : IDeserializeFile<HashSet<Component>>
    {
        public HashSet<Component> DeserializeFile(string filePath)
        {
            HashSet<Component> components = new(new ComponentEqualityComparer());

            using (FileStream openStream = File.OpenRead(filePath))
            {
                JsonDocument document = JsonDocument.Parse(openStream);

                using (document)
                {
                    foreach (var element in document.RootElement.EnumerateArray())
                    {
                        Component component = element.ToObject<Component>();
                        if (components.Contains(component))
                        {
                            continue;
                        }
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

    class ComponentEqualityComparer : IEqualityComparer<Component>
    {
        public bool Equals(Component x, Component y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.Name == y.Name && x.Type == y.Type)
                return true;
            else
                return false;
        }

        public int GetHashCode([DisallowNull] Component obj)
        {
           return HashCode.Combine(obj.Name, obj.Type);
        }
    }
}
