using APICatalogo.Models;
using APICatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    /* 
    Repository:
    Dois repositórios implementados pois no IProdutoRepository tem uma implementação específica.
    Não é necessário implementar o IRepository nesse caso pois o repositório específico (IProdutoRepository) também
    implementa o repositório genérico por padrão
     */

    private readonly IUnityOfWork _uof;
    private readonly ILogger<CategoriasController> _logger;

    public ProdutosController(IUnityOfWork uof, ILogger<CategoriasController> logger)
    {
        _uof = uof;
        _logger = logger;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
    {
        var produtos = _uof.ProdutoRepository.GetProdutosPorCategoria(id);

        if (produtos is null)
        {
            return NotFound();
        }
        return Ok(produtos);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _uof.ProdutoRepository.GetAll();
        if (produtos is null)
        {
            return NotFound();
        }
        return Ok(produtos);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _uof.ProdutoRepository.Get(c => c.CategoriaId == id); // Um critério foi definido para a busca do produto
        if (produto is null)
        {
            return NotFound("Produto não encontrado...");
        }
        return produto;
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
            return BadRequest();

        var novoProduto = _uof.ProdutoRepository.Create(produto);
        _uof.Commit();

        return new CreatedAtRouteResult("ObterProduto",
            new { id = produto.ProdutoId }, novoProduto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            return BadRequest(); //400
        }

        var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto == null)
        {
            _logger.LogWarning($"Produto com id={id} não encontrado...");
            return NotFound($"Produto com id={id} não encontrado...");
        }

        var produtoExcluido = _uof.ProdutoRepository.Delete(produto);
        _uof.Commit();


        return Ok(produtoExcluido);
    }
}