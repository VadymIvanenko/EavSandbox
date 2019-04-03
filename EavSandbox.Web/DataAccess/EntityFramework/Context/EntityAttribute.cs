using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EavSandbox.Web.EntityFramework.Context
{
    public class EntityAttribute
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public ICollection<AttributeValue> AttributeValues { get; set; } = new Collection<AttributeValue>();
    }
}
