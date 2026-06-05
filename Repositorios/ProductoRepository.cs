using parcial_1.Modelos;

namespace parcial_1.Repositorios;

// Repositorio de productos: implementa acceso a datos usando una lista estática en memoria con datos iniciales.
// Proporciona operaciones CRUD y filtrado de productos por categoría.
public class ProductoRepository : IProductoRepository
{
    private static List<Producto> _productos = new()
    {
        new Producto { Id = 1, Nombre = "Laptop", Descripcion = "Laptop HP", Precio = 1500.00m, CategoriaId = 1, FechaCreacion = DateTime.Now, Activo = true },
        new Producto { Id = 2, Nombre = "Camisa", Descripcion = "Camisa de algodon", Precio = 25.00m, CategoriaId = 2, FechaCreacion = DateTime.Now, Activo = true }
    };
    private static int _nextId = 3;

    public Task<Producto?> GetById(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(producto);
    }

    public Task<IEnumerable<Producto>> GetAll()
    {
        return Task.FromResult<IEnumerable<Producto>>(_productos);
    }

    public Task<IEnumerable<Producto>> GetByCategoria(int categoriaId)
    {
        var productos = _productos.Where(p => p.CategoriaId == categoriaId);
        return Task.FromResult(productos);
    }

    public Task<IEnumerable<Producto>> GetByDisponibilidad(bool activo)
    {
        var productos = _productos.Where(p => p.Activo == activo);
        return Task.FromResult(productos);
    }

    public Task<Producto> Add(Producto producto)
    {
        producto.Id = _nextId++;
        _productos.Add(producto);
        return Task.FromResult(producto);
    }

    public Task<Producto> Update(Producto producto)
    {
        var index = _productos.FindIndex(p => p.Id == producto.Id);
        if (index >= 0)
        {
            _productos[index] = producto;
        }
        return Task.FromResult(producto);
    }

    public Task Delete(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto != null)
        {
            _productos.Remove(producto);
        }
        return Task.CompletedTask;
    }
}
