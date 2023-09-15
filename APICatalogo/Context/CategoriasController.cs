﻿using APICatalogo.Controllers;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace APICatalogo.Context
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _context.Categorias.AsNoTracking().Include(p => p.Produtos).ToList();
            if (categorias is null)
            {
                return NotFound();
            }
            return categorias;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias.AsNoTracking().ToList();
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("Categoria não encontrada...");
            }

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                return BadRequest();
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
               new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não encontrada");
            }
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
            return Ok(categoria);
        }

    }
}
