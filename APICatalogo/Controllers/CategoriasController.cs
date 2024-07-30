using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnityOfWork _uof; //Injeção da interface IRepository categoria
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(IUnityOfWork uof,
        ILogger<CategoriasController> logger)
    {
        _uof = uof;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CategoriaDTO>> Get()
    {
        var categorias = _uof.CategoriaRepository.GetAllAsync();

        //Trecho substituido por => var categoriasDto = categorias.ToCategoriaDTOList();
        //var categoriasDto = new List<CategoriaDTO>();
        //foreach (var categoria in categorias)
        //{
        //    var categoriaDto = new CategoriaDTO
        //    {
        //        CategoriaId = categoria.CategoriaId,
        //        Nome = categoria.Nome,
        //        ImagemUrl = categoria.ImagemUrl,
        //    };
        //    categoriasDto.Add(categoriaDto);
        //}

        var categoriasDto = categorias.ToCategoriaDTOList();
        return Ok(categoriasDto);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> Get([FromQuery] CategoriasParameters categoriasParameters)
    {
        var categorias = _uof.CategoriaRepository.GetCategorias(categoriasParameters);

        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.HasNext,
            categorias.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var categoriasDto = categorias.ToCategoriaDTOList();

        return Ok(categoriasDto);
    }

    [HttpGet("filter/nome/pagination")]
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasFiltradas(
                                 [FromQuery] CategoriasFiltroNome categoriasFiltro)
    {
        var categoriasFiltradas = _uof.CategoriaRepository
                                     .GetCategoriasFiltroNome(categoriasFiltro);

        return ObterCategorias(categoriasFiltradas);
    }
    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(PagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.TotalCount,
            categorias.PageSize,
            categorias.CurrentPage,
            categorias.TotalPages,
            categorias.HasNext,
            categorias.HasPrevious
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
        var categoriasDto = categorias.ToCategoriaDTOList();
        return Ok(categoriasDto);
    }


    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> Get(int id)
    {
        var categoria = _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id= {id} não encontrada...");
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        //Trecho substituido por => var categoriaDto = categoria.ToCategoriaDTO();
        //var categoriaDto = new CategoriaDTO()
        //{
        //    CategoriaId = categoria.CategoriaId,
        //    Nome = categoria.Nome,
        //    ImagemUrl = categoria.ImagemUrl
        //};
        var categoriaDto = categoria.ToCategoriaDTO();

        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
    {
        if (categoriaDto is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        //Trecho substituido por => var categoria = categoriaDto.ToCategoria();
        //var categoria = new Categoria()
        //{
        //    CategoriaId = categoriaDto.CategoriaId,
        //    Nome = categoriaDto.Nome,
        //    ImagemUrl = categoriaDto.ImagemUrl
        //};

        var categoria = categoriaDto.ToCategoria();

        var newCategoria = _uof.CategoriaRepository.Create(categoria);
        _uof.Commit();

        //Trecho substituido por => var newCategoriaDto = newCategoria.ToCategoriaDTO();
        //var newCategoriaDto = new CategoriaDTO()
        //{
        //    CategoriaId = newCategoria.CategoriaId,
        //    Nome = newCategoria.Nome,
        //    ImagemUrl = newCategoria.ImagemUrl
        //};
        var newCategoriaDto = newCategoria.ToCategoriaDTO();


        return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, newCategoria);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
    {
        if (id != categoriaDto.CategoriaId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        //Trecho substituido por =>  var categoria = categoriaDto.ToCategoria();
        //var categoria = new Categoria()
        //{
        //    CategoriaId = categoriaDto.CategoriaId,
        //    Nome = categoriaDto.Nome,
        //    ImagemUrl = categoriaDto.ImagemUrl
        //};
        var categoria = categoriaDto.ToCategoria();

        var categoriaUpdate = _uof.CategoriaRepository.Update(categoria);
        _uof.Commit();

        //var categoriaUpdateDto = new Categoria()
        //{
        //    CategoriaId = categoriaUpdate.CategoriaId,
        //    Nome = categoriaUpdate.Nome,
        //    ImagemUrl = categoriaUpdate.ImagemUrl
        //};
        var categoriaUpdateDto = categoriaUpdate.ToCategoriaDTO();

        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> Delete(int id)
    {
        var categoria = _uof.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

        if (categoria == null)
        {
            _logger.LogWarning($"Categoria com id={id} não encontrada...");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _uof.CategoriaRepository.Delete(categoria);
        _uof.Commit();

        //var categoriaExcluidaDto = new CategoriaDTO()
        //{
        //    CategoriaId = categoriaExcluida.CategoriaId,
        //    Nome = categoriaExcluida.Nome,
        //    ImagemUrl = categoriaExcluida.ImagemUrl
        //};
        var categoriaExcluidaDto = categoriaExcluida.ToCategoriaDTO();

        return Ok(categoriaExcluida);
    }
}