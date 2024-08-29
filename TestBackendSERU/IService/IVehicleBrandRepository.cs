using static TestBackendSERU.Models.Response;
using static TestBackendSERU.Models.Vehicle;
using System.Threading.Tasks;

namespace TestBackendSERU.IService
{
    public interface IVehicleBrandRepository
    {
        Task<Brand.Add> Add(Brand.Add brand);
        Task<Brand.Delete> Delete(Brand.Delete brand);
        Task<Brand.Edit> Edit(Brand.Edit brand);
        Task<GetAllBrand> GetBrand(int limit, int offset);
        Task<GetBrandById> GetBrandById(int id);
    }
}
