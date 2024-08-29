using static TestBackendSERU.Models.Vehicle;
using static TestBackendSERU.Models.Response;
using System.Threading.Tasks;

namespace TestBackendSERU.IService
{
    public interface IVehicleYearRepository
    {
        Task<Default> Add(Year.AddYear data);
        Task<Default> Edit(Year.EditYear data);
        Task<Default> Delete(Year.DeleteYear data);
        Task<GetAllYear> GetYear(int limit, int offset);
        Task<Default> GetYearById(int id);
    }
}
