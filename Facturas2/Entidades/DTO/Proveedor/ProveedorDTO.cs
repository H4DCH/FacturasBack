
using Facturas2.Entidades.DTO.Factura;

namespace Facturas2.Entidades.DTO.Proveedor
{
    public class ProveedorDTO
    {
        public int Id { get; set; }
        public string nombreProveedor { get; set; }
        public List<FacturaDTO> facturas { get; set; }
    }
}
