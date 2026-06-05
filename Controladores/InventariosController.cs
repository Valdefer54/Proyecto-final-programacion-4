using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using parcial_1.DTOs;
using parcial_1.Modelos;
using parcial_1.Servicios;

namespace parcial_1.Controladores;

// Controlador de inventarios: gestiona el CRUD del inventario y consulta de stock bajo.
// Permite actualizar cantidades y consultar inventario por producto.
// Incluye endpoints: GET all, GET byId, GET byProducto, GET bajo-stock, POST create, PUT update, DELETE.
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventariosController : ControllerBase
{
    private readonly IInventarioService _inventarioService;
    private readonly IProductoService _productoService;
    private readonly IMovimientoInventarioService _movimientoService;

    public InventariosController(IInventarioService inventarioService, IProductoService productoService, IMovimientoInventarioService movimientoService)
    {
        _inventarioService = inventarioService;
        _productoService = productoService;
        _movimientoService = movimientoService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var inventarios = await _inventarioService.GetAll();
        var dtos = await MapearInventarios(inventarios);
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var inventario = await _inventarioService.GetById(id);
        if (inventario == null) return NotFound();
        
        var dto = await MapearInventario(inventario);
        return Ok(dto);
    }

    [HttpGet("producto/{productoId}")]
    public async Task<IActionResult> GetByProducto(int productoId)
    {
        var inventario = await _inventarioService.GetByProducto(productoId);
        if (inventario == null) return NotFound();
        
        var dto = await MapearInventario(inventario);
        return Ok(dto);
    }

    [HttpGet("movimientos")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> GetMovimientos()
    {
        var movimientos = await _movimientoService.GetAll();
        return Ok(movimientos);
    }

    [HttpGet("movimientos/producto/{productoId}")]
    public async Task<IActionResult> GetMovimientosByProducto(int productoId)
    {
        var movimientos = await _movimientoService.GetByProducto(productoId);
        return Ok(movimientos);
    }

    [HttpGet("bajo-stock")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> GetBajoStock()
    {
        var inventarios = await _inventarioService.GetBajoStock();
        var dtos = await MapearInventarios(inventarios);
        return Ok(dtos);
    }

    [HttpPost]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Create([FromBody] InventarioCreateDto dto)
    {
        var producto = await _productoService.GetById(dto.ProductoId);
        if (producto == null) return BadRequest(new { mensaje = "Producto no encontrado" });

        var existente = await _inventarioService.GetByProducto(dto.ProductoId);
        if (existente != null) return BadRequest(new { mensaje = "El producto ya tiene inventario" });

        var inventario = dto.Adapt<Inventario>();

        var creado = await _inventarioService.Add(inventario);

        await _movimientoService.Add(new MovimientoInventario
        {
            ProductoId = dto.ProductoId,
            Tipo = "Entrada",
            Cantidad = dto.Cantidad,
            StockResultante = dto.Cantidad,
            Referencia = "Inventario inicial"
        });

        var resultDto = await MapearInventario(creado);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, resultDto);
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Update(int id, [FromBody] InventarioUpdateDto dto)
    {
        var inventario = await _inventarioService.GetById(id);
        if (inventario == null) return NotFound();

        var diferencia = dto.Cantidad - inventario.Cantidad;

        inventario.ActualizarCantidad(dto.Cantidad);
        inventario.StockMinimo = dto.StockMinimo;
        inventario.StockMaximo = dto.StockMaximo;

        await _inventarioService.Update(inventario);

        if (diferencia != 0)
        {
            await _movimientoService.Add(new MovimientoInventario
            {
                ProductoId = inventario.ProductoId,
                Tipo = diferencia > 0 ? "Entrada" : "Salida",
                Cantidad = Math.Abs(diferencia),
                StockResultante = inventario.Cantidad,
                Referencia = "Ajuste manual"
            });
        }
        var resultDto = await MapearInventario(inventario);
        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var inventario = await _inventarioService.GetById(id);
        if (inventario == null) return NotFound();

        await _inventarioService.Delete(id);
        return NoContent();
    }

    private async Task<InventarioDto> MapearInventario(Inventario inventario)
    {
        var dto = inventario.Adapt<InventarioDto>();
        var producto = await _productoService.GetById(inventario.ProductoId);
        dto.ProductoNombre = producto?.Nombre;
        dto.BajoStock = inventario.EstaBajoStock();
        return dto;
    }

    private async Task<IEnumerable<InventarioDto>> MapearInventarios(IEnumerable<Inventario> inventarios)
    {
        var dtos = new List<InventarioDto>();
        foreach (var i in inventarios)
        {
            dtos.Add(await MapearInventario(i));
        }
        return dtos;
    }
}
