﻿using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;

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

        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            var produtos = GetAll().OrderBy(p => p.ProdutoId)
                                   .AsQueryable();

            var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);

            return produtosOrdenados;
        }

        public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroPreco)
        {
            var produtos = GetAll().AsQueryable();

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
            var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos, produtosFiltroPreco.PageNumber,
                                                                                                  produtosFiltroPreco.PageSize);
            return produtosFiltrados;
        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
    }
}
