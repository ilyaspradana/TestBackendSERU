using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TestBackendSERU.IService;
using static TestBackendSERU.Models.Response;
using static TestBackendSERU.Models.Vehicle;

namespace TestBackendSERU.Controllers
{
    [Route("api/vehicle/type")]
    [ApiController]
    public class VehicleTypeController : Controller
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;
        private readonly IConfiguration _configuration;

        public VehicleTypeController(IVehicleTypeRepository vehicleTypeRepository, IConfiguration configuration)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBrands([FromQuery] int? id, int limit = 10, int offset = 0)
        {
            if (id.HasValue)
            {
                var response = await _vehicleTypeRepository.GetTypeById(id.Value);

                return Ok(response);
            }
            else
            {
                var response = new Default();

                var data = await _vehicleTypeRepository.GetType(limit, offset);

                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(data.Paginantion));

                response.Status = "Success";
                response.Message = data;

                return Ok(response);
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddType([FromBody] Type.AddType type)
        {
            var response = await _vehicleTypeRepository.Add(type);

            return Ok(response);
        }

        [HttpPost("edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditType([FromBody] Type.EditType type)
        {
            var response = await _vehicleTypeRepository.Edit(type);

            return Ok(response);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteType([FromBody] Type.DeleteType type)
        {
            var response = await _vehicleTypeRepository.Delete(type);

            return Ok(response);
        }
    }
}
