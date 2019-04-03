using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace EavSandbox.Web.DataAccess
{
    public interface IEavRepository
    {
        Task<JArray> GetDataAsync(string resource, string expand);

        Task AddDataAsync(JObject data, string resource);

        Task UpdateDataAsync(JObject data, string resource);

        Task DeleteDataAsync(int id);

        Task ResetStorageAsync();
    }
}
