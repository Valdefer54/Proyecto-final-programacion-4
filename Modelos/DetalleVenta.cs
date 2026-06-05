namespace parcial_1.Modelos;

// Modelo de detalle de venta: representa una línea dentro de una venta con producto, cantidad y precio.
// Calcula el subtotal automáticamente como cantidad multiplicada por precio unitario.
public class DetalleVenta : BaseEntity
{
    public int VentaId { get; set; }
    public Venta? Venta { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => Cantidad * PrecioUnitario;
}