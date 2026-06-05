using parcial_1.Modelos;

namespace parcial_1.Repositorios;

// Interfaz del repositorio de categorías: define el contrato para acceso a datos de categorías.
public interface ICategoriaRepository
{
    Task<Categoria?> GetById(int id);
    Task<IEnumerable<Categoria>> GetAll();
    Task<Categoria> Add(Categoria categoria);
    Task<Categoria> Update(Categoria categoria);
    Task Delete(int id);
}