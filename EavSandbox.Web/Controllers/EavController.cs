using System.Threading.Tasks;
using EavSandbox.Web.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace EavSandbox.Web.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class EavController : ControllerBase
    {
        private readonly IEavRepository _eavRepository;

        public EavController(IEavRepository eavRepository)
        {
            _eavRepository = eavRepository;
        }

        [HttpGet("{resource}")]
        public async Task<IActionResult> Get(string resource, [FromQuery]string expand)
        {
            var data = await _eavRepository.GetDataAsync(resource, expand);

            return Ok(data);
        }

        [HttpPost("{resource}")]
        public async Task<IActionResult> Post(string resource, [FromBody] JObject data)
        {
            await _eavRepository.AddDataAsync(data, resource);

            return Ok();
        }

        [HttpPut("{resource}/{id}")]
        public async Task Put(string resource, int id, [FromBody] JObject data)
        {
            await _eavRepository.UpdateDataAsync(data, resource);
        }

        [HttpDelete("{resource}/{id}")]
        public async Task Delete(int id)
        {
            await _eavRepository.DeleteDataAsync(id);
        }

        [HttpPut("reset")]
        public async Task Reset()
        {
            await _eavRepository.ResetStorageAsync();
        }
    }
}
