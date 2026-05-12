// Controle de Produtos
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControleEstoqueApi.Data;
using ControleEstoqueApi.Models;
using ControleEstoqueApi.DTOs.Produto;

namespace ControleEstoqueApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase {
    private readonly AppDbContext _context;

    public ProdutosController(AppDbContext context) {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProdutoDto>>>> GetAllAsync() {
        var produtos = await _context.Produtos
            .AsNoTracking()
            .Include(p => p.TipoProduto)
            .ToListAsync();
        var result = produtos.Select(p => new ProdutoDto {
            Id = p.Id, 
            Nome = p.Nome, 
            Preco = p.Preco, 
            Estoque = p.Estoque,
            TipoProdutoId = p.TipoProdutoId,
            TipoProdutoNome = p.TipoProduto?.Nome ?? string.Empty
        });
        return Ok(ApiResponse<IEnumerable<ProdutoDto>>.Ok(result, "Produtos recuperados com sucesso."));
    }

    [HttpGet("{id:int}", Name = "GetProdutoById")]
    public async Task<ActionResult<ApiResponse<ProdutoDto>>> GetByIdAsync(int id) {
        var produto = await _context.Produtos
            .AsNoTracking()
            .Include(p => p.TipoProduto)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (produto is null) 
            return NotFound(ApiResponse<ProdutoDto>.NotFound($"Produto com id {id} não encontrado."));
        
        return Ok(ApiResponse<ProdutoDto>.Ok(new ProdutoDto { 
            Id = produto.Id, 
            Nome = produto.Nome, 
            Preco = produto.Preco, 
            Estoque = produto.Estoque,
            TipoProdutoId = produto.TipoProdutoId,
            TipoProdutoNome = produto.TipoProduto?.Nome ?? string.Empty
        }));
    }

    // POST → 201 Created
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProdutoDto>>> CreateAsync(ProdutoCreateDto dto) {
        // Validar se TipoProduto existe
        var tipoProduto = await _context.TiposProduto.FindAsync(dto.TipoProdutoId);
        if (tipoProduto is null) 
            return BadRequest(ApiResponse<ProdutoDto>.BadRequest("TipoProdutoId inválido."));

        var produto = new Produto { 
            Nome = dto.Nome, 
            Preco = dto.Preco, 
            Estoque = dto.Estoque,
            TipoProdutoId = dto.TipoProdutoId
        };
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        var produtoDto = new ProdutoDto {
            Id = produto.Id,
            Nome = produto.Nome,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            TipoProdutoId = produto.TipoProdutoId,
            TipoProdutoNome = tipoProduto.Nome
        };

        return CreatedAtRoute("GetProdutoById", new { id = produto.Id }, ApiResponse<ProdutoDto>.Ok(produtoDto, "Produto criado com sucesso."));
    }

    // PUT → 204 No Content
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, ProdutoUpdateDto dto) {
        if (id != dto.Id) 
            return BadRequest(ApiResponse<object>.BadRequest("Id da URL não confere com o Id do corpo."));
        
        // Validar se TipoProduto existe
        var tipoProduto = await _context.TiposProduto.FindAsync(dto.TipoProdutoId);
        if (tipoProduto is null) 
            return BadRequest(ApiResponse<object>.BadRequest("TipoProdutoId inválido."));
        
        var produto = await _context.Produtos.FindAsync(id);
        if (produto is null) 
            return NotFound(ApiResponse<object>.NotFound($"Produto com id {id} não encontrado."));
        
        produto.Nome = dto.Nome; 
        produto.Preco = dto.Preco; 
        produto.Estoque = dto.Estoque;
        produto.TipoProdutoId = dto.TipoProdutoId;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE → 204 No Content
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id) {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto is null) 
            return NotFound(ApiResponse<object>.NotFound($"Produto com id {id} não encontrado."));
        
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
