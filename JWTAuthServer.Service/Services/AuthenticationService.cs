﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using JWTAuthServer.Core.Configuration;
using JWTAuthServer.Core.DTOs;
using JWTAuthServer.Core.Models;
using JWTAuthServer.Core.Repositories;
using JWTAuthServer.Core.Services;
using JWTAuthServer.Core.UnitOfWork;

namespace JWTAuthServer.Service.Services;
public class AuthenticationService(IOptions<List<Client>> optionsClient, ITokenService tokenService, UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshTokenService) : IAuthenticationService
{
    private readonly List<Client> _clients = optionsClient.Value;
    private readonly ITokenService _tokenService = tokenService;
    private readonly UserManager<UserApp> _userManager = userManager;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService = userRefreshTokenService;

    public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
    {
        if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user == null) return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);

        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);
        }
        var token = _tokenService.CreateToken(user);

        var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

        if (userRefreshToken == null)
        {
            await _userRefreshTokenService.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
        }
        else
        {
            userRefreshToken.Code = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
        }

        await _unitOfWork.CommmitAsync();

        return Response<TokenDto>.Success(token, 200);
    }

    public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

        if (client == null)
        {
            return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);
        }

        var token = _tokenService.CreateTokenByClient(client);

        return Response<ClientTokenDto>.Success(token, 200);
    }

    public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
    {
        var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

        if (existRefreshToken == null)
        {
            return Response<TokenDto>.Fail("Refresh token not found", 404, true);
        }

        var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);

        if (user == null)
        {
            return Response<TokenDto>.Fail("User Id not found", 404, true);
        }

        var tokenDto = _tokenService.CreateToken(user);

        existRefreshToken.Code = tokenDto.RefreshToken;
        existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

        await _unitOfWork.CommmitAsync();

        return Response<TokenDto>.Success(tokenDto, 200);
    }

    public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
    {
        var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
        if (existRefreshToken == null)
        {
            return Response<NoDataDto>.Fail("Refresh token not found", 404, true);
        }

        _userRefreshTokenService.Remove(existRefreshToken);

        await _unitOfWork.CommmitAsync();

        return Response<NoDataDto>.Success(200);
    }
}