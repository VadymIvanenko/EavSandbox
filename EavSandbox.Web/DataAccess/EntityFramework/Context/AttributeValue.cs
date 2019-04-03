namespace EavSandbox.Web.EntityFramework.Context
{
    public class AttributeValue
    {
        public int EntityId { get; set; }

        public int AttributeId { get; set; }

        public Entity Entity { get; set; }

        public EntityAttribute Attribute { get; set; }

        public string Value { get; set; }
    }
}
