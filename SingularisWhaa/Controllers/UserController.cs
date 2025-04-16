using FluentValidation;
using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using SingularisWhaa.Models.User;
using SingularisWhaa.Services.Abstractions;

namespace SingularisWhaa.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserCollectionService userCollection;
        private readonly IValidator<UserDto> userValidator;

        public UserController(ILogger<UserController> logger, IUserCollectionService userCollection, IValidator<UserDto> userValidator)
        {
            this.logger = logger;
            this.userCollection = userCollection;
            this.userValidator = userValidator;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto, [FromServices] IEmailService emailService)
        {
            var validationResult = await userValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);

                return ValidationProblem(ModelState);
            }

            if (!await userCollection.CheckEmailUnique(userDto.Email!))
            {
                ModelState.AddModelError("Email", "Дубликат email'а!");
                return ValidationProblem(ModelState);
            }

            var createdUser = await userCollection.Add(userDto);

            if (createdUser is null)
            {
                logger.LogError("Пользователь ()");
                return Problem();
            }

            await emailService.SendEmail(createdUser, "welcome.liquid");

            return Created();
        }
    }
}
