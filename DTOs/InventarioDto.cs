using parcial_1.Modelos;

namespace parcial_1.DTOs;

// DTO para inventario en respuesta: incluye datos del producto, cantidades, límites de stock y estado de bajo stock.
public class InventarioDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public int Cantidad { get; set; }
    public int StockMinimo { get; set; }
    public int StockMaximo { get; set; }
    public DateTime UltimaActualizacion { get; set; }
    public bool BajoStock { get; set; }
}

// DTO para crear inventario: recibe producto ID, cantidad inicial y límites de stock mínimo/máximo.
public class InventarioCreateDto
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public int StockMinimo { get; set; }
    public int StockMaximo { get; set; }
}

// DTO para actualizar inventario: recibe ID, producto ID, nueva cantidad y límites de stock.
public class InventarioUpdateDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public int StockMinimo { get; set; }
    public int StockMaximo { get; set; }
}
