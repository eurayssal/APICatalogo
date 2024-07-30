using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;

namespace APICatalogo.Repositories.Implementations
{
    //Herda de repositoru e implementa ICategoriaRepository
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        /*
         Qual o objetivo do repository?
        - Centralizar a lógica de acesso a dados.
        - Não deve ter a regra de negócio, somente o acesso ao dados.
         */
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await GetAllAsync();

            var categoriasOrdenadas = categorias.OrderBy(o => o.CategoriaId)
                                                .AsQueryable();


            var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return resultado;
        }

        public async Task<PagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias = await GetAllAsync();

            if (!string.IsNullOrEmpty(categoriasFiltroNome.Nome))
            {
                categorias = categorias.Where(w => w.Nome.Contains(categoriasFiltroNome.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasFiltroNome.PageSize, categoriasFiltroNome.PageNumber);

            return categoriasFiltradas;
        }
    }
}
