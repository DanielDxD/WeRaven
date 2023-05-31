using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using WeRaven.Api.Models;
using WeRaven.Api.Models.EmailTemplates;
using WeRaven.Api.Models.Flat;
using WeRaven.Api.Models.Requests;
using WeRaven.Api.Models.Responses;
using WeRaven.Api.Repositories.Interfaces;
using WeRaven.Api.Services.Interfaces;

namespace WeRaven.Api.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [SwaggerResponse(201, "User created", typeof(UserFlat))]
        [SwaggerResponse(400, "Email or username in use", typeof(ErrorResponse))]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest model, [FromServices] IEmailService emailService)
        {
            if(await _repository.ExistEmail(model.Email))
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "This email is already in use"
                });
            }
            if(await _repository.ExistUsername(model.Username))
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "This username is already in use"
                });
            }

            var user = new User
            {
                Username = model.Username,
                Name = model.Name,
                Email = model.Email,
                Password = Argon2.Hash(model.Password),
                Birthdate = DateTime.Parse(model.Birthdate, CultureInfo.InvariantCulture).ToUniversalTime(),
                Gender = model.Gender
            };

            var auth = new Auth
            {
                UserId = user.Id
            };

            await _repository.CreateUserAsync(user);
            await _repository.CreateAuthAsync(auth);

            await _repository.SaveAsync();

            await emailService.CompileAsync<VerifyEmailTemplate>("Verify", new()
            {
                Name = user.Name,
                Code = auth.Code
            });

            emailService.Send(user.Name, user.Email, "Verify your email");

            return Created($"https://api.weraven.net/v1/users/{user.Id}", user);
        }
        [HttpGet("re-send")]
        [SwaggerResponse(200, "Successfully email sended")]
        [SwaggerResponse(404, "User not found", typeof(ErrorResponse))]
        public async Task<IActionResult> Resend([FromQuery] string email, [FromServices] IEmailService emailService)
        {
            var user = await _repository.GetUserAsync(email);
            if(user == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "User not found"
                });
            }

            var auth = await _repository.GetAuthAsync(user.Id);
            if(auth == null)
            {
                auth = new Auth
                {
                    UserId = user.Id
                };

                await _repository.CreateAuthAsync(auth);
                await _repository.SaveAsync();
            }

            await emailService.CompileAsync<VerifyEmailTemplate>("Verify", new()
            {
                Name = user.Name,
                Code = auth.Code
            });

            emailService.Send(user.Name, user.Email, "Verify your email");

            return Ok();
        }

        [HttpPost("verify")]
        [SwaggerResponse(200, "Successfully user verified", typeof(LoginResponse))]
        [SwaggerResponse(400, "Expired authentication", typeof(ErrorResponse))]
        [SwaggerResponse(401, "Invalid code", typeof(ErrorResponse))]
        [SwaggerResponse(404, "User not found", typeof(ErrorResponse))]
        [SwaggerResponse(406, "No authentication request", typeof(ErrorResponse))]
        public async Task<IActionResult> Verify([FromBody] VerifyRequest model, [FromServices] ITokenService tokenService)
        {
            var user = await _repository.GetUserAsync(model.Email, asNoTracking: false);
            if (user == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "User not found"
                });
            }

            var auth = await _repository.GetAuthAsync(user.Id);
            if(auth == null)
            {
                return StatusCode(406, new ErrorResponse
                {
                    Message = "Can't find an authentication request"
                });
            }

            var currentTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            var expiryTimestamp = new DateTimeOffset(auth.ExpiryAt).ToUnixTimeMilliseconds();

            if(currentTimestamp > expiryTimestamp)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Expired authentication request."
                });
            }

            if(auth.Code != model.Code)
            {
                return Unauthorized(new ErrorResponse
                {
                    Message = "Invalid code."
                });
            }

            user.Confirmed = true;

            _repository.UpdateUser(user);
            _repository.RemoveAuth(auth);

            await _repository.SaveAsync();

            return Ok(new LoginResponse
            {
                Token = tokenService.GenerateToken(user)
            });
        }

        [HttpPost("login")]
        [SwaggerResponse(200, "Sucess logged in", typeof(LoginResponse))]
        [SwaggerResponse(400, "Invalid email, username or password", typeof(ErrorResponse))]
        public async Task<IActionResult> Login([FromBody] LoginRequest model, [FromServices] ITokenService tokenService)
        {
            var user = await _repository.GetUserAsync(model.EmailOrUsername);
            if (user == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid username or password"
                });
            }

            if(!Argon2.Verify(user.Password, model.Password))
            {
                return BadRequest(new ErrorResponse
                {
                    Message = "Invalid username or password"
                });
            }

            return Ok(new LoginResponse
            {
                Token = tokenService.GenerateToken(user)
            });
        }

        [HttpGet]
        [Authorize]
        [SwaggerResponse(200, "Success", typeof(UserFlat))]
        [SwaggerResponse(401, "Unauthorized", typeof(ErrorResponse))]
        public async Task<IActionResult> Get()
        {
            var user = await _repository.GetUserAsync(User.Identity.Name);
            if(user == null)
            {
                return Unauthorized(new ErrorResponse
                {
                    Message = "Can't identify your account."
                });
            }

            return Ok(user);
        }
    }
}
