using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using parcial_1.DTOs;
using parcial_1.Modelos;
using parcial_1.Servicios;

namespace parcial_1.Controladores;

// Controlador de productos: gestiona el CRUD de productos y consulta por categoría.
// Valida que la categoría exista antes de crear o actualizar productos.
// Incluye endpoints: GET all, GET byId, GET byCategoria, POST create, PUT update, DELETE.
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;
    private readonly ICategoriaService _categoriaService;
    private readonly IInventarioService _inventarioService;

    public ProductosController(IProductoService productoService, ICategoriaService categoriaService, IInventarioService inventarioService)
    {
        _productoService = productoService;
        _categoriaService = categoriaService;
        _inventarioService = inventarioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var productos = await _productoService.GetAll();
        var dtos = await MapearProductos(productos);
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var producto = await _productoService.GetById(id);
        if (producto == null) return NotFound();
        
        var dto = await MapearProducto(producto);
        return Ok(dto);
    }

    [HttpGet("categoria/{categoriaId}")]
    public async Task<IActionResult> GetByCategoria(int categoriaId)
    {
        var productos = await _productoService.GetByCategoria(categoriaId);
        var dtos = await MapearProductos(productos);
        return Ok(dtos);
    }

    [HttpGet("disponibilidad/{activo}")]
    public async Task<IActionResult> GetByDisponibilidad(bool activo)
    {
        var productos = await _productoService.GetByDisponibilidad(activo);
        var dtos = await MapearProductos(productos);
        return Ok(dtos);
    }

    [HttpGet("bajo-stock")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> GetBajoStock()
    {
        var inventarios = await _inventarioService.GetBajoStock();
        var productos = new List<ProductoDto>();
        foreach (var inv in inventarios)
        {
            var producto = await _productoService.GetById(inv.ProductoId);
            if (producto != null) productos.Add(await MapearProducto(producto));
        }
        return Ok(productos);
    }

    [HttpPost]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Create([FromBody] ProductoCreateDto dto)
    {
        var categoria = await _categoriaService.GetById(dto.CategoriaId);
        if (categoria == null) return BadRequest(new { mensaje = "Categoria no encontrada" });

        var producto = dto.Adapt<Producto>();

        var creado = await _productoService.Add(producto);

        var inventarioExistente = await _inventarioService.GetByProducto(creado.Id);
        if (inventarioExistente == null)
        {
            await _inventarioService.Add(new Inventario
            {
                ProductoId = creado.Id,
                Cantidad = 0,
                StockMinimo = 0,
                StockMaximo = 0
            });
        }

        var resultDto = await MapearProducto(creado);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductoUpdateDto dto)
    {
        var producto = await _productoService.GetById(id);
        if (producto == null) return NotFound();

        var categoria = await _categoriaService.GetById(dto.CategoriaId);
        if (categoria == null) return BadRequest(new { mensaje = "Categoria no encontrada" });

        producto.ActualizarNombre(dto.Nombre);
        producto.Descripcion = dto.Descripcion;
        producto.ActualizarPrecio(dto.Precio);
        producto.CategoriaId = dto.CategoriaId;
        if (dto.Activo) producto.Activar(); else producto.Desactivar();

        await _productoService.Update(producto);
        var resultDto = await MapearProducto(producto);
        return Ok(resultDto);
    }

    [HttpPatch("{id}/precio")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> UpdatePrecio(int id, [FromBody] ProductoPrecioUpdateDto dto)
    {
        try
        {
            await _productoService.UpdatePrecio(id, dto.NuevoPrecio);
            var producto = await _productoService.GetById(id);
            var resultDto = await MapearProducto(producto!);
            return Ok(resultDto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("{id}/categoria/{categoriaId}")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> AsociarCategoria(int id, int categoriaId)
    {
        var producto = await _productoService.GetById(id);
        if (producto == null) return NotFound();

        var categoria = await _categoriaService.GetById(categoriaId);
        if (categoria == null) return BadRequest(new { mensaje = "Categoria no encontrada" });

        producto.CategoriaId = categoriaId;
        await _productoService.Update(producto);
        var resultDto = await MapearProducto(producto);
        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _productoService.GetById(id);
        if (producto == null) return NotFound();

        await _productoService.Delete(id);
        return NoContent();
    }

    private async Task<ProductoDto> MapearProducto(Producto producto)
    {
        var dto = producto.Adapt<ProductoDto>();
        var categoria = await _categoriaService.GetById(producto.CategoriaId);
        dto.CategoriaNombre = categoria?.Nombre;
        return dto;
    }

    private async Task<IEnumerable<ProductoDto>> MapearProductos(IEnumerable<Producto> productos)
    {
        var dtos = new List<ProductoDto>();
        foreach (var p in productos)
        {
            dtos.Add(await MapearProducto(p));
        }
        return dtos;
    }
}
