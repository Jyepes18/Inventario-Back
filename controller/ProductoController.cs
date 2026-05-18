using Microsoft.AspNetCore.Mvc;
using Inventario_Back.Model.Productos;
using Inventario_Back.interfaces;

namespace Inventario_Back.Model.Productos
{
    [ApiController]
    [Route("[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpPost]
        [Route("crear")]
        public async Task<IActionResult> CrearProductoAsync ([FromBody] Producto producto)
        {
            var result = await _productoService.CrearProducto(producto);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> ObtenerProductoAsyn([FromRoute]int Id)
        {
            var result = await _productoService.ObtenerProducto(Id);
            return Ok(result);
        }

        [HttpPost]
        [Route("paginados/{page}/{pageSize}")]
        public async Task<IActionResult> ObtenerListadoPaginadosProductos([FromRoute] int page, [FromRoute] int pageSize, [FromBody] ProductoFilter productoFilter)
        {
            var result = await _productoService.ObtenerListadoProductos(page, pageSize, productoFilter);
            return Ok(new
            {
                productos = result.Item1,
                total = result.total
            });
        }

        [HttpDelete]
        [Route("eliminar/{Id}")]
        public async Task<IActionResult> EliminarProducto([FromRoute] int Id)
        {
            var result = await _productoService.EliminarProducto(Id);
            return Ok(result);
        }

        [HttpPost]
        [Route("actualizar")]
        public async Task<IActionResult> ActualizarProductoAsync ([FromBody] Producto producto)
        {
            var result = await _productoService.ActualizarProducto(producto);
            return Ok(result);
        }
    }   
}