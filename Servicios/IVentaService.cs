using parcial_1.Modelos;

namespace parcial_1.Servicios;

// Interfaz del servicio de ventas: define el contrato para operaciones CRUD y búsqueda por usuario de ventas.
public interface IVentaService
{
    Task<Venta?> GetById(int id);
    Task<IEnumerable<Venta>> GetAll();
    Task<IEnumerable<Venta>> GetByUsuario(int usuarioId);
    Task<IEnumerable<Venta>> GetByFechaRange(DateTime inicio, DateTime fin);
    Task<Venta> Add(Venta venta);
    Task<Venta> Update(Venta venta);
    Task Delete(int id);
}