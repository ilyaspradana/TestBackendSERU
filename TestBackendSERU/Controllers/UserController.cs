using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using TestBackendSERU.IService;
using TestBackendSERU.Service;
using static TestBackendSERU.Models.User;

namespace TestBackendSERU.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login userLogin)
        {
            var r = new Models.Response.Default();

            if (userLogin == null || string.IsNullOrEmpty(userLogin.Username))
            {
                r.Status = "Failed";
                r.Message = "Harap isi Username";

                return Ok(r);
            }

            var User = _userRepository.Login(userLogin);

            if(User == null)
            {
                r.Status = "Failed";
                r.Message = "Username tidak ditemukan";
            }
            else
            {
                r.Status = "Success";
                r.Message = User;
            }

            return Ok(r);
        }

        [HttpPost("registration")]
        public async Task<IActionResult> RegistrationAsync([FromBody] Registration user)
        {
            var r = new Models.Response.Default();

            if (user == null || string.IsNullOrEmpty(user.Username))
            {
                r.Status = "Failed";
                r.Message = "Harap isi Username";

                return Ok(r);
            }

            var data = await _userRepository.Registration(user);

            if (data == null)
            {
                r.Status = "Failed";
                r.Message = "Username sudah terdaftar";
            }
            else
            {
                r.Status = "Success";
                r.Message = "Username "+ data.Username+" berhasil terdaftar";
            }

            return Ok(r);
        }

        [HttpPost("edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser([FromBody] EditUser data)
        {
            var response = await _userRepository.Edit(data);

            return Ok(response);
        }
    }
}
