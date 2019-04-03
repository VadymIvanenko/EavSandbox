using EavSandbox.Web.EntityFramework.Context;
using Newtonsoft.Json.Linq;

namespace EavSandbox.Web.DataAccess.EntityFramework.Conversion
{
    public interface IEntityConverter
    {
        Entity ConvertJObjectToEntity(JObject root, string resource);

        JObject ConvertEntityToJObject(Entity entity, params Include[] includes);
    }
}
