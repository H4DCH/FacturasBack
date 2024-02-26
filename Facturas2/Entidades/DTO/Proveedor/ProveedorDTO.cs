
using Facturas2.Entidades.DTO.Factura;

namespace Facturas2.Entidades.DTO.Proveedor
{
    public class ProveedorDTO : Recurso
    {
        public int Id { get; set; }
        public string nombreProveedor { get; set; } = string.Empty;
        public List<FacturaDTO> facturas { get; set; }
    }
}
