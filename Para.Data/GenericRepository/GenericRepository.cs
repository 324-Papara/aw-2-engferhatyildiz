using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Para.Base.Entity;
using Para.Data.Context;

namespace Para.Data.GenericRepository;

public class GenericRepository<TEntity>(ParaDbContext dbContext) : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    public async Task Save()
    {
        await dbContext.SaveChangesAsync();
    }

    public async Task<TEntity?> GetById(long Id)
    {
        return await dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == Id);
    }

    public async Task Insert(TEntity entity)
    {
        entity.IsActive = true;
        entity.InsertDate = DateTime.UtcNow;
        entity.InsertUser = "System";
        await dbContext.Set<TEntity>().AddAsync(entity);
    }

    public Task Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
        return Task.CompletedTask;
    }

    public Task Delete(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public async Task Delete(long Id)
    {
        var entity = await dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == Id);
        if(entity is not null)
            dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<List<TEntity>> GetAll()
    {
       return await dbContext.Set<TEntity>().ToListAsync();
    }
    
    public async Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return await dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> queryable = dbContext.Set<TEntity>();

        return includes.Aggregate(queryable, (current, include) => current.Include(include));
    }
}