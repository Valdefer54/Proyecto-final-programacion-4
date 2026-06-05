using parcial_1.Modelos;

namespace parcial_1.Repositorios;

// Repositorio de ventas: implementa acceso a datos usando una lista estática en memoria.
// Proporciona operaciones CRUD y búsqueda por usuario con un contador autoincremental para IDs.
public class VentaRepository : IVentaRepository
{
    private static List<Venta> _ventas = new();
    private static int _nextId = 1;

    public Task<Venta?> GetById(int id)
    {
        var venta = _ventas.FirstOrDefault(v => v.Id == id);
        return Task.FromResult(venta);
    }

    public Task<IEnumerable<Venta>> GetAll()
    {
        return Task.FromResult<IEnumerable<Venta>>(_ventas);
    }

    public Task<IEnumerable<Venta>> GetByUsuario(int usuarioId)
    {
        var ventas = _ventas.Where(v => v.UsuarioId == usuarioId);
        return Task.FromResult(ventas);
    }

    public Task<IEnumerable<Venta>> GetByFechaRange(DateTime inicio, DateTime fin)
    {
        var ventas = _ventas.Where(v => v.Fecha >= inicio && v.Fecha <= fin);
        return Task.FromResult(ventas);
    }

    public Task<Venta> Add(Venta venta)
    {
        venta.Id = _nextId++;
        _ventas.Add(venta);
        return Task.FromResult(venta);
    }

    public Task<Venta> Update(Venta venta)
    {
        var index = _ventas.FindIndex(v => v.Id == venta.Id);
        if (index >= 0)
        {
            _ventas[index] = venta;
        }
        return Task.FromResult(venta);
    }

    public Task Delete(int id)
    {
        var venta = _ventas.FirstOrDefault(v => v.Id == id);
        if (venta != null)
        {
            _ventas.Remove(venta);
        }
        return Task.CompletedTask;
    }
}
