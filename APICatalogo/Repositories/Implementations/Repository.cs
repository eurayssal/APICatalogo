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

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking().ToList();
            /*
             AsNoTracking => Faz com que o estado nao seja mais gerenciado na memoria
            Se eu precisar de fazer algo após a exclusão (mudança de estado) nao posso usar isso
            */
        }

        public T? Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
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
