using parcial_1.Modelos;

namespace parcial_1.Repositorios;

public class KardexRepository : IKardexRepository
{
    private static readonly List<KardexMovimiento> _movimientos = new();
    private static int _nextId = 1;

    public Task<IEnumerable<KardexMovimiento>> GetByProducto(int productoId)
    {
        var result = _movimientos
            .Where(m => m.ProductoId == productoId)
            .OrderBy(m => m.Fecha)
            .ThenBy(m => m.Id)
            .ToList();
        return Task.FromResult<IEnumerable<KardexMovimiento>>(result);
    }

    public Task<KardexMovimiento?> GetById(int id)
    {
        var mov = _movimientos.FirstOrDefault(m => m.Id == id);
        return Task.FromResult(mov);
    }

    public Task<KardexMovimiento> Add(KardexMovimiento movimiento)
    {
        movimiento.Id = _nextId++;
        movimiento.FechaCreacion = DateTime.Now;
        _movimientos.Add(movimiento);
        return Task.FromResult(movimiento);
    }

    public Task<KardexMovimiento> Update(KardexMovimiento movimiento)
    {
        var idx = _movimientos.FindIndex(m => m.Id == movimiento.Id);
        if (idx >= 0)
            _movimientos[idx] = movimiento;
        return Task.FromResult(movimiento);
    }

    public Task Delete(int id)
    {
        var mov = _movimientos.FirstOrDefault(m => m.Id == id);
        if (mov != null)
            _movimientos.Remove(mov);
        return Task.CompletedTask;
    }

    public Task<bool> HasMovimientosPosteriores(int productoId, int movimientoId)
    {
        var mov = _movimientos.FirstOrDefault(m => m.Id == movimientoId);
        if (mov == null)
            return Task.FromResult(false);

        var has = _movimientos.Any(m =>
            m.ProductoId == productoId &&
            m.Id != movimientoId &&
            (m.Fecha > mov.Fecha || (m.Fecha == mov.Fecha && m.Id > mov.Id)));

        return Task.FromResult(has);
    }
}
