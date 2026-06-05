using parcial_1.Modelos;

namespace parcial_1.Repositorios;

public interface IKardexRepository
{
    Task<IEnumerable<KardexMovimiento>> GetByProducto(int productoId);
    Task<KardexMovimiento?> GetById(int id);
    Task<KardexMovimiento> Add(KardexMovimiento movimiento);
    Task<KardexMovimiento> Update(KardexMovimiento movimiento);
    Task Delete(int id);
    Task<bool> HasMovimientosPosteriores(int productoId, int movimientoId);
}
