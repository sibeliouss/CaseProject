using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess.EfEntityRepositoryBase;

public class EfEntityRepositoryBase<T, TContext> : IEntityRepository<T>
    where T : class
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly DbSet<T> _dbSet;

    public EfEntityRepositoryBase(TContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public T Get(Expression<Func<T, bool>> filter)
    {
        return _dbSet.SingleOrDefault(filter);
    }

    public List<T> GetAll(Expression<Func<T, bool>>? filter = null)
    {
        return filter == null
            ? _dbSet.ToList()
            : _dbSet.Where(filter).ToList();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }
}