using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using parcial_1.DTOs;
using parcial_1.Modelos;
using parcial_1.Servicios;

namespace parcial_1.Controladores;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IConfiguration _configuration;

    public AuthController(IUsuarioService usuarioService, IConfiguration configuration)
    {
        _usuarioService = usuarioService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var usuario = await _usuarioService.GetByEmail(loginDto.Email);
        if (usuario == null || usuario.PasswordHash != loginDto.Password)
        {
            return Unauthorized(new { mensaje = "Credenciales invalidas" });
        }

        if (!usuario.Activo)
        {
            return Unauthorized(new { mensaje = "Usuario desactivado" });
        }

        var dto = usuario.Adapt<UsuarioDto>();
        var token = GenerarToken(usuario);
        return Ok(new
        {
            token,
            usuario = dto
        });
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UsuarioCreateDto dto)
    {
        var existente = await _usuarioService.GetByEmail(dto.Email);
        if (existente != null)
        {
            return BadRequest(new { mensaje = "El email ya esta registrado" });
        }

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            PasswordHash = dto.Password,
            Rol = dto.Rol
        };

        var creado = await _usuarioService.Add(usuario);
        return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado.Adapt<UsuarioDto>());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var usuario = await _usuarioService.GetById(id);
        if (usuario == null) return NotFound();

        return Ok(usuario.Adapt<UsuarioDto>());
    }

    [HttpGet]
    [Authorize(Policy = "Administrador")]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _usuarioService.GetAll();
        var dtos = usuarios.Adapt<IEnumerable<UsuarioDto>>();
        return Ok(dtos);
    }

    private string GenerarToken(Usuario usuario)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "ClaveSuperSecretaParaJwt2024!Minimo32Caracteres");
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
