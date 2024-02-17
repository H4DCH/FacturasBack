﻿using AutoMapper;
using Facturas2.Entidades;
using Facturas2.Entidades.DTO.Factura;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facturas2.Controllers
{
    [ApiController]
    [Route("api/proveedor/{Idproveedor:int}/facturas")]
    public class FacturaController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly Context context;
        public FacturaController(Context context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }

        [HttpGet]
        public async Task<ActionResult<List<FacturaDTO>>> GetFactura(int Idproveedor)
        {
            var factura = await context.Facturas.Where(f => f.ProveedorId == Idproveedor).ToListAsync();

            if (factura == null)
            {
                return BadRequest("Id de proveedor no registrado");
            }
            return mapper.Map<List<FacturaDTO>>(factura);

        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> NewFactura(int Idproveedor, FacturaCraecionDTO facturaCreacionDTO)
        {
            var email = HttpContext.User.Claims.Where(c => c.Type == "Email").FirstOrDefault();

            var proveedorexist = await context.Proveedores.AnyAsync(p => p.Id == Idproveedor);

            if (!proveedorexist)
            {
                return NotFound();
            }
            var facturarep = await context.Facturas.FirstOrDefaultAsync(f => f.NumeroFactura == facturaCreacionDTO.NumeroFactura
            && f.ProveedorId == Idproveedor);

            if (facturarep != null)
            {
                return BadRequest($"La factura con numero : {facturaCreacionDTO.NumeroFactura} ya esta registrada");
            }
            var factura = mapper.Map<Factura>(facturaCreacionDTO);
            factura.ProveedorId = Idproveedor;

            context.Add(factura);
            await context.SaveChangesAsync();

            var facturaDTO = mapper.Map<FacturaDTO>(factura);
            return CreatedAtRoute("obtenerFacturas", new { numeroFactura = factura.NumeroFactura, Idproveedor = Idproveedor }, facturaDTO);
        }

        [HttpGet("{numeroFactura:int}", Name = "obtenerFacturas")]
        public async Task<ActionResult<FacturaDTO>> GetxId(int numeroFactura)
        {
            var facturaId = await context.Facturas.FirstOrDefaultAsync(f => f.NumeroFactura == numeroFactura);

            if (facturaId == null)
            {
                return BadRequest($"No se encontro la factura con el numero {numeroFactura}");
            }

            return mapper.Map<FacturaDTO>(facturaId);
        }

        [HttpPut("{numeroFactura:int}")]
        public async Task<ActionResult> PutFactura(int numeroFactura, int Idproveedor, FacturaUpdateDTO facturaUpdateDTO)
        {
            var existeProv = await context.Proveedores.AnyAsync(p => p.Id == Idproveedor);
            if (!existeProv)
            {
                return NotFound();
            }

            var existeFact = await context.Facturas.FirstOrDefaultAsync(f => f.NumeroFactura == numeroFactura);

            if (existeFact == null)
            {
                return NotFound();
            }

            var localExist = context.Set<Factura>().Local.FirstOrDefault(e => e.Id == existeFact.Id);

            if (localExist is not null)
            {
                context.Entry(localExist).State = EntityState.Detached;

            }
            var factura = mapper.Map<Factura>(facturaUpdateDTO);

            factura.Id = existeFact.Id;
            factura.NumeroFactura = numeroFactura;
            factura.ProveedorId = Idproveedor;

            context.Update(factura);
            await context.SaveChangesAsync();

            return NoContent();


        }

        [HttpDelete("{numeroFactura:int}")]
        public async Task<ActionResult> DeleteFactura(int numeroFactura)
        {
            var existe = await context.Facturas.FirstOrDefaultAsync(f => f.NumeroFactura==numeroFactura);

            if(existe == null)
            {
                return BadRequest();
            }

            context.Remove(existe);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
