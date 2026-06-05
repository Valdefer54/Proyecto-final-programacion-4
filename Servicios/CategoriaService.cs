using parcial_1.Modelos;
using parcial_1.Repositorios;

namespace parcial_1.Servicios;

// Servicio de categorías: implementa la lógica de negocio para operaciones con categorías.
// Establece fecha de creación y estado activa al crear una nueva categoría.
public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public Task<Categoria?> GetById(int id) => _categoriaRepository.GetById(id);
    public Task<IEnumerable<Categoria>> GetAll() => _categoriaRepository.GetAll();

    public Task<Categoria> Add(Categoria categoria)
    {
        categoria.FechaCreacion = DateTime.Now;
        categoria.Activa = true;
        return _categoriaRepository.Add(categoria);
    }

    public Task<Categoria> Update(Categoria categoria) => _categoriaRepository.Update(categoria);
    public Task Delete(int id) => _categoriaRepository.Delete(id);
}
