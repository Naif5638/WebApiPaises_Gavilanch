using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPaises.Models;

namespace WebApiPaises.Controllers
{
    [Route("api/Pais/{paisId}/[controller]")]
    [ApiController]
    public class ProvinciaController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProvinciaController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Provincia> GetAll(int paisId)
        {
            return this.context.Provincias.Where(p => p.PaisId == paisId).ToList();
        }

        [HttpGet("{id}", Name = "provinciaById")]
        public IActionResult GetById(int id)
        {
            var pais = this.context.Provincias.FirstOrDefault(p => p.Id == id);

            if (pais == null) return NotFound();

            return new ObjectResult(pais);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Provincia provincia , int paisId)
        {
            provincia.PaisId = paisId;

            if (ModelState.IsValid)
            {
                this.context.Provincias.Add(provincia);
                this.context.SaveChanges();
                return new CreatedAtRouteResult("paisCreado", new { id = provincia.Id }, paisId);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Provincia provincia, int id)
        {
            if (provincia.Id != id)
            {
                return BadRequest();
            }

            this.context.Entry(provincia).State = EntityState.Modified;
            this.context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var provincia = this.context.Provincias.Find(id);

            if (provincia == null)
            {
                return BadRequest();
            }

            this.context.Remove(provincia);//.State = EntityState.Deleted;
            this.context.SaveChanges();
            return Ok(provincia);
        }
    }
}