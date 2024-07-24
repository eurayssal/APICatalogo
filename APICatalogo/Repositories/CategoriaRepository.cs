using APICatalogo.Context;
using APICatalogo.Models;

namespace APICatalogo.Repositories
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
    }
}
