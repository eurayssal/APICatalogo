using APICatalogo.Context;
using APICatalogo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace APICatalogo.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                                 .AsNoTracking()
                                 .ToListAsync();
            /*
             AsNoTracking => Faz com que o estado nao seja mais gerenciado na memoria
            Se eu precisar de fazer algo após a exclusão (mudança de estado) nao posso usar isso
            */
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            //_context.SaveChanges(); // Persistencia
            return entity;
        }
        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            //_context.SaveChanges();
            return entity;
        }

        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            //_context.SaveChanges();
            return entity;
        }

    }
}
