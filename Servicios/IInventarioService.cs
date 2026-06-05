using parcial_1.Modelos;

namespace parcial_1.Servicios;

// Interfaz del servicio de inventarios: define el contrato para operaciones CRUD y consulta de stock bajo.
public interface IInventarioService
{
    Task<Inventario?> GetById(int id);
    Task<Inventario?> GetByProducto(int productoId);
    Task<IEnumerable<Inventario>> GetAll();
    Task<IEnumerable<Inventario>> GetBajoStock();
    Task<Inventario> Add(Inventario inventario);
    Task<Inventario> Update(Inventario inventario);
    Task Delete(int id);
}