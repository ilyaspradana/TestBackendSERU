using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TestBackendSERU.IService;
using static TestBackendSERU.Models.Response;
using static TestBackendSERU.Models.Vehicle;

namespace TestBackendSERU.Controllers
{
    [Route("api/vehicle/year")]
    [ApiController]
    public class VehicleYearController : Controller
    {
        private readonly IVehicleYearRepository _vehicleYearRepository;
        private readonly IConfiguration _configuration;

        public VehicleYearController(IVehicleYearRepository vehicleYearRepository, IConfiguration configuration)
        {
            _vehicleYearRepository = vehicleYearRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBrands([FromQuery] int? id, int limit = 10, int offset = 0)
        {
            if (id.HasValue)
            {
                var response = await _vehicleYearRepository.GetYearById(id.Value);

                return Ok(response);
            }
            else
            {
                var response = new Default();

                var data = await _vehicleYearRepository.GetYear(limit, offset);

                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(data.Paginantion));

                response.Status = "Success";
                response.Message = data;

                return Ok(response);
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddYear([FromBody] Year.AddYear year)
        {
            var response = await _vehicleYearRepository.Add(year);

            return Ok(response);
        }

        [HttpPost("edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditYear([FromBody] Year.EditYear year)
        {
            var response = await _vehicleYearRepository.Edit(year);

            return Ok(response);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteYear([FromBody] Year.DeleteYear year)
        {
            var response = await _vehicleYearRepository.Delete(year);

            return Ok(response);
        }
    }
}
