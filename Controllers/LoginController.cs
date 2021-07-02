using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CeluGamaSystem.Dtos;
using CeluGamaSystem.Models;
using CeluGamaSystem.Services;
using CeluGamaSystem.Exceptions;

namespace CeluGamaSystem.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<object>> Login(User postedUser)
        {
            User user = _service.tryLogin(postedUser);
            if (user == null)
            {
                // la excepción no está funcionando, la API responde 500 cuando se produce la excepción.
                // throw new LoginException($"User not found with those credentials", 404);
                return Unauthorized();
            }
            // todo:
            // _context.Entry(user).Collection(u => u).Load();

            JWToken token = _service.generateToken(user.Username);

            return Ok(token);
        }

        [Authorize]
        [HttpPut("{username}")]
        public async Task<ActionResult> ChangePassword(string Username, [FromBody] PasswordContainer Container)
        {
            try
            {
                _service.changePassword(new User() { Username = Username, Password = Container.Password });
                return NoContent();
            }
            catch (Exception Error)
            {
                throw new ChangePasswordException($"La contraseña no pudo ser modificada: {Error.Message}");
            }
        }
    }
}
