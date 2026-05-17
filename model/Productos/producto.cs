namespace Inventario_Back.Model.Productos
{
    public class Producto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Codigo { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioVenta { get; set; } 
        public int categoriaId {get; set;}     
        public string? NombreCategoria {get; set;}  
    }
}