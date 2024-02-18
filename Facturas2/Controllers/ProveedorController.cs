using AutoMapper;
using Azure;
using Facturas2.Entidades;
using Facturas2.Entidades.DTO.Proveedor;
using Facturas2.Entidades.DTO.Proveedore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Facturas2.Controllers
{
    [ApiController]
    [Route("api/proveedor")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Policy ="EsAdmin")]
    public class ProveedorController : ControllerBase
    {
        private readonly Context context;
        private readonly IMapper mapper;
        public ProveedorController(Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;

        }
        [HttpGet]
        public async Task<ActionResult<List<ProveedorDTO>>> GetProveedor()
        {
            var proveedor = await context.Proveedores.Include(f => f.facturas).ToListAsync();
            return mapper.Map<List<ProveedorDTO>>(proveedor);
        }

        [HttpGet("{Id:int}", Name = "obtenerProveedor")]
        public async Task<ActionResult<ProveedorDTO>> GetId(int Id)
        {
            var autor = await context.Proveedores.Include(f => f.facturas).FirstOrDefaultAsync(x => x.Id == Id);

            return mapper.Map<ProveedorDTO>(autor);
        }

        [HttpPost]
        public async Task<ActionResult> NewProveedor(ProveedorCreacionDTO proveedorCreacionDTO)
        {
            var resul = await context.Proveedores.AnyAsync(x => x.nombreProveedor == proveedorCreacionDTO.nombreProveedor);

            if (resul)
            {
                return BadRequest("El nombre digitado ya esta registrado");
            }

            var proveedor = mapper.Map<Proveedor>(proveedorCreacionDTO);
            context.Add(proveedor);
            await context.SaveChangesAsync();

            var proveedorDTO = mapper.Map<ProveedorDTO>(proveedor);

            return CreatedAtRoute("obtenerProveedor", new { Id = proveedor.Id }, proveedorDTO);


        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> DeleteProveedor(int Id)
        {
            var existePro = await context.Proveedores.FirstOrDefaultAsync(p => p.Id == Id);

            if (existePro == null)
            {
                return BadRequest();
            }
            context.Remove(existePro);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> PutProveedor(ProveedorCreacionDTO proveedorCreacionDTO, int Id)
        {
            var existe = await context.Proveedores.AnyAsync(p => p.Id == Id);

            if (!existe)
            {
                return NotFound();
            }

            var autor = mapper.Map<Proveedor>(proveedorCreacionDTO);
            autor.Id = Id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{Id:int}")]
        public async Task<ActionResult> PacthProveedor(int Id, JsonPatchDocument<ProveedorPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var proveedor = await context.Proveedores.FirstOrDefaultAsync(p => p.Id == Id);

            if (proveedor == null)
            {
                return NotFound();
            }

            var proveedorDTO = mapper.Map<ProveedorPatchDTO>(proveedor);

            patchDocument.ApplyTo(proveedorDTO, ModelState);

            var esValido = TryValidateModel(proveedorDTO);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(proveedorDTO, proveedor);

            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}