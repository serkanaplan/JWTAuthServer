using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using JWTAuthServer.Core.DTOs;
using JWTAuthServer.Core.Models;
using JWTAuthServer.Core.Services;

namespace JWTAuthServer.Service.Services;
public class UserService(UserManager<UserApp> userManager) : IUserService
{
    private readonly UserManager<UserApp> _userManager = userManager;

    public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = new UserApp { Email = createUserDto.Email, UserName = createUserDto.UserName };

        var result = await _userManager.CreateAsync(user, createUserDto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();

            return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
        }
        return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
    }

    public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
        {
            return Response<UserAppDto>.Fail("UserName not found", 404, true);
        }

        return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
    }
}