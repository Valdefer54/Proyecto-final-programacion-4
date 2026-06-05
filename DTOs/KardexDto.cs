namespace parcial_1.DTOs;

public class KardexResponseDto
{
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public List<KardexMovimientoDto> Movimientos { get; set; } = new();
    public KardexSaldoDto Saldo { get; set; } = new();
}

public class KardexMovimientoDto
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string? Documento { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal CostoUnitario { get; set; }
    public decimal TotalMovimiento { get; set; }
    public string? Concepto { get; set; }
    public string? Observaciones { get; set; }
    public int SaldoCantidad { get; set; }
    public decimal SaldoCostoUnitario { get; set; }
    public decimal SaldoValorTotal { get; set; }
}

public class KardexSaldoDto
{
    public int Cantidad { get; set; }
    public decimal CostoPromedioPonderado { get; set; }
    public decimal ValorTotal { get; set; }
}

public class KardexSaldoResponseDto
{
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public int Cantidad { get; set; }
    public decimal CostoPromedioPonderado { get; set; }
    public decimal ValorTotal { get; set; }
}

public class KardexEntradaRequestDto
{
    public int ProductoId { get; set; }
    public DateTime Fecha { get; set; }
    public string Documento { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal CostoUnitario { get; set; }
    public string? Concepto { get; set; }
}

public class KardexSalidaRequestDto
{
    public int ProductoId { get; set; }
    public DateTime Fecha { get; set; }
    public string Documento { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public string? Concepto { get; set; }
}

public class KardexUpdateDto
{
    public string? Concepto { get; set; }
    public string? Observaciones { get; set; }
}

public class KardexOperacionResponseDto
{
    public int Id { get; set; }
    public decimal? NuevoCpp { get; set; }
    public decimal? CppUsado { get; set; }
    public decimal? CostoTotalSalida { get; set; }
    public int SaldoCantidad { get; set; }
    public decimal? ValorSaldo { get; set; }
    public string Estado { get; set; } = "ok";
    public bool? Actualizado { get; set; }
    public bool? Eliminado { get; set; }
    public string? Error { get; set; }
}
