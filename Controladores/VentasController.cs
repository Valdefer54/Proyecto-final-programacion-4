using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using parcial_1.DTOs;
using parcial_1.Modelos;
using parcial_1.Servicios;

namespace parcial_1.Controladores;

// Controlador de ventas: gestiona el CRUD de ventas, confirmación y cancelación.
// Valida stock antes de crear ventas y actualiza inventario automáticamente.
// Incluye endpoints: GET all, GET byId, GET byUsuario, POST create, PUT confirmar, PUT cancelar.
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VentasController : ControllerBase
{
    private readonly IVentaService _ventaService;
    private readonly IUsuarioService _usuarioService;
    private readonly IProductoService _productoService;
    private readonly IInventarioService _inventarioService;
    private readonly IMovimientoInventarioService _movimientoService;

    public VentasController(
        IVentaService ventaService,
        IUsuarioService usuarioService,
        IProductoService productoService,
        IInventarioService inventarioService,
        IMovimientoInventarioService movimientoService)
    {
        _ventaService = ventaService;
        _usuarioService = usuarioService;
        _productoService = productoService;
        _inventarioService = inventarioService;
        _movimientoService = movimientoService;
    }

    [HttpGet]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> GetAll()
    {
        var ventas = await _ventaService.GetAll();
        var dtos = await MapearVentas(ventas);
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var venta = await _ventaService.GetById(id);
        if (venta == null) return NotFound();
        
        var dto = await MapearVenta(venta);
        return Ok(dto);
    }

    [HttpGet("usuario/{usuarioId}")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> GetByUsuario(int usuarioId)
    {
        var ventas = await _ventaService.GetByUsuario(usuarioId);
        var dtos = await MapearVentas(ventas);
        return Ok(dtos);
    }

    [HttpGet("rango")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> GetByFechaRange([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
    {
        var ventas = await _ventaService.GetByFechaRange(inicio, fin);
        var dtos = await MapearVentas(ventas);
        return Ok(dtos);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] VentaCreateDto dto)
    {
        var usuario = await _usuarioService.GetById(dto.UsuarioId);
        if (usuario == null) return BadRequest(new { mensaje = "Usuario no encontrado" });

        var detalles = new List<DetalleVenta>();
        foreach (var det in dto.Detalles)
        {
            var producto = await _productoService.GetById(det.ProductoId);
            if (producto == null) return BadRequest(new { mensaje = $"Producto {det.ProductoId} no encontrado" });

            var inventario = await _inventarioService.GetByProducto(det.ProductoId);
            if (inventario == null || inventario.Cantidad < det.Cantidad)
            {
                return BadRequest(new { mensaje = $"Stock insuficiente para {producto.Nombre}" });
            }

            inventario.ReducirStock(det.Cantidad);
            await _inventarioService.Update(inventario);

            await _movimientoService.Add(new MovimientoInventario
            {
                ProductoId = det.ProductoId,
                Tipo = "Salida",
                Cantidad = det.Cantidad,
                StockResultante = inventario.Cantidad,
                Referencia = $"Venta"
            });

            detalles.Add(new DetalleVenta
            {
                ProductoId = det.ProductoId,
                Cantidad = det.Cantidad,
                PrecioUnitario = det.PrecioUnitario
            });
        }

        var venta = new Venta
        {
            UsuarioId = dto.UsuarioId,
            Detalles = detalles
        };

        var creado = await _ventaService.Add(venta);
        var resultDto = await MapearVenta(creado);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, resultDto);
    }

    [HttpPut("{id}/confirmar")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Confirmar(int id)
    {
        var venta = await _ventaService.GetById(id);
        if (venta == null) return NotFound();

        venta.Confirmar();
        await _ventaService.Update(venta);
        var dto = await MapearVenta(venta);
        return Ok(dto);
    }

    [HttpPut("{id}/cancelar")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> Cancelar(int id)
    {
        var venta = await _ventaService.GetById(id);
        if (venta == null) return NotFound();

        foreach (var det in venta.Detalles)
        {
            var inventario = await _inventarioService.GetByProducto(det.ProductoId);
            if (inventario != null)
            {
                inventario.AgregarStock(det.Cantidad);
                await _inventarioService.Update(inventario);

                await _movimientoService.Add(new MovimientoInventario
                {
                    ProductoId = det.ProductoId,
                    Tipo = "Entrada",
                    Cantidad = det.Cantidad,
                    StockResultante = inventario.Cantidad,
                    Referencia = $"Cancelación venta #{id}"
                });
            }
        }

        venta.Cancelar();
        await _ventaService.Update(venta);
        var dto = await MapearVenta(venta);
        return Ok(dto);
    }

    private async Task<VentaDto> MapearVenta(Venta venta)
    {
        var dto = venta.Adapt<VentaDto>();
        var usuario = await _usuarioService.GetById(venta.UsuarioId);
        dto.UsuarioNombre = usuario?.Nombre;

        foreach (var det in dto.Detalles)
        {
            var producto = await _productoService.GetById(det.ProductoId);
            det.ProductoNombre = producto?.Nombre;
        }

        return dto;
    }

    private async Task<IEnumerable<VentaDto>> MapearVentas(IEnumerable<Venta> ventas)
    {
        var dtos = new List<VentaDto>();
        foreach (var v in ventas)
        {
            dtos.Add(await MapearVenta(v));
        }
        return dtos;
    }
}
