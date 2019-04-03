using System;
using System.Linq;
using EavSandbox.Web.EntityFramework.Context;
using Newtonsoft.Json.Linq;

namespace EavSandbox.Web.DataAccess.EntityFramework.Conversion
{
    public class EntityConverter : IEntityConverter
    {
        public JObject ConvertEntityToJObject(Entity entity, params Include[] includes)
        {
            var token = new JObject();

            token.Add(nameof(Entity.Id), new JValue(entity.Id));

            foreach (var attr in entity.AttributeValues)
                token.Add(attr.Attribute.Name, new JValue(attr.Value));

            var groups = entity.Entities.Where(e => includes.Any(i => i.Value == e.Category.Name))
                .GroupBy(e => e.Category.Name);

            foreach (var group in groups)
            {
                if (group.HasSingleElement())
                {
                    token.Add(group.Key, ConvertEntityToJObject(group.Single(), includes.Next()));
                }
                else
                {
                    var array = new JArray();
                    foreach (var item in group) array.Add(ConvertEntityToJObject(item, includes.Next()));
                    token.Add(group.Key, array);
                }
            }

            return token;
        }

        public Entity ConvertJObjectToEntity(JObject root, string resource)
        {
            var entity = new Entity { Category = new Category { Name = resource } };

            JTokenToEntity(root, entity);

            return entity;
        }

        private void JTokenToEntity(JToken token, Entity entity)
        {
            if (token.Type == JTokenType.Object)
            {
                foreach (var item in token.AsJEnumerable()) JTokenToEntity(item, entity);
            }
            else if (token.Type == JTokenType.Property)
            {
                var property = (JProperty)token;
                var name = property.Name;
                var value = property.Value;

                if (value.Type == JTokenType.String)
                {
                    var attributeValue = new AttributeValue
                    {
                        Attribute = new EntityAttribute { Name = name, Category = new Category { Name = entity.Category.Name } },
                        Value = (string)value
                    };
                    entity.AttributeValues.Add(attributeValue);
                }
                else if (value.Type == JTokenType.Integer && name == nameof(Entity.Id))
                {
                    entity.Id = value.Value<int>();
                }
                else if (value.Type == JTokenType.Object)
                {
                    var childObj = new Entity { Category = new Category { Name = name } };
                    JTokenToEntity(value, childObj);
                    entity.Entities.Add(childObj);
                }
                else if (value.Type == JTokenType.Array)
                {
                    foreach (var item in value.AsJEnumerable())
                    {
                        var childEntity = new Entity { Category = new Category { Name = name } };
                        JTokenToEntity(item, childEntity);
                        entity.Entities.Add(childEntity);
                    }
                }
                else throw new Exception($"Unknown value type {value.Type.ToString()}.");
            }
            else throw new Exception($"Unknown token type {token.Type.ToString()}.");
        }
    }
}
