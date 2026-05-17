using Dapper;
using Inventario_Back.interfaces;
using Inventario_Back.Model.Productos;
using Npgsql;

namespace Inventario_Back.services
{
    public class ProductoService : IProductoService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connection;

        public ProductoService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = _configuration["ConnectionDB:connectioString"] ?? throw new Exception("Error al obtener conexion");

        }


        /// <summary>
        /// Crear un producto 
        /// </summary>
        /// <param name="producto">Modelo con los datos necesarios para crear producto</param>
        /// <returns>entero que si es mayor a 1 se devolvera un 201 (creado) y si no un 400 (error)</returns>
        public async Task<int> CrearProducto(Producto producto)
        {
            try
            {
                string insertarProducto = @"
                    insert into producto (nombre, descripcion, codigo, cantidad, precio, precioventa, categoriaid)
                    values (@nombre, @descripcion, @codigo, @cantidad, @precio, @precioventa, @categoriaid)
                ";

                await using var connection = new NpgsqlConnection(_connection);
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync(insertarProducto, new
                {
                    nombre = producto.Nombre, 
                    descripcion = producto.Descripcion, 
                    codigo = producto.Codigo, 
                    cantidad = producto.Cantidad, 
                    precio = producto.Precio, 
                    precioventa = producto.PrecioVenta,
                    categoriaid = producto.categoriaId
                });

                await connection.DisposeAsync();

                if(result > 0)
                    return 201;

                return 400;
            }catch(Exception ex)
            {
                throw new Exception("Error al generar producto::CrearProducto ", ex);
            }
        }

        public async Task<Producto> ObtenerProducto(int Id)
        {
            try
            {
                string buscarProducto = @"
                    select 
                    p.id as Id,
                    p.nombre as Nombre, 
                    p.descripcion as Descripcion, 
                    p.codigo as Codigo, 
                    p.cantidad as Cantidad, 
                    p.precio as Precio, 
                    p.precioventa as PrecioVenta, 
                    c.id as categoriaId, 
                    c.nombre as NombreCategoria   
                    from producto p 
                    inner join categoria c ON c.id = p.categoriaid 
                    where p.id = @id
                ";

                await using var connection = new NpgsqlConnection(_connection);
                await connection.OpenAsync();


                var result = await connection.QueryFirstOrDefaultAsync<Producto>(buscarProducto, new
                {
                   id = Id 
                });

                if(result != null)
                    return result;

                return null;
                
            }catch(Exception ex)
            {
                throw new Exception("Erro al obtener producto por id::ObtenerProducto ", ex);
            }
        }
    }
}