using System.Linq.Expressions;

namespace APICatalogo.Repositories.Interfaces
{
    //Repositório genérico
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);

        //Bebidas Updat e Delete nao acessam o banco de dados. Eles sao executados na memória por isso nao precisam de ser Async
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
