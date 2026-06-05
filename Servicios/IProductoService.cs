using parcial_1.Modelos;

namespace parcial_1.Servicios;

// Interfaz del servicio de productos: define el contrato para operaciones CRUD y búsqueda por categoría.
public interface IProductoService
{
    Task<Producto?> GetById(int id);
    Task<IEnumerable<Producto>> GetAll();
    Task<IEnumerable<Producto>> GetByCategoria(int categoriaId);
    Task<IEnumerable<Producto>> GetByDisponibilidad(bool activo);
    Task UpdatePrecio(int id, decimal nuevoPrecio);
    Task<Producto> Add(Producto producto);
    Task<Producto> Update(Producto producto);
    Task Delete(int id);
}