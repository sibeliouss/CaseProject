using Core.DataAccess.EfEntityRepositoryBase;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;

namespace DataAccess.Concrete;

public class EfTaskDal : EfEntityRepositoryBase<TaskEnt, AppDbContext>, ITaskDal
{
    public EfTaskDal(AppDbContext context) : base(context)
    {
    }
}