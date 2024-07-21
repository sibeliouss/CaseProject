using System.Linq.Expressions;

namespace Core.DataAccess;

public interface IEntityRepository<T> where T : class
{
    T Get(Expression<Func<T, bool>> filter);
    List<T> GetAll(Expression<Func<T, bool>>? filter = null);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}