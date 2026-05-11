using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControleEstoqueApi.Data;
using ControleEstoqueApi.Models;
using ControleEstoqueApi.DTOs.Estoquista;

namespace ControleEstoqueApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EstoquistasController : ControllerBase {
    private readonly AppDbContext _context;

    public EstoquistasController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EstoquistaDto>>> GetAllAsync() {
        var estoquistas = await _context.Estoquistas.AsNoTracking().ToListAsync();
        var result = estoquistas.Select(e => new EstoquistaDto {
            Id = e.Id, Nome = e.Nome, Email = e.Email, Idade = e.Idade
        });
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetEstoquistaById")]
    public async Task<ActionResult<EstoquistaDto>> GetByIdAsync(int id) {
        var e = await _context.Estoquistas.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        if (e is null) return NotFound();
        return Ok(new EstoquistaDto { Id = e.Id, Nome = e.Nome, Email = e.Email, Idade = e.Idade });
    }

    // POST → 201 Created
    [HttpPost]
    public async Task<ActionResult<EstoquistaDto>> CreateAsync(EstoquistaCreateDto dto) {
        var estoquista = new Estoquista { Nome = dto.Nome, Email = dto.Email, Idade = dto.Idade };
        _context.Estoquistas.Add(estoquista);
        await _context.SaveChangesAsync();
        return CreatedAtRoute("GetEstoquistaById", new { id = estoquista.Id }, new { id = estoquista.Id });
    }

    // PUT → 204 No Content
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, EstoquistaUpdateDto dto) {
        if (id != dto.Id) return BadRequest("Id da URL não confere com o Id do corpo.");
        var estoquista = await _context.Estoquistas.FindAsync(id);
        if (estoquista is null) return NotFound();
        estoquista.Nome = dto.Nome; estoquista.Email = dto.Email; estoquista.Idade = dto.Idade;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE → 204 No Content
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id) {
        var estoquista = await _context.Estoquistas.FindAsync(id);
        if (estoquista is null) return NotFound();
        _context.Estoquistas.Remove(estoquista);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
