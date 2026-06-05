using parcial_1.Modelos;

namespace parcial_1.DTOs;

// DTO para detalle de venta en respuesta: incluye datos del producto, cantidad, precio y subtotal.
public class DetalleVentaDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

// DTO para venta en respuesta: incluye datos del usuario, fecha, total, estado y lista de detalles.
public class VentaDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string? UsuarioNombre { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public string Estado { get; set; } = string.Empty;
    public List<DetalleVentaDto> Detalles { get; set; } = new();
}

// DTO para crear una venta: recibe el ID del usuario y la lista de detalles a incluir.
public class VentaCreateDto
{
    public int UsuarioId { get; set; }
    public List<DetalleVentaCreateDto> Detalles { get; set; } = new();
}

// DTO para crear un detalle de venta: recibe producto ID, cantidad y precio unitario.
public class DetalleVentaCreateDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}
