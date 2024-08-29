using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TestBackendSERU.IService;
using static TestBackendSERU.Models.Response;
using static TestBackendSERU.Models.Vehicle;

namespace TestBackendSERU.Controllers
{
    [Route("api/vehicle/model")]
    [ApiController]
    public class VehicleModelController : Controller
    {
        private readonly IVehicleModelRepository _vehicleModelRepository;
        private readonly IConfiguration _configuration;

        public VehicleModelController(IVehicleModelRepository vehicleModelRepository, IConfiguration configuration)
        {
            _vehicleModelRepository = vehicleModelRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBrands([FromQuery] int? id, int limit = 10, int offset = 0)
        {
            if (id.HasValue)
            {
                var response = await _vehicleModelRepository.GetModelById(id.Value);

                return Ok(response);
            }
            else
            {
                var response = new Default();

                var data = await _vehicleModelRepository.GetModel(limit, offset);

                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(data.Paginantion));

                response.Status = "Success";
                response.Message = data;

                return Ok(response);
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddModel([FromBody] Model.AddModel model)
        {
            var response = await _vehicleModelRepository.Add(model);

            return Ok(response);
        }

        [HttpPost("edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditModel([FromBody] Model.EditModel model)
        {
            var response = await _vehicleModelRepository.Edit(model);

            return Ok(response);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteModel([FromBody] Model.DeleteModel model)
        {
            var response = await _vehicleModelRepository.Delete(model);

            return Ok(response);
        }
    }
}
