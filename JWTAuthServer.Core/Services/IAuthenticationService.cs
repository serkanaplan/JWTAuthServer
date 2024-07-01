﻿using JWTAuthServer.Core.DTOs;
using SharedLibrary.Dtos;

namespace JWTAuthServer.Core.Services;
public interface IAuthenticationService
{
    Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);

    Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

    Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);

    Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
}