using FutureProjects.Application.Abstractions.IServices;
using FutureProjects.Domain.Entities.DTOs;
using FutureProjects.Domain.Entities.Models;
using FutureProjects.Domain.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FutureProjects.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAllUsers()
            {
            try
            {
               
                _logger.LogInformation("Test Logs");


                var result = await _userService.GetAll();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<User>>> CreateUser(UserDTO model)
        {
            var result = await _userService.Create(model);

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<User>> UpdateUser(int id, UserDTO model)
        {
            var result = await _userService.Update(id, model);

            return Ok(result);
        }

    }
}
