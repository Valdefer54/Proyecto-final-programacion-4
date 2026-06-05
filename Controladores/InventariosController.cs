using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using parcial_1.DTOs;
using parcial_1.Modelos;
using parcial_1.Servicios;

namespace parcial_1.Controladores;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventariosController : ControllerBase
{
    private readonly IInventarioService _inventarioService;
    private readonly IProductoService _productoService;
    private readonly IKardexService _kardexService;

    public InventariosController(IInventarioService inventarioService, IProductoService productoService, IKardexService kardexService)
    {
        _inventarioService = inventarioService;
        _productoService = productoService;
        _kardexService = kardexService;
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
            if (diferencia > 0)
            {
                await _kardexService.RegistrarEntrada(new KardexEntradaRequestDto
                {
                    ProductoId = inventario.ProductoId,
                    Fecha = DateTime.Now,
                    Documento = "Ajuste",
                    Cantidad = diferencia,
                    CostoUnitario = 0,
                    Concepto = "Ajuste manual de inventario"
                });
            }
            else
            {
                await _kardexService.RegistrarSalida(new KardexSalidaRequestDto
                {
                    ProductoId = inventario.ProductoId,
                    Fecha = DateTime.Now,
                    Documento = "Ajuste",
                    Cantidad = Math.Abs(diferencia),
                    Concepto = "Ajuste manual de inventario"
                });
            }
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
