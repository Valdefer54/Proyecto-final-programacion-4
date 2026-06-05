using parcial_1.Modelos;

namespace parcial_1.Repositorios;

// Interfaz del repositorio de ventas: define el contrato para acceso a datos de ventas.
public interface IVentaRepository
{
    Task<Venta?> GetById(int id);
    Task<IEnumerable<Venta>> GetAll();
    Task<IEnumerable<Venta>> GetByUsuario(int usuarioId);
    Task<IEnumerable<Venta>> GetByFechaRange(DateTime inicio, DateTime fin);
    Task<Venta> Add(Venta venta);
    Task<Venta> Update(Venta venta);
    Task Delete(int id);
}