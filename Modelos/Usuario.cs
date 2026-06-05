namespace parcial_1.Modelos;

// Modelo de usuario: representa un usuario del sistema con nombre, email, password y rol.
// Permite actualizar email, password, rol y activar/desactivar la cuenta.
public class Usuario : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Rol Rol { get; set; }
    public bool Activo { get; set; }

    public void ActualizarEmail(string nuevoEmail) => Email = nuevoEmail;
    public void CambiarPassword(string nuevoHash) => PasswordHash = nuevoHash;
    public void CambiarRol(Rol nuevoRol) => Rol = nuevoRol;
    public void Desactivar() => Activo = false;
    public void Activar() => Activo = true;
}