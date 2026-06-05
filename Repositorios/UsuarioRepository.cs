using parcial_1.Modelos;

namespace parcial_1.Repositorios;

// Repositorio de usuarios: implementa acceso a datos usando una lista estática en memoria con un usuario admin inicial.
// Proporciona operaciones CRUD y búsqueda por email con un contador autoincremental para IDs.
public class UsuarioRepository : IUsuarioRepository
{
    private static List<Usuario> _usuarios = new()
    {
        new Usuario { Id = 1, Nombre = "Admin", Email = "admin@test.com", PasswordHash = "admin123", Rol = Rol.Administrador, FechaCreacion = DateTime.Now, Activo = true }
    };
    private static int _nextId = 2;

    public Task<Usuario?> GetById(int id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(usuario);
    }

    public Task<Usuario?> GetByEmail(string email)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Email == email);
        return Task.FromResult(usuario);
    }

    public Task<IEnumerable<Usuario>> GetAll()
    {
        return Task.FromResult<IEnumerable<Usuario>>(_usuarios);
    }

    public Task<Usuario> Add(Usuario usuario)
    {
        usuario.Id = _nextId++;
        _usuarios.Add(usuario);
        return Task.FromResult(usuario);
    }

    public Task<Usuario> Update(Usuario usuario)
    {
        var index = _usuarios.FindIndex(u => u.Id == usuario.Id);
        if (index >= 0)
        {
            _usuarios[index] = usuario;
        }
        return Task.FromResult(usuario);
    }

    public Task Delete(int id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        if (usuario != null)
        {
            _usuarios.Remove(usuario);
        }
        return Task.CompletedTask;
    }
}
