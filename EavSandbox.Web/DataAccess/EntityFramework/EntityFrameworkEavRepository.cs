using System.Linq;
using System.Threading.Tasks;
using EavSandbox.Web.DataAccess.EntityFramework.Conversion;
using EavSandbox.Web.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace EavSandbox.Web.DataAccess.EntityFramework
{
    public class EntityFrameworkEavRepository : IEavRepository
    {
        private readonly EavContext _context;
        private readonly IEntityConverter _entityConverter;

        public EntityFrameworkEavRepository(ContextFactory contextFactory, IEntityConverter entityConverter)
        {
            _context = contextFactory.CreateContext();
            _entityConverter = entityConverter;
        }

        public async Task<JArray> GetDataAsync(string resource, string expand)
        {
            var includes = Include.Parse(expand);

            var entities = await _context.Entities.Where(e => e.Category.Name == resource)
                .IncludeAll()
                .ToArrayAsync();

            var jObject = entities.Select(e => _entityConverter.ConvertEntityToJObject(e, includes));
            return new JArray(jObject);
        }

        public async Task AddDataAsync(JObject data, string resource)
        {
            var entity = _entityConverter.ConvertJObjectToEntity(data, resource);

            _context.Entities.Add(await ActualizeReferencesAsync(entity));

            await _context.SaveChangesAsync();
        }

        public async Task UpdateDataAsync(JObject data, string resource)
        {
            var update = _entityConverter.ConvertJObjectToEntity(data, resource);

            _context.Entities.Update(await ActualizeReferencesAsync(update));

            await _context.SaveChangesAsync();
        }

        public async Task DeleteDataAsync(int id)
        {
            void DeleteRecursively(Entity e)
            {
                _context.Entities.Remove(e);

                foreach (var item in e.Entities) DeleteRecursively(item);
            }

            var entity = await _context.Entities.Where(e => e.Id == id)
                .IncludeAll()
                .SingleOrDefaultAsync();

            DeleteRecursively(entity);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Actualizes the entity (foreign keys, linked references).
        /// </summary>
        private async Task<Entity> ActualizeReferencesAsync(Entity e)
        {
            Entity ActualizeReferences(Entity entity)
            {
                entity.Category = GetOrCreateCategory(entity.Category.Name);

                foreach (var item in entity.Entities)
                    ActualizeReferences(item);

                foreach (var attributeValue in entity.AttributeValues)
                {
                    attributeValue.Attribute = GetOrCreateAttribute(entity.Category.Name, attributeValue.Attribute.Name);
                    attributeValue.EntityId = entity.Id;
                    attributeValue.AttributeId = attributeValue.Attribute.Id;
                }

                return entity;
            }

            await _context.Categories.Include(c => c.Attributes).LoadAsync();

            return ActualizeReferences(e);
        }

        /// <summary>
        /// Gets category or creates new one.
        /// </summary>
        private Category GetOrCreateCategory(string name)
        {
            var category = _context.Categories.Local.SingleOrDefault(c => c.Name == name);
            if (category == null)
                _context.Categories.Add(category = new Category { Name = name });
            return category;
        }

        /// <summary>
        /// Gets attribute or creates new one.
        /// </summary>
        private EntityAttribute GetOrCreateAttribute(string categoryName, string attributeName)
        {
            var category = GetOrCreateCategory(categoryName);
            var attr = category.Attributes.SingleOrDefault(a => a.Name == attributeName);
            if (attr == null)
                category.Attributes.Add(attr = new EntityAttribute { Name = attributeName });
            return attr;
        }

        public async Task ResetStorageAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }
    }
}
