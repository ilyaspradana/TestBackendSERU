using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TestBackendSERU.IService;
using static TestBackendSERU.Models.Vehicle;
using static TestBackendSERU.Models.Response;

namespace TestBackendSERU.Controllers
{
    [Route("api/vehicle/brand")]
    [ApiController]
    public class VehicleBrandController : Controller
    {
        private readonly IVehicleBrandRepository _vehicleBrandRepository;
        private readonly IConfiguration _configuration;

        public VehicleBrandController(IVehicleBrandRepository vehicleBrandRepository, IConfiguration configuration)
        {
            _vehicleBrandRepository = vehicleBrandRepository;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetBrands([FromQuery] int? id,int limit = 10, int offset = 0)
        {
            var r = new Default();

            if (id.HasValue)
            {
                var data = await _vehicleBrandRepository.GetBrandById(id.Value);

                if (data == null)
                {
                    r.Status = "Failed";
                    r.Message = "Brand tidak ditemukan";
                }
                else
                {
                    r.Status = "Success";
                    r.Message = data;
                }
            }
            else
            {
                var data = await _vehicleBrandRepository.GetBrand(limit, offset);

                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(data.Paginantion));

                r.Status = "Success";
                r.Message = data;
            }

            return Ok(r);
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBrand([FromBody] Brand.Add brand)
        {
            var r = new Default();

            if (brand == null || string.IsNullOrEmpty(brand.Name))
            {
                r.Status = "Failed";
                r.Message = "Harap isi Nama Brand";

                return Ok(r);
            }

            var data = await _vehicleBrandRepository.Add(brand);

            if (data == null)
            {
                r.Status = "Failed";
                r.Message = "Nama Brand sudah terdaftar";
            }
            else
            {
                r.Status = "Success";
                r.Message = "Brand " + data.Name + " berhasil disimpan";
            }

            return Ok(r);
        }

        [HttpPost("edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditBrand([FromBody] Brand.Edit brand)
        {
            var r = new Default();

            if (brand == null || string.IsNullOrEmpty(brand.Name) || brand.ID <= 0)
            {
                r.Status = "Failed";
                r.Message = "Harap pilih Brand terlebih dahulu";

                return Ok(r);
            }

            var data = await _vehicleBrandRepository.Edit(brand);

            if (data == null)
            {
                r.Status = "Failed";
                r.Message = "Nama Brand tidak ditemukan";
            }
            else
            {
                r.Status = "Success";
                r.Message = "Nama Brand berhasil diubah";
            }

            return Ok(r);
        }

        [HttpPost("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBrand([FromBody] Brand.Delete brand)
        {
            var r = new Default();

            if (brand == null || brand.ID <= 0)
            {
                r.Status = "Failed";
                r.Message = "Harap pilih Brand terlebih dahulu";

                return Ok(r);
            }

            var data = await _vehicleBrandRepository.Delete(brand);

            if (data == null)
            {
                r.Status = "Failed";
                r.Message = "Brand tidak ditemukan";
            }
            else
            {
                r.Status = "Success";
                r.Message = "Brand berhasil dihapus";
            }

            return Ok(r);
        }
    }
}
