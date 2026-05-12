// Controle de Tipos de Produto
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControleEstoqueApi.Data;
using ControleEstoqueApi.Models;
using ControleEstoqueApi.DTOs.TipoProduto;

namespace ControleEstoqueApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TiposProdutoController : ControllerBase {
    private readonly AppDbContext _context;

    public TiposProdutoController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<TipoProdutoDto>>>> GetAllAsync() {
        var tipos = await _context.TiposProduto.AsNoTracking().ToListAsync();
        var result = tipos.Select(t => new TipoProdutoDto { Id = t.Id, Nome = t.Nome, Descricao = t.Descricao });
        return Ok(ApiResponse<IEnumerable<TipoProdutoDto>>.Ok(result, "Tipos de produto recuperados com sucesso."));
    }

    [HttpGet("{id:int}", Name = "GetTipoProdutoById")]
    public async Task<ActionResult<ApiResponse<TipoProdutoDto>>> GetByIdAsync(int id) {
        var tipo = await _context.TiposProduto.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (tipo is null) 
            return NotFound(ApiResponse<TipoProdutoDto>.NotFound($"Tipo de produto com id {id} não encontrado."));
        
        return Ok(ApiResponse<TipoProdutoDto>.Ok(new TipoProdutoDto { Id = tipo.Id, Nome = tipo.Nome, Descricao = tipo.Descricao }));
    }

    // POST → 201 Created
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TipoProdutoDto>>> CreateAsync(TipoProdutoCreateDto dto) {
        var tipo = new TipoProduto { Nome = dto.Nome, Descricao = dto.Descricao };
        _context.TiposProduto.Add(tipo);
        await _context.SaveChangesAsync();

        var result = new TipoProdutoDto { Id = tipo.Id, Nome = tipo.Nome, Descricao = tipo.Descricao };
        return CreatedAtRoute("GetTipoProdutoById", new { id = tipo.Id }, ApiResponse<TipoProdutoDto>.Ok(result, "Tipo de produto criado com sucesso."));
    }

    // PUT → 204 No Content
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, TipoProdutoUpdateDto dto) {
        if (id != dto.Id) 
            return BadRequest(ApiResponse<object>.BadRequest("Id da URL não confere com o Id do corpo."));
        
        var tipo = await _context.TiposProduto.FindAsync(id);
        if (tipo is null) 
            return NotFound(ApiResponse<object>.NotFound($"Tipo de produto com id {id} não encontrado."));
        
        tipo.Nome = dto.Nome;
        tipo.Descricao = dto.Descricao;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE → 204 No Content
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id) {
        var tipo = await _context.TiposProduto.FindAsync(id);
        if (tipo is null) 
            return NotFound(ApiResponse<object>.NotFound($"Tipo de produto com id {id} não encontrado."));
        
        _context.TiposProduto.Remove(tipo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
