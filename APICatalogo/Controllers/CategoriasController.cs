﻿using APICatalogo.Models;
using APICatalogo.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IRepository<Categoria> _repository; //Injeção da interface IRepository categoria
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(IRepository<Categoria> repository, ILogger<CategoriasController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        var categorias = _repository.GetAll();
        return Ok(categorias);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> Get(int id)
    {
        var categoria = _repository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            _logger.LogWarning($"Categoria com id= {id} não encontrada...");
            return NotFound($"Categoria com id= {id} não encontrada...");
        }
        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        if (categoria is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var newCategoria = _repository.Create(categoria);

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, newCategoria);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria)
    {
        if (id != categoria.CategoriaId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        _repository.Update(categoria);
        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var categoria = _repository.Get(c => c.CategoriaId == id);

        if (categoria == null)
        {
            _logger.LogWarning($"Categoria com id={id} não encontrada...");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _repository.Delete(categoria);
        return Ok(categoriaExcluida);
    }
}