using JWTAuthServer.Core.Configuration;
using JWTAuthServer.Core.DTOs;
using JWTAuthServer.Core.Models;

namespace JWTAuthServer.Core.Services;

public interface ITokenService
{
    TokenDto CreateToken(UserApp userApp);

    ClientTokenDto CreateTokenByClient(Client client);
}
