using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Facturas2.Entidades
{
    public class Factura
    {
        [Key]
        public int Id { get; set; }
        public int NumeroFactura { get; set; }
        public decimal PrecioFactura { get; set; }  
        public int ProveedorId { get; set; }
        public Proveedor proveedor { get; set; }
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }   
    }
}
