using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Swashbuckle.Swagger;

namespace S7TechIntegracao.API {
    public class SwaggerExcludeFilter : ISchemaFilter {
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type) {
            if (schema?.properties == null || type == null)
                return;

            var excludedProperties = type.GetProperties()
                                         .Where(t =>
                                                t.GetCustomAttribute<SwaggerExcludeAttribute>()
                                                != null);

            foreach (var excludedProperty in excludedProperties) {
                if (schema.properties.ContainsKey(excludedProperty.Name))
                    schema.properties.Remove(excludedProperty.Name);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SwaggerExcludeAttribute : Attribute {
    }
}