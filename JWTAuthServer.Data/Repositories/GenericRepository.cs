using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using JWTAuthServer.Core.Repositories;

namespace JWTAuthServer.Data.Repositories;

public class GenericRepository<Tentity>(AppDbContext context) : IGenericRepository<Tentity> where Tentity : class
{
    private readonly DbContext _context = context;
    private readonly DbSet<Tentity> _dbSet = context.Set<Tentity>();


    public async Task AddAsync(Tentity entity) => await _dbSet.AddAsync(entity);

    public async Task<IEnumerable<Tentity>> GetAllAsync() => await _dbSet.ToListAsync();
    
    public void Remove(Tentity entity) => _dbSet.Remove(entity);

    public IQueryable<Tentity> Where(Expression<Func<Tentity, bool>> predicate) => _dbSet.Where(predicate);

    public async Task<Tentity> GetByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null) _context.Entry(entity).State = EntityState.Detached;
        return entity; 
    }


    public Tentity Update(Tentity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return entity;
    }

}