namespace parcial_1.Modelos;

// Modelo de categoría: representa una agrupación de productos con nombre y descripción.
// Permite actualizar nombre, descripción y activar/desactivar la categoría.
public class Categoria : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public bool Activa { get; set; }

    public void ActualizarNombre(string nombre) => Nombre = nombre;
    public void ActualizarDescripcion(string descripcion) => Descripcion = descripcion;
    public void Desactivar() => Activa = false;
    public void Activar() => Activa = true;
}