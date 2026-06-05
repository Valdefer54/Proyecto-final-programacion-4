using parcial_1.Modelos;

namespace parcial_1.Repositorios;

public class MovimientoInventarioRepository : IMovimientoInventarioRepository
{
    private static List<MovimientoInventario> _movimientos = new();
    private static int _nextId = 1;

    public Task<IEnumerable<MovimientoInventario>> GetAll()
    {
        return Task.FromResult<IEnumerable<MovimientoInventario>>(_movimientos.AsEnumerable().Reverse());
    }

    public Task<IEnumerable<MovimientoInventario>> GetByProducto(int productoId)
    {
        return Task.FromResult<IEnumerable<MovimientoInventario>>(
            _movimientos.Where(m => m.ProductoId == productoId).Reverse());
    }

    public Task<MovimientoInventario> Add(MovimientoInventario movimiento)
    {
        movimiento.Id = _nextId++;
        _movimientos.Add(movimiento);
        return Task.FromResult(movimiento);
    }
}
