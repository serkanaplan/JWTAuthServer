using Microsoft.AspNetCore.Mvc;
using JWTAuthServer.Core.DTOs;
using JWTAuthServer.Core.Services;

namespace JWTAuthServer.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController(IAuthenticationService authenticationService) : CustomBaseController
{
    private readonly IAuthenticationService _authenticationService = authenticationService;

    //api/auth/
    [HttpPost]
    public async Task<IActionResult> CreateToken(LoginDto loginDto)
    {
        var result = await _authenticationService.CreateTokenAsync(loginDto);

        return ActionResultInstance(result);
    }

    [HttpPost]
    public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var result = _authenticationService.CreateTokenByClient(clientLoginDto);

        return ActionResultInstance(result);
    }

    [HttpPost]
    public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        var result = await _authenticationService.RevokeRefreshToken(refreshTokenDto.Token);

        return ActionResultInstance(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)

    {
        var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.Token);

        return ActionResultInstance(result);
    }
}