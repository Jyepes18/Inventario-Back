using Inventario_Back.Model.Productos;

namespace Inventario_Back.interfaces
{
    public interface IProductoService
    {
        Task<int> CrearProducto(Producto producto); 
        Task<Producto> ObtenerProducto(int Id);
    }
}