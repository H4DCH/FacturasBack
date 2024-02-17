using AutoMapper;
using Facturas2.Entidades;
using Facturas2.Entidades.DTO.Factura;
using Facturas2.Entidades.DTO.Proveedor;
using Facturas2.Entidades.DTO.Proveedore;

namespace Facturas2.Profiles
{
    public class AutoMapping : Profile
    {
        public AutoMapping() 
        {
            CreateMap<ProveedorCreacionDTO, Proveedor>();
            CreateMap<Proveedor,ProveedorDTO>();
            CreateMap<FacturaCraecionDTO, Factura>();
            CreateMap<Factura, FacturaDTO>();
            CreateMap<FacturaUpdateDTO, Factura>();
            CreateMap<Proveedor, ProveedorPatchDTO>().ReverseMap();
   
        }    
    }
}
