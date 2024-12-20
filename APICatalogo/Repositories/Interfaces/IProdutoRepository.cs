﻿using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
        Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters);
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id);
        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco);
    }
}
