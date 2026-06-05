using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using parcial_1.DTOs;
using parcial_1.Servicios;

namespace parcial_1.Controladores;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class KardexController : ControllerBase
{
    private readonly IKardexService _kardexService;

    public KardexController(IKardexService kardexService)
    {
        _kardexService = kardexService;
    }

    [HttpGet("{productoId}")]
    public async Task<IActionResult> GetKardex(int productoId)
    {
        var result = await _kardexService.GetKardexByProducto(productoId);
        return Ok(result);
    }

    [HttpGet("{productoId}/saldo")]
    public async Task<IActionResult> GetSaldo(int productoId)
    {
        var result = await _kardexService.GetSaldoByProducto(productoId);
        return Ok(result);
    }

    [HttpPost("entrada")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> RegistrarEntrada([FromBody] KardexEntradaRequestDto dto)
    {
        var result = await _kardexService.RegistrarEntrada(dto);
        if (result.Estado == "error")
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("{movimientoId}")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> ActualizarMovimiento(int movimientoId, [FromBody] KardexUpdateDto dto)
    {
        var result = await _kardexService.ActualizarMovimiento(movimientoId, dto);
        if (result.Estado == "error")
            return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("{movimientoId}")]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> EliminarMovimiento(int movimientoId)
    {
        var result = await _kardexService.EliminarMovimiento(movimientoId);
        if (result.Estado == "error")
            return BadRequest(result);
        return Ok(result);
    }
}
