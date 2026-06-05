using parcial_1.DTOs;

namespace parcial_1.Servicios;

public interface IKardexService
{
    Task<KardexResponseDto> GetKardexByProducto(int productoId);
    Task<KardexSaldoResponseDto> GetSaldoByProducto(int productoId);
    Task<KardexOperacionResponseDto> RegistrarEntrada(KardexEntradaRequestDto dto);
    Task<KardexOperacionResponseDto> RegistrarSalida(KardexSalidaRequestDto dto);
    Task<KardexOperacionResponseDto> ActualizarMovimiento(int id, KardexUpdateDto dto);
    Task<KardexOperacionResponseDto> EliminarMovimiento(int id);
}
