using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TestBackendSERU.IService;
using static TestBackendSERU.Models.Vehicle;
using static TestBackendSERU.Models.Response;

namespace TestBackendSERU.Controllers
{
    [Route("api/vehicle/pricelist")]
    [ApiController]
    public class VehiclePricelistController : Controller
    {
        private readonly IVehiclePricelistRepository _vehiclePricelistRepository;
        private readonly IConfiguration _configuration;

        public VehiclePricelistController(IVehiclePricelistRepository vehiclePricelistRepository, IConfiguration configuration)
        {
            _vehiclePricelistRepository = vehiclePricelistRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPricelist([FromQuery] int? id, int limit = 10, int offset = 0)
        {
            if (id.HasValue)
            {
                var response = await _vehiclePricelistRepository.GetPricelistById(id.Value);

                return Ok(response);
            }
            else
            {
                var response = new Default();

                var data = await _vehiclePricelistRepository.GetPricelist(limit, offset);

                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(data.Paginantion));

                response.Status = "Success";
                response.Message = data;

                return Ok(response);
            }
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPricelist([FromBody] Pricelist.AddPrice price)
        {
            var response = await _vehiclePricelistRepository.Add(price);

            return Ok(response);
        }


        [HttpPost("edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditPricelist([FromBody] Pricelist.EditPrice price)
        {
            var response = await _vehiclePricelistRepository.Edit(price);

            return Ok(response);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePricelist([FromBody] Pricelist.DeletePrice price)
        {
            var response = await _vehiclePricelistRepository.Delete(price);

            return Ok(response);
        }
    }
}
