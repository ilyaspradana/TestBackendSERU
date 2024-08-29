using System.Threading.Tasks;
using static TestBackendSERU.Models.Vehicle;
using static TestBackendSERU.Models.Response;

namespace TestBackendSERU.IService
{
    public interface IVehicleTypeRepository
    {
        Task<Default> Add(Type.AddType data);
        Task<Default> Edit(Type.EditType data);
        Task<Default> Delete(Type.DeleteType data);
        Task<GetAllType> GetType(int limit, int offset);
        Task<Default> GetTypeById(int id);
    }
}
