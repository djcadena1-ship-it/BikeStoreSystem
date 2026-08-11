using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStore.Datos.Models
{
    [Table("Bicicleta")]
    public class Bicicleta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdBicicleta { get; set; }
        
        [Required]
        public int IdCategoria { get; set; }

        [Required]
        [StringLength(50)]
        public string Marca { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Modelo { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = null!;
    }
}
