namespace BikeStore.Web.Models
{
    public class RegistrarVentaRequest
    {
        public int IdCliente { get; set; }
        public List<DetalleVentaRequest> Detalles { get; set; } = new();
    }

    public class DetalleVentaRequest
    {
        public int IdBicicleta { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
