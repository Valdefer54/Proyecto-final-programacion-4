namespace parcial_1.Modelos;

// Modelo de venta: representa una transacción de venta asociada a un usuario.
// Contiene la lista de detalles, calcula el total, y permite confirmar o cancelar la venta.
public class Venta : BaseEntity
{
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public List<DetalleVenta> Detalles { get; set; } = new();
    public string Estado { get; set; } = "Pendiente";

    public void CalcularTotal()
    {
        Total = Detalles.Sum(d => d.Subtotal);
    }
    public void Confirmar() => Estado = "Confirmada";
    public void Cancelar() => Estado = "Cancelada";
}