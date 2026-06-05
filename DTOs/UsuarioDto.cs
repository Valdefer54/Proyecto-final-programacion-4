using parcial_1.Modelos;

namespace parcial_1.DTOs;

// DTO para usuario en respuesta: incluye nombre, email, rol, fecha de creación y estado activo.
public class UsuarioDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Rol Rol { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }
}

// DTO para crear un usuario: recibe nombre, email, password y rol.
public class UsuarioCreateDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Rol Rol { get; set; }
}

// DTO para actualizar un usuario: recibe ID, nombre, email, rol y estado activo.
public class UsuarioUpdateDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Rol Rol { get; set; }
    public bool Activo { get; set; }
}
