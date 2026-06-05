using parcial_1.DTOs;
using parcial_1.Modelos;
using parcial_1.Repositorios;

namespace parcial_1.Servicios;

public class KardexService : IKardexService
{
    private readonly IKardexRepository _repository;
    private readonly IProductoService _productoService;
    private readonly IInventarioService _inventarioService;

    public KardexService(IKardexRepository repository, IProductoService productoService, IInventarioService inventarioService)
    {
        _repository = repository;
        _productoService = productoService;
        _inventarioService = inventarioService;
    }

    private (int cantidad, decimal cpp, decimal valorTotal) CalcularBalance(
        IEnumerable<KardexMovimiento> movimientos)
    {
        int cantidad = 0;
        decimal valorTotal = 0;
        decimal cpp = 0;

        foreach (var m in movimientos)
        {
            if (m.Tipo == "Entrada")
            {
                valorTotal += m.Cantidad * m.CostoUnitario;
                cantidad += m.Cantidad;
                cpp = cantidad > 0 ? valorTotal / cantidad : 0;
            }
            else
            {
                var costoSalida = m.Cantidad * cpp;
                valorTotal -= costoSalida;
                cantidad -= m.Cantidad;
            }
        }

        return (cantidad, cpp, valorTotal);
    }

    public async Task<KardexResponseDto> GetKardexByProducto(int productoId)
    {
        var producto = await _productoService.GetById(productoId);
        var movimientos = await _repository.GetByProducto(productoId);

        int cantidad = 0;
        decimal valorTotal = 0;
        decimal cpp = 0;
        var movimientosDto = new List<KardexMovimientoDto>();

        foreach (var m in movimientos)
        {
            decimal totalMovimiento;
            if (m.Tipo == "Entrada")
            {
                totalMovimiento = m.Cantidad * m.CostoUnitario;
                valorTotal += totalMovimiento;
                cantidad += m.Cantidad;
                cpp = cantidad > 0 ? valorTotal / cantidad : 0;
            }
            else
            {
                totalMovimiento = m.Cantidad * cpp;
                valorTotal -= totalMovimiento;
                cantidad -= m.Cantidad;
            }

            movimientosDto.Add(new KardexMovimientoDto
            {
                Id = m.Id,
                Fecha = m.Fecha,
                Documento = m.Documento,
                Tipo = m.Tipo,
                Cantidad = m.Cantidad,
                CostoUnitario = m.Tipo == "Entrada" ? m.CostoUnitario : cpp,
                TotalMovimiento = totalMovimiento,
                Concepto = m.Concepto,
                Observaciones = m.Observaciones,
                SaldoCantidad = cantidad,
                SaldoCostoUnitario = cpp,
                SaldoValorTotal = valorTotal
            });
        }

        return new KardexResponseDto
        {
            ProductoId = productoId,
            ProductoNombre = producto?.Nombre,
            Movimientos = movimientosDto,
            Saldo = new KardexSaldoDto
            {
                Cantidad = cantidad,
                CostoPromedioPonderado = cpp,
                ValorTotal = valorTotal
            }
        };
    }

    public async Task<KardexSaldoResponseDto> GetSaldoByProducto(int productoId)
    {
        var producto = await _productoService.GetById(productoId);
        var movimientos = await _repository.GetByProducto(productoId);
        var (cantidad, cpp, valorTotal) = CalcularBalance(movimientos);

        return new KardexSaldoResponseDto
        {
            ProductoId = productoId,
            ProductoNombre = producto?.Nombre,
            Cantidad = cantidad,
            CostoPromedioPonderado = cpp,
            ValorTotal = valorTotal
        };
    }

    public async Task<KardexOperacionResponseDto> RegistrarEntrada(KardexEntradaRequestDto dto)
    {
        var producto = await _productoService.GetById(dto.ProductoId);
        if (producto == null)
            return ErrorResponse("Producto no encontrado");

        var movimientos = await _repository.GetByProducto(dto.ProductoId);
        var (cantidadActual, _, valorActual) = CalcularBalance(movimientos);

        var nuevoValorTotal = valorActual + dto.Cantidad * dto.CostoUnitario;
        var nuevaCantidad = cantidadActual + dto.Cantidad;
        var nuevoCpp = nuevoValorTotal / nuevaCantidad;

        var movimiento = new KardexMovimiento
        {
            ProductoId = dto.ProductoId,
            Fecha = dto.Fecha,
            Documento = dto.Documento,
            Tipo = "Entrada",
            Cantidad = dto.Cantidad,
            CostoUnitario = dto.CostoUnitario,
            Concepto = dto.Concepto ?? "Compra"
        };

        var creado = await _repository.Add(movimiento);

        var inventario = await _inventarioService.GetByProducto(dto.ProductoId);
        if (inventario == null)
        {
            inventario = new Inventario { ProductoId = dto.ProductoId, Cantidad = cantidadActual, StockMinimo = 0, StockMaximo = 0 };
            await _inventarioService.Add(inventario);
        }
        inventario.AgregarStock(dto.Cantidad);
        await _inventarioService.Update(inventario);

        return new KardexOperacionResponseDto
        {
            Id = creado.Id,
            NuevoCpp = nuevoCpp,
            SaldoCantidad = nuevaCantidad,
            ValorSaldo = nuevoValorTotal,
            Estado = "ok"
        };
    }

    public async Task<KardexOperacionResponseDto> RegistrarSalida(KardexSalidaRequestDto dto)
    {
        var producto = await _productoService.GetById(dto.ProductoId);
        if (producto == null)
            return ErrorResponse("Producto no encontrado");

        var movimientos = await _repository.GetByProducto(dto.ProductoId);
        var (cantidadActual, cppActual, valorActual) = CalcularBalance(movimientos);

        if (dto.Cantidad > cantidadActual)
            return ErrorResponse("Stock insuficiente");

        var costeTotalSalida = dto.Cantidad * cppActual;
        var nuevoValorTotal = valorActual - costeTotalSalida;
        var nuevaCantidad = cantidadActual - dto.Cantidad;

        var movimiento = new KardexMovimiento
        {
            ProductoId = dto.ProductoId,
            Fecha = dto.Fecha,
            Documento = dto.Documento,
            Tipo = "Salida",
            Cantidad = dto.Cantidad,
            CostoUnitario = cppActual,
            Concepto = dto.Concepto ?? "Venta"
        };

        var creado = await _repository.Add(movimiento);

        var inventario = await _inventarioService.GetByProducto(dto.ProductoId);
        if (inventario == null)
        {
            inventario = new Inventario { ProductoId = dto.ProductoId, Cantidad = cantidadActual, StockMinimo = 0, StockMaximo = 0 };
            await _inventarioService.Add(inventario);
        }
        inventario.ReducirStock(dto.Cantidad);
        await _inventarioService.Update(inventario);

        return new KardexOperacionResponseDto
        {
            Id = creado.Id,
            CppUsado = cppActual,
            CostoTotalSalida = costeTotalSalida,
            SaldoCantidad = nuevaCantidad,
            ValorSaldo = nuevoValorTotal,
            Estado = "ok"
        };
    }

    public async Task<KardexOperacionResponseDto> ActualizarMovimiento(int id, KardexUpdateDto dto)
    {
        var movimiento = await _repository.GetById(id);
        if (movimiento == null)
            return ErrorResponse("Movimiento no encontrado");

        if (dto.Concepto != null)
            movimiento.Concepto = dto.Concepto;
        if (dto.Observaciones != null)
            movimiento.Observaciones = dto.Observaciones;

        await _repository.Update(movimiento);

        return new KardexOperacionResponseDto
        {
            Id = id,
            Actualizado = true,
            Estado = "ok"
        };
    }

    public async Task<KardexOperacionResponseDto> EliminarMovimiento(int id)
    {
        var movimiento = await _repository.GetById(id);
        if (movimiento == null)
            return ErrorResponse("Movimiento no encontrado");

        var hayPosteriores = await _repository.HasMovimientosPosteriores(
            movimiento.ProductoId, id);

        if (hayPosteriores)
            return ErrorResponse("No se puede eliminar: afecta movimientos posteriores");

        await _repository.Delete(id);

        return new KardexOperacionResponseDto
        {
            Id = id,
            Eliminado = true,
            Estado = "ok"
        };
    }

    private static KardexOperacionResponseDto ErrorResponse(string mensaje)
    {
        return new KardexOperacionResponseDto
        {
            Estado = "error",
            Error = mensaje
        };
    }
}
