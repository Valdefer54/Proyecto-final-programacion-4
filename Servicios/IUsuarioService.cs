using parcial_1.Modelos;

namespace parcial_1.Servicios;

// Interfaz del servicio de usuarios: define el contrato para operaciones CRUD y búsqueda por email de usuarios.
public interface IUsuarioService
{
    Task<Usuario?> GetById(int id);
    Task<Usuario?> GetByEmail(string email);
    Task<IEnumerable<Usuario>> GetAll();
    Task<Usuario> Add(Usuario usuario);
    Task<Usuario> Update(Usuario usuario);
    Task Delete(int id);
}