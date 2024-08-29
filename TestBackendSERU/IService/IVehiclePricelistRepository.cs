using static TestBackendSERU.Models.Vehicle;
using static TestBackendSERU.Models.Response;
using System.Threading.Tasks;

namespace TestBackendSERU.IService
{
    public interface IVehiclePricelistRepository
    {
        Task<Default> Add(Pricelist.AddPrice data);
        Task<Default> Edit(Pricelist.EditPrice data);
        Task<Default> Delete(Pricelist.DeletePrice data);
        Task<GetAllPricelist> GetPricelist(int limit, int offset);
        Task<Default> GetPricelistById(int id);
    }
}
