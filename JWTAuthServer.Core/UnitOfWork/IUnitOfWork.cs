namespace JWTAuthServer.Core.UnitOfWork;

public interface IUnitOfWork
{
    Task CommmitAsync();

    void Commit();
}