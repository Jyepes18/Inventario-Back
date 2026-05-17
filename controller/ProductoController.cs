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


    }   
}