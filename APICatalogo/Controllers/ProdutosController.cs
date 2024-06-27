﻿using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            var produtos = _context.Produtos.AsNoTracking().ToList();
            if (produtos is null) { return NotFound("Produtos não encontrados."); }
            return produtos;
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var findProduto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (findProduto is null) { return NotFound("Produto não encontrado."); }
            return findProduto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var findProduto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (findProduto is null)
                return NotFound("Produto não encontrado.");

            _context.Produtos.Remove(findProduto);
            _context.SaveChanges();

            return Ok(findProduto);
        }
    }
}
