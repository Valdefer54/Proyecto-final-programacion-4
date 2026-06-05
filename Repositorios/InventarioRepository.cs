using parcial_1.Modelos;

namespace parcial_1.Repositorios;

// Repositorio de inventarios: implementa acceso a datos usando una lista estática en memoria con datos iniciales.
// Proporciona operaciones CRUD, búsqueda por producto y filtrado de inventario con stock bajo.
public class InventarioRepository : IInventarioRepository
{
    private static List<Inventario> _inventarios = new()
    {
        new Inventario { Id = 1, ProductoId = 1, Cantidad = 50, StockMinimo = 10, StockMaximo = 100, UltimaActualizacion = DateTime.Now },
        new Inventario { Id = 2, ProductoId = 2, Cantidad = 5, StockMinimo = 20, StockMaximo = 100, UltimaActualizacion = DateTime.Now }
    };
    private static int _nextId = 3;

    public Task<Inventario?> GetById(int id)
    {
        var inventario = _inventarios.FirstOrDefault(i => i.Id == id);
        return Task.FromResult(inventario);
    }

    public Task<Inventario?> GetByProducto(int productoId)
    {
        var inventario = _inventarios.FirstOrDefault(i => i.ProductoId == productoId);
        return Task.FromResult(inventario);
    }

    public Task<IEnumerable<Inventario>> GetAll()
    {
        return Task.FromResult<IEnumerable<Inventario>>(_inventarios);
    }

    public Task<IEnumerable<Inventario>> GetBajoStock()
    {
        var bajosStock = _inventarios.Where(i => i.EstaBajoStock());
        return Task.FromResult(bajosStock);
    }

    public Task<Inventario> Add(Inventario inventario)
    {
        inventario.Id = _nextId++;
        _inventarios.Add(inventario);
        return Task.FromResult(inventario);
    }

    public Task<Inventario> Update(Inventario inventario)
    {
        var index = _inventarios.FindIndex(i => i.Id == inventario.Id);
        if (index >= 0)
        {
            _inventarios[index] = inventario;
        }
        return Task.FromResult(inventario);
    }

    public Task Delete(int id)
    {
        var inventario = _inventarios.FirstOrDefault(i => i.Id == id);
        if (inventario != null)
        {
            _inventarios.Remove(inventario);
        }
        return Task.CompletedTask;
    }
}
