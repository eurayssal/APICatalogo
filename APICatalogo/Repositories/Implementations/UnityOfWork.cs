using APICatalogo.Context;
using APICatalogo.Repositories.Interfaces;

namespace APICatalogo.Repositories.Implementations
{
    public class UnityOfWork : IUnityOfWork
    {
        private IProdutoRepository? _produtoRepo;
        private ICategoriaRepository? _categoriaRepo;

        public AppDbContext _context;

        public UnityOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produtoRepo = _produtoRepo ?? new ProdutoRepository(_context);
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepo ?? new CategoriaRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
