namespace BikeStore.Web.Models
{
    public class DetalleVenta
    {
        public int IdDetalle { get; set; }
        public int IdBicicleta { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal SubTotal { get; set; }
        public Bicicleta? Bicicleta { get; set; }
    }
}
