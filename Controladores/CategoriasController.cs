using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using parcial_1.DTOs;
using parcial_1.Modelos;
using parcial_1.Servicios;

namespace parcial_1.Controladores;

// Controlador de categorías: gestiona el CRUD de categorías de productos.
// Permite activar/desactivar categorías y consultar todas o por ID.
// Incluye endpoints: GET all, GET byId, POST create, PUT update, DELETE.
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categorias = await _categoriaService.GetAll();
        var dtos = categorias.Adapt<IEnumerable<CategoriaDto>>();
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _categoriaService.GetById(id);
        if (categoria == null) return NotFound();

        return Ok(categoria.Adapt<CategoriaDto>());
    }

    [HttpPost]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Create([FromBody] CategoriaCreateDto dto)
    {
        var categoria = dto.Adapt<Categoria>();
        var creado = await _categoriaService.Add(categoria);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado.Adapt<CategoriaDto>());
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Update(int id, [FromBody] CategoriaUpdateDto dto)
    {
        var categoria = await _categoriaService.GetById(id);
        if (categoria == null) return NotFound();

        categoria.ActualizarNombre(dto.Nombre);
        categoria.ActualizarDescripcion(dto.Descripcion);
        if (dto.Activa) categoria.Activar(); else categoria.Desactivar();

        await _categoriaService.Update(categoria);
        return Ok(categoria.Adapt<CategoriaDto>());
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var categoria = await _categoriaService.GetById(id);
        if (categoria == null) return NotFound();

        await _categoriaService.Delete(id);
        return NoContent();
    }
}
