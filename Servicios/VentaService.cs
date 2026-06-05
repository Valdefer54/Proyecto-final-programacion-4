using parcial_1.Modelos;
using parcial_1.Repositorios;

namespace parcial_1.Servicios;

// Servicio de ventas: implementa la lógica de negocio para operaciones con ventas.
// Delega al repositorio y establece fecha y total al crear una venta.
public class VentaService : IVentaService
{
    private readonly IVentaRepository _ventaRepository;

    public VentaService(IVentaRepository ventaRepository)
    {
        _ventaRepository = ventaRepository;
    }

    public Task<Venta?> GetById(int id) => _ventaRepository.GetById(id);
    public Task<IEnumerable<Venta>> GetAll() => _ventaRepository.GetAll();
    public Task<IEnumerable<Venta>> GetByUsuario(int usuarioId) => _ventaRepository.GetByUsuario(usuarioId);
    public Task<IEnumerable<Venta>> GetByFechaRange(DateTime inicio, DateTime fin) => _ventaRepository.GetByFechaRange(inicio, fin);

    public Task<Venta> Add(Venta venta)
    {
        venta.Fecha = DateTime.Now;
        venta.CalcularTotal();
        return _ventaRepository.Add(venta);
    }

    public Task<Venta> Update(Venta venta) => _ventaRepository.Update(venta);
    public Task Delete(int id) => _ventaRepository.Delete(id);
}
