using Dapper;
using Inventario_Back.interfaces;
using Inventario_Back.Model;
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

        /// <summary>
        /// este metodo devuelve la informacion de producto
        /// </summary>
        /// <param name="Id">identificador unico del producto</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Obtiene listado paginados de los productos
        /// </summary>
        /// <param name="page">Numero de pagina</param>
        /// <param name="pageSize">cantidad de resgistros de dicha pagina</param>
        /// <param name="productoFilter">filtro que solo se fiultra por nombre</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<(ICollection<Producto>, int total)> ObtenerListadoProductos(int page, int pageSize, ProductoFilter productoFilter)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_connection);
                await connection.OpenAsync();

                string contarProductos = @"
                    select COUNT(p.*) from producto p where (@Nombre is null or p.nombre ilike '%' || @Nombre || '%')
                ";
                
                var totalProductos = await connection.QueryAsync<int>(contarProductos, new
                {
                    Nombre = productoFilter.Nombre ?? null
                });

                string obtenerDatos = @"
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
                    where (@Nombre is null or p.nombre ilike '%' || @Nombre || '%')
                    order by id desc
                    LIMIT @PageSize
                    OFFSET @Offset
                ";

                var Offset = (page - 1) * pageSize;
                        
                var listadoProductos = await  connection.QueryAsync<Producto>(obtenerDatos, new
                {
                    Offset = Offset,
                    PageSize = pageSize,
                    Nombre = productoFilter.Nombre ?? null
                });

                await connection.DisposeAsync();

                return (listadoProductos.ToList(), totalProductos.Single());

            }catch(Exception ex)
            {
                throw new Exception("Error al obtener listado ", ex);
            }
        }


        /// <summary>
        /// Elimina un producto 
        /// </summary>
        /// <param name="Id">Identificador del usuario</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> EliminarProducto(int Id)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_connection);
                await connection.OpenAsync();

                string eliminarProducto = @"
                    delete from producto where id = @Id;
                ";
                
                var ProductoEliminado = await connection.ExecuteAsync(eliminarProducto, new
                {
                    Id = Id
                });

                await connection.DisposeAsync();
                
                if(ProductoEliminado > 0)
                    return 200;


                return 404;

            }catch(Exception ex)
            {
                throw new Exception("Error al obtener listado ", ex);
            }
        }
    }
}