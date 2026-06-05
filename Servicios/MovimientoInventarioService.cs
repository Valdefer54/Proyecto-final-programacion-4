using parcial_1.Modelos;
using parcial_1.Repositorios;

namespace parcial_1.Servicios;

public class MovimientoInventarioService : IMovimientoInventarioService
{
    private readonly IMovimientoInventarioRepository _repository;

    public MovimientoInventarioService(IMovimientoInventarioRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<MovimientoInventario>> GetAll()
    {
        return _repository.GetAll();
    }

    public Task<IEnumerable<MovimientoInventario>> GetByProducto(int productoId)
    {
        return _repository.GetByProducto(productoId);
    }

    public Task<MovimientoInventario> Add(MovimientoInventario movimiento)
    {
        return _repository.Add(movimiento);
    }
}
