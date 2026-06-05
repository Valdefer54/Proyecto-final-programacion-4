using parcial_1.Modelos;
using parcial_1.Repositorios;

namespace parcial_1.Servicios;

// Servicio de usuarios: implementa la lógica de negocio para operaciones con usuarios.
// Establece fecha de creación y estado activo al registrar un nuevo usuario.
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public Task<Usuario?> GetById(int id) => _usuarioRepository.GetById(id);
    public Task<Usuario?> GetByEmail(string email) => _usuarioRepository.GetByEmail(email);
    public Task<IEnumerable<Usuario>> GetAll() => _usuarioRepository.GetAll();

    public Task<Usuario> Add(Usuario usuario)
    {
        usuario.FechaCreacion = DateTime.Now;
        usuario.Activo = true;
        return _usuarioRepository.Add(usuario);
    }

    public Task<Usuario> Update(Usuario usuario) => _usuarioRepository.Update(usuario);
    public Task Delete(int id) => _usuarioRepository.Delete(id);
}
