using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SalesDashBoardApplication.Models;
using SalesDashBoardApplication.Models.DTO.UserDto;
using SalesDashBoardApplication.Services.Contracts;

namespace SalesDashBoardApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }


        //[HttpPost("register")]
        //public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        //{
        //    var user = new User
        //    {
        //        UserName = userRegisterDto.UserName,
        //        UserEmail = userRegisterDto.UserEmail,
        //        UserPassword = userRegisterDto.UserPassword,
        //        UserRole = userRegisterDto.UserRole
        //    };

        //    await _userService.RegisterUser(user);
        //    return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        //}       


        //[HttpPost("login")]
        //public async Task<IActionResult> Login(UserLoginDto loginDto)
        //{
        //    var user = await _userService.Login(loginDto.UserEmail, loginDto.UserPassword);
        //    if (user == null)
        //        return Unauthorized();
        //    var response = new UserLoginResponseDto
        //    {
        //        UserName = user.UserName,                                                       
        //        UserEmail = user.UserEmail,
        //        UserRole = user.UserRole
        //    };
        //    return Ok(response);    
        //}


        //[HttpGet]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await _userService.GetAllUsers();
        //    return Ok(users);
        //}


        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetUserById(int id)
        //{
        //    var user = await _userService.GetUserById(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}


        //[HttpPost("{id}/change-password")]
        //public async Task<IActionResult> ChangePassword(int id, UserPasswordChangeDto userPasswordChangeDto)
        //{
        //    await _userService.ChangePassword(id, userPasswordChangeDto.UserPassword);
        //    return Created();
        //}


        //[HttpPatch("{id}")]
        //public async Task<IActionResult> UpdateUser(int id, JsonPatchDocument<UserUpdateDto> patchDocument)
        //{
        //    if (patchDocument == null)
        //        return BadRequest();        

        //    var existingUser = await _userService.GetUserById(id);

        //    if (existingUser == null)
        //        return NotFound();

        //    var userUpdateDto = new UserUpdateDto
        //    {
        //        UserName = existingUser.UserName,
        //        UserEmail = existingUser.UserEmail
        //    };
             
        //    patchDocument.ApplyTo(userUpdateDto, ModelState);       

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    existingUser.UserName = userUpdateDto.UserName;
        //    existingUser.UserEmail = userUpdateDto.UserEmail;             

        //    await _userService.UpdateUser(existingUser);
        //    return NoContent();
        //}







        [HttpPost("register")]
        public async Task Register(UserRegisterDto userRegisterDto)
        {
            var user = new User
            {
                UserName = userRegisterDto.UserName,
                UserEmail = userRegisterDto.UserEmail,
                UserPassword = userRegisterDto.UserPassword,
                UserRole = userRegisterDto.UserRole
            };

            _logger.LogInformation("Registering a new user");
            await _userService.RegisterUser(user);
        }



        [HttpPost("login")]
        public async Task<UserWithTokenDto> Login(UserLoginDto loginDto)
        {
            _logger.LogInformation("Logging in the user");
            return await _userService.Login(loginDto.UserEmail, loginDto.UserPassword);
        }



        [HttpGet]
        public async Task<IEnumerable<UserGetDto>> GetAllUsers()
        {
            _logger.LogInformation("Getting all user data");
            return await _userService.GetAllUsers();
        }



        [HttpGet("{id}")]
        public async Task<User> GetUserById(int id)
        {
            _logger.LogInformation("Getting user data by id");
            return await _userService.GetUserById(id);
        }



        [HttpPost("{id}/change-password")]
        public async Task ChangePassword(int id, UserPasswordChangeDto userPasswordChangeDto)
        {
            _logger.LogInformation("Changing the password of the user");
            await _userService.ChangePassword(id, userPasswordChangeDto.UserPassword);
        }



        [HttpPatch("{id}")]
        public async Task UpdateUser(int id, JsonPatchDocument<UserUpdateDto> patchDocument)
        {
            if (patchDocument == null)
                throw new ApplicationException("Could not update user try again later"); 

            var existingUser = await _userService.GetUserById(id);

            if (existingUser == null)
                throw new ApplicationException("Could not fetch user data to update user try again later");

            var userUpdateDto = new UserUpdateDto
            {
                UserName = existingUser.UserName,
                UserEmail = existingUser.UserEmail
            };

            patchDocument.ApplyTo(userUpdateDto, ModelState);

            if (!ModelState.IsValid)
                throw new ApplicationException("Could not update user try again later after sometimes");

            existingUser.UserName = userUpdateDto.UserName;
            existingUser.UserEmail = userUpdateDto.UserEmail;

            _logger.LogInformation("Updating the user details");
            await _userService.UpdateUser(existingUser);
        }

    }
}
