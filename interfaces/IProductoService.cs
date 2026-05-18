using Inventario_Back.Model;
using Inventario_Back.Model.Productos;

namespace Inventario_Back.interfaces
{
    public interface IProductoService
    {
        Task<int> CrearProducto(Producto producto); 
        Task<Producto> ObtenerProducto(int Id);
        Task<(ICollection<Producto>, int total)> ObtenerListadoProductos(int page, int pageSize, ProductoFilter productoFilter);
        Task<int> EliminarProducto(int Id);
        Task<int> ActualizarProducto(Producto producto);
    }
}