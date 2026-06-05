using parcial_1.Modelos;
using parcial_1.Repositorios;

namespace parcial_1.Servicios;

// Servicio de productos: implementa la lógica de negocio para operaciones con productos.
// Establece fecha de creación y estado activo al crear un nuevo producto.
public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productoRepository;

    public ProductoService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public Task<Producto?> GetById(int id) => _productoRepository.GetById(id);
    public Task<IEnumerable<Producto>> GetAll() => _productoRepository.GetAll();
    public Task<IEnumerable<Producto>> GetByCategoria(int categoriaId) => _productoRepository.GetByCategoria(categoriaId);
    public Task<IEnumerable<Producto>> GetByDisponibilidad(bool activo) => _productoRepository.GetByDisponibilidad(activo);
    public async Task UpdatePrecio(int id, decimal nuevoPrecio)
    {
        var producto = await _productoRepository.GetById(id);
        if (producto == null) throw new KeyNotFoundException("Producto no encontrado");
        producto.ActualizarPrecio(nuevoPrecio);
        await _productoRepository.Update(producto);
    }

    public Task<Producto> Add(Producto producto)
    {
        producto.FechaCreacion = DateTime.Now;
        producto.Activo = true;
        return _productoRepository.Add(producto);
    }

    public Task<Producto> Update(Producto producto) => _productoRepository.Update(producto);
    public Task Delete(int id) => _productoRepository.Delete(id);
}
