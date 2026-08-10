using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStore.Datos.Models
{
    [Table("Detalle_Venta")]
    public class DetalleVenta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdDetalle { get; set; }

        [Required]
        public int IdVenta { get; set; }

        [Required]
        public int IdBicicleta { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal SubTotal { get; set; }

        // Atributos de navegación
        [ForeignKey("IdVenta")]
        public virtual Venta? Venta { get; set; }

        [ForeignKey("IdBicicleta")]
        public virtual Bicicleta? Bicicleta { get; set; }
    }
}
