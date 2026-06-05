using parcial_1.Modelos;

namespace parcial_1.DTOs;

// DTO para producto en respuesta: incluye nombre, descripción, precio, categoría y estado activo.
public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string? CategoriaNombre { get; set; }
    public DateTime FechaCreacion { get; set; }
    public bool Activo { get; set; }
}

// DTO para crear un producto: recibe nombre, descripción, precio e ID de categoría.
public class ProductoCreateDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int CategoriaId { get; set; }
}

// DTO para actualizar un producto: recibe ID, nombre, descripción, precio, categoría y estado activo.
public class ProductoUpdateDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int CategoriaId { get; set; }
    public bool Activo { get; set; }
}

// DTO para actualizar solo el precio de un producto.
public class ProductoPrecioUpdateDto
{
    public decimal NuevoPrecio { get; set; }
}
