using parcial_1.Modelos;

namespace parcial_1.DTOs;

// DTO para categoría en respuesta: incluye nombre, descripción, fecha de creación y estado activa.
public class CategoriaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public bool Activa { get; set; }
}

// DTO para crear una categoría: recibe nombre y descripción.
public class CategoriaCreateDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}

// DTO para actualizar una categoría: recibe ID, nombre, descripción y estado activa.
public class CategoriaUpdateDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activa { get; set; }
}
