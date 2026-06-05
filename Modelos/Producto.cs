namespace parcial_1.Modelos;

// Modelo de producto: representa un artículo vendible con nombre, descripción, precio y categoría.
// Permite actualizar precio, nombre y activar/desactivar el producto.
public class Producto : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
    public bool Activo { get; set; }

    public void ActualizarPrecio(decimal nuevoPrecio) => Precio = nuevoPrecio;
    public void ActualizarNombre(string nombre) => Nombre = nombre;
    public void Desactivar() => Activo = false;
    public void Activar() => Activo = true;
}