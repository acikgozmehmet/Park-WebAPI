using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Park.Models;
using Park.Repository.IRepository;

namespace Park.Controllers
{

    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)         
        { 
            var  user = _userRepository.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user.Token);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthenticationRegisterModel model)
        {
            bool IfUserNameUnique = _userRepository.IsUniqueUser(model.Username);
            if (IfUserNameUnique)
            {
                return BadRequest(new { message = "Username already exists" });
            }

            var user = _userRepository.Register(model.Username, model.Password, model.Role);

            if (user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }

            return Ok();
        }




    }
}
