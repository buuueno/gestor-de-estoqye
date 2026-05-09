using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControleEstoqueApi.Data;
using ControleEstoqueApi.Models;
using ControleEstoqueApi.DTOs.TipoProduto;

namespace ControleEstoqueApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TiposProdutoController : ControllerBase {
    private readonly AppDbContext _context;

    public TiposProdutoController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TipoProdutoDto>>> GetAllAsync() {
        var tipos = await _context.TiposProduto.AsNoTracking().ToListAsync();
        var result = tipos.Select(t => new TipoProdutoDto { Id = t.Id, Nome = t.Nome, Descricao = t.Descricao });
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "GetTipoProdutoById")]
    public async Task<ActionResult<TipoProdutoDto>> GetByIdAsync(int id) {
        var tipo = await _context.TiposProduto.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (tipo is null) return NotFound();
        return Ok(new TipoProdutoDto { Id = tipo.Id, Nome = tipo.Nome, Descricao = tipo.Descricao });
    }

    // POST → 201 Created
    [HttpPost]
    public async Task<ActionResult<TipoProdutoDto>> CreateAsync(TipoProdutoCreateDto dto) {
        var tipo = new TipoProduto { Nome = dto.Nome, Descricao = dto.Descricao };
        _context.TiposProduto.Add(tipo);
        await _context.SaveChangesAsync();
        return CreatedAtRoute("GetTipoProdutoById", new { id = tipo.Id }, new { id = tipo.Id });
    }

    // PUT → 204 No Content
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, TipoProdutoUpdateDto dto) {
        if (id != dto.Id) return BadRequest("Id da URL não confere com o Id do corpo.");
        var tipo = await _context.TiposProduto.FindAsync(id);
        if (tipo is null) return NotFound();
        tipo.Nome = dto.Nome; tipo.Descricao = dto.Descricao;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE → 204 No Content
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id) {
        var tipo = await _context.TiposProduto.FindAsync(id);
        if (tipo is null) return NotFound();
        _context.TiposProduto.Remove(tipo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
