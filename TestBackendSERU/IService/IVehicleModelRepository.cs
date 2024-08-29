using static TestBackendSERU.Models.Vehicle;
using static TestBackendSERU.Models.Response;
using System.Threading.Tasks;

namespace TestBackendSERU.IService
{
    public interface IVehicleModelRepository
    {
        Task<Default> Add(Model.AddModel data);
        Task<Default> Edit(Model.EditModel data);
        Task<Default> Delete(Model.DeleteModel data);
        Task<GetAllModel> GetModel(int limit, int offset);
        Task<Default> GetModelById(int id);
    }
}
