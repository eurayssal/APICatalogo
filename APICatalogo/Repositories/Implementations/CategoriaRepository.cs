﻿using APICatalogo.Context;
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

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
        {
            var categorias = GetAll()
                            .OrderBy(o => o.CategoriaId)
                            .AsQueryable();

            var categoriasOrdenadas = PagedList<Categoria>.ToPagedList(categorias, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriasOrdenadas;
        }

        public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias = GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(categoriasFiltroNome.Nome))
            {
                categorias = categorias.Where(w => w.Nome.Contains(categoriasFiltroNome.Nome));
            }

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias, categoriasFiltroNome.PageSize, categoriasFiltroNome.PageNumber);

            return categoriasFiltradas;
        }
    }
}
