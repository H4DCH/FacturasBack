using Facturas2.Entidades.DTO.Proveedor;
using Facturas2.Entidades.DTO.Proveedore;
using System.ComponentModel.DataAnnotations;

namespace Facturas2.Entidades.DTO.Factura
{
    public class FacturaCraecionDTO
    {
        [Required(ErrorMessage ="Debe ingresar el numero de la factura")]
        public int NumeroFactura { get; set; }
        public decimal PrecioFactura { get; set; }
    }
}
