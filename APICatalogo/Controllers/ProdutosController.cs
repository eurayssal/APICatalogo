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
    private readonly IProdutoRepository _produtoRepository;
    private readonly IRepository<Produto> _repository;
    private readonly ILogger<CategoriasController> _logger;

    public ProdutosController(IRepository<Produto> repository, IProdutoRepository produtoRepository)
    {
        _repository = repository;
        _produtoRepository = produtoRepository;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
    {
        var produtos = _produtoRepository.GetProdutosPorCategoria(id);

        if (produtos is null)
        {
            return NotFound();
        }
        return Ok(produtos);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _repository.GetAll();
        if (produtos is null)
        {
            return NotFound();
        }
        return Ok(produtos);
    }

    [HttpGet("{id}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _repository.Get(c => c.CategoriaId == id); // Um critério foi definido para a busca do produto
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

        var novoProduto = _repository.Create(produto);

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

        var produtoAtualizado = _produtoRepository.Update(produto);

        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _repository.Get(p => p.ProdutoId == id);

        if (produto == null)
        {
            _logger.LogWarning($"Produto com id={id} não encontrado...");
            return NotFound($"Produto com id={id} não encontrado...");
        }

        var produtoExcluido = _repository.Delete(produto);
        return Ok(produtoExcluido);

    }
}