using parcial_1.Modelos;

namespace parcial_1.Repositorios;

// Interfaz del repositorio de inventarios: define el contrato para acceso a datos de inventario.
public interface IInventarioRepository
{
    Task<Inventario?> GetById(int id);
    Task<Inventario?> GetByProducto(int productoId);
    Task<IEnumerable<Inventario>> GetAll();
    Task<IEnumerable<Inventario>> GetBajoStock();
    Task<Inventario> Add(Inventario inventario);
    Task<Inventario> Update(Inventario inventario);
    Task Delete(int id);
}