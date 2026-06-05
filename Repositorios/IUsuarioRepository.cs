using parcial_1.Modelos;

namespace parcial_1.Repositorios;

// Interfaz del repositorio de usuarios: define el contrato para acceso a datos de usuarios.
public interface IUsuarioRepository
{
    Task<Usuario?> GetById(int id);
    Task<Usuario?> GetByEmail(string email);
    Task<IEnumerable<Usuario>> GetAll();
    Task<Usuario> Add(Usuario usuario);
    Task<Usuario> Update(Usuario usuario);
    Task Delete(int id);
}