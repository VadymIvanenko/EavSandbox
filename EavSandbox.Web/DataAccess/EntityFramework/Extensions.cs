using System.Collections.Generic;
using System.Linq;
using EavSandbox.Web.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace EavSandbox.Web.DataAccess.EntityFramework
{
    public static class Extensions
    {
        public static Include[] Next(this Include[] includes) =>
            includes.Where(p => p.Next != null).Select(p => p.Next).ToArray();

        public static bool HasSingleElement<T>(this IEnumerable<T> set) =>
            set.Count() == 1;

        public static IQueryable<Entity> IncludeAll(this IQueryable<Entity> entities)
        {
            return entities.Include(e => e.Category)
                .Include($"{nameof(Entity.AttributeValues)}.{nameof(AttributeValue.Attribute)}")
                // +1 level hardcoded
                .Include($"{nameof(Entity.Entities)}.{nameof(Entity.Category)}")
                .Include($"{nameof(Entity.Entities)}.{nameof(Entity.AttributeValues)}.{nameof(AttributeValue.Attribute)}");
        }
    }
}
