using Microsoft.EntityFrameworkCore;
using JWTAuthServer.Core.UnitOfWork;

namespace JWTAuthServer.Data;
public class UnitOfWork(AppDbContext appDbContext) : IUnitOfWork
{
    private readonly DbContext _context = appDbContext;

    public void Commit() => _context.SaveChanges();

    public async Task CommmitAsync() => await _context.SaveChangesAsync();
}