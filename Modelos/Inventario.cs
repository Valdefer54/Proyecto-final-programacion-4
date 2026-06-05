namespace parcial_1.Modelos;

// Modelo de inventario: representa el stock de un producto con cantidades y límites.
// Permite actualizar, agregar y reducir stock, y verificar si está por debajo del mínimo.
public class Inventario : BaseEntity
{
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public int Cantidad { get; set; }
    public int StockMinimo { get; set; }
    public int StockMaximo { get; set; }
    public DateTime UltimaActualizacion { get; set; }

    public void ActualizarCantidad(int cantidad) => Cantidad = cantidad;
    public void AgregarStock(int cantidad) => Cantidad += cantidad;
    public void ReducirStock(int cantidad) => Cantidad -= cantidad;
    public bool EstaBajoStock() => Cantidad < StockMinimo;
}