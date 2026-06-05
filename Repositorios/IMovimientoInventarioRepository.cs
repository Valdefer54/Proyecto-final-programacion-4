using parcial_1.Modelos;

namespace parcial_1.Repositorios;

public interface IMovimientoInventarioRepository
{
    Task<IEnumerable<MovimientoInventario>> GetAll();
    Task<IEnumerable<MovimientoInventario>> GetByProducto(int productoId);
    Task<MovimientoInventario> Add(MovimientoInventario movimiento);
}
