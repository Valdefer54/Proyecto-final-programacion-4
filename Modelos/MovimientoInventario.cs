namespace parcial_1.Modelos;

public class MovimientoInventario
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public int StockResultante { get; set; }
    public string? Referencia { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;
}
