using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JWTAuthServer.Core.DTOs;
using JWTAuthServer.Core.Services;

namespace JWTAuthServer.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : CustomBaseController
{
    private readonly IUserService _userService = userService;

    //api/user
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto createUserDto) => ActionResultInstance(await _userService.CreateUserAsync(createUserDto));


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUser() => ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
}