using System.ComponentModel.DataAnnotations;

namespace Facturas2.Entidades
{
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Debe digitar el nombre del proveedor")]
        public string nombreProveedor { get; set; } 
        public List<Factura> facturas { get; set; } 
    }
}
