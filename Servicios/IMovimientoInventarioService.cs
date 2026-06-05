using parcial_1.Modelos;

namespace parcial_1.Servicios;

public interface IMovimientoInventarioService
{
    Task<IEnumerable<MovimientoInventario>> GetAll();
    Task<IEnumerable<MovimientoInventario>> GetByProducto(int productoId);
    Task<MovimientoInventario> Add(MovimientoInventario movimiento);
}
