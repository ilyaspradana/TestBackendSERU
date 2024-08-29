using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static TestBackendSERU.Models.Response;
using static TestBackendSERU.Models.User;

namespace TestBackendSERU.IService
{
    public interface IUserRepository
    {
        DtoLogin Login([FromBody] Login userLogin);

        Task<Registration> Registration(Registration user);

        Task<Default> Edit(EditUser user);
    }
}