using parcial_1.Modelos;
using parcial_1.Repositorios;

namespace parcial_1.Servicios;

// Servicio de inventarios: implementa la lógica de negocio para operaciones con inventario.
// Actualiza la fecha de última modificación al crear o actualizar registros.
public class InventarioService : IInventarioService
{
    private readonly IInventarioRepository _inventarioRepository;

    public InventarioService(IInventarioRepository inventarioRepository)
    {
        _inventarioRepository = inventarioRepository;
    }

    public Task<Inventario?> GetById(int id) => _inventarioRepository.GetById(id);
    public Task<Inventario?> GetByProducto(int productoId) => _inventarioRepository.GetByProducto(productoId);
    public Task<IEnumerable<Inventario>> GetAll() => _inventarioRepository.GetAll();
    public Task<IEnumerable<Inventario>> GetBajoStock() => _inventarioRepository.GetBajoStock();

    public Task<Inventario> Add(Inventario inventario)
    {
        inventario.UltimaActualizacion = DateTime.Now;
        return _inventarioRepository.Add(inventario);
    }

    public Task<Inventario> Update(Inventario inventario)
    {
        inventario.UltimaActualizacion = DateTime.Now;
        return _inventarioRepository.Update(inventario);
    }

    public Task Delete(int id) => _inventarioRepository.Delete(id);
}
