using parcial_1.Modelos;

namespace parcial_1.Repositorios;

// Repositorio de categorías: implementa acceso a datos usando una lista estática en memoria con datos iniciales.
// Proporciona operaciones CRUD con un contador autoincremental para IDs.
public class CategoriaRepository : ICategoriaRepository
{
    private static List<Categoria> _categorias = new()
    {
        new Categoria { Id = 1, Nombre = "Electronica", Descripcion = "Productos electronicos", FechaCreacion = DateTime.Now, Activa = true },
        new Categoria { Id = 2, Nombre = "Ropa", Descripcion = "Articulos de vestimenta", FechaCreacion = DateTime.Now, Activa = true }
    };
    private static int _nextId = 3;

    public Task<Categoria?> GetById(int id)
    {
        var categoria = _categorias.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(categoria);
    }

    public Task<IEnumerable<Categoria>> GetAll()
    {
        return Task.FromResult<IEnumerable<Categoria>>(_categorias);
    }

    public Task<Categoria> Add(Categoria categoria)
    {
        categoria.Id = _nextId++;
        _categorias.Add(categoria);
        return Task.FromResult(categoria);
    }

    public Task<Categoria> Update(Categoria categoria)
    {
        var index = _categorias.FindIndex(c => c.Id == categoria.Id);
        if (index >= 0)
        {
            _categorias[index] = categoria;
        }
        return Task.FromResult(categoria);
    }

    public Task Delete(int id)
    {
        var categoria = _categorias.FirstOrDefault(c => c.Id == id);
        if (categoria != null)
        {
            _categorias.Remove(categoria);
        }
        return Task.CompletedTask;
    }
}
