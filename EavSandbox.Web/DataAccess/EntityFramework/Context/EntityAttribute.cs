using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EavSandbox.Web.EntityFramework.Context
{
    /// <summary>
    /// Represents string attribute. Can be upgraded to support multiple data types.
    /// </summary>
    public class EntityAttribute
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public ICollection<AttributeValue> AttributeValues { get; set; } = new Collection<AttributeValue>();
    }
}
