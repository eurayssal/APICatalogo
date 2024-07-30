using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;
using X.PagedList;

namespace APICatalogo.Repositories.Implementations
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        //public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
        //{
        //    return GetAll()
        //        .OrderBy(x => x.Nome)
        //        .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
        //        .Take(produtosParameters.PageSize)
        //        .ToList();
        //}

        public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
        {
            var produtos = await GetAllAsync();

            var categoriasOrdenadas = produtos.OrderBy(p => p.ProdutoId)
                                              .AsQueryable();

            var resultdo = await categoriasOrdenadas.ToPagedListAsync(produtosParameters.PageNumber, produtosParameters.PageSize);

            return resultdo;
        }

        public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco)
        {
            var produtos = await GetAllAsync();

            if (produtosFiltroPreco.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroPreco.PrecoCriterio))
            {
                if (produtosFiltroPreco.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroPreco.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroPreco.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroPreco.Preco.Value).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroPreco.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroPreco.Preco.Value).OrderBy(p => p.Preco);
                }
            }
            var produtosFiltrados = await produtos.ToPagedListAsync(produtosFiltroPreco.PageNumber, produtosFiltroPreco.PageSize);

            return produtosFiltrados;
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
        {
            var produtos = await GetAllAsync();

            var produtosPorCategoria = produtos.Where(c => c.CategoriaId == id);

            return produtosPorCategoria;
        }
    }
}
