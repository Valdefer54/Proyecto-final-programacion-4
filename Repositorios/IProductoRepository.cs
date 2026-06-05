using parcial_1.Modelos;

namespace parcial_1.Repositorios;

// Interfaz del repositorio de productos: define el contrato para acceso a datos de productos.
public interface IProductoRepository
{
    Task<Producto?> GetById(int id);
    Task<IEnumerable<Producto>> GetAll();
    Task<IEnumerable<Producto>> GetByCategoria(int categoriaId);
    Task<IEnumerable<Producto>> GetByDisponibilidad(bool activo);
    Task<Producto> Add(Producto producto);
    Task<Producto> Update(Producto producto);
    Task Delete(int id);
}