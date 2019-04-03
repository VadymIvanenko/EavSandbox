using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EavSandbox.Web.EntityFramework.Context
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Entity> Entity { get; set; } = new Collection<Entity>();

        public ICollection<EntityAttribute> Attributes { get; set; } = new Collection<EntityAttribute>();
    }
}
