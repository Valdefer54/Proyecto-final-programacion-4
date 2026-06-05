namespace parcial_1.Modelos;

public class KardexMovimiento
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public DateTime Fecha { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal CostoUnitario { get; set; }
    public string? Concepto { get; set; }
    public string? Observaciones { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}
