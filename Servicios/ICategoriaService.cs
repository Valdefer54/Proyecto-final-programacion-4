using parcial_1.Modelos;

namespace parcial_1.Servicios;

// Interfaz del servicio de categorías: define el contrato para operaciones CRUD de categorías.
public interface ICategoriaService
{
    Task<Categoria?> GetById(int id);
    Task<IEnumerable<Categoria>> GetAll();
    Task<Categoria> Add(Categoria categoria);
    Task<Categoria> Update(Categoria categoria);
    Task Delete(int id);
}