using AutoMapper;
using Azure;
using Facturas2.Entidades;
using Facturas2.Entidades.DTO;
using Facturas2.Entidades.DTO.Proveedor;
using Facturas2.Entidades.DTO.Proveedore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Facturas2.Controllers.V1
{
    [ApiController]
    [Route("api/V1/proveedor")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class ProveedorController : ControllerBase
    {
        private readonly Context context;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        public ProveedorController(Context context, IMapper mapper, IAuthorizationService authorizationService)
        {
            this.context = context;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
        }
        [HttpGet(Name = "ObtenerProveedores")]
        [AllowAnonymous]
        public async Task<ColeccionRecursos<ProveedorDTO>> GetProveedor()
        {
            var proveedor = await context.Proveedores.Include(f => f.facturas).ToListAsync();
            var dtos = mapper.Map<List<ProveedorDTO>>(proveedor);

            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            dtos.ForEach(dto => Enlaces(dto, esAdmin.Succeeded));

            var resul = new ColeccionRecursos<ProveedorDTO> { Valores = dtos };

            resul.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerProveedores", new { }),
                descripcion: "Self",
                metodo: "GET"));

            if (esAdmin.Succeeded)
            {
                resul.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("CrearProveedor", new { }),
                descripcion: "Proveedor - Crear",
                metodo: "POST"));
            }

            return resul;

        }

        [HttpGet("{Id:int}", Name = "obtenerProveedor")]
        [AllowAnonymous]
        public async Task<ActionResult<ProveedorDTO>> GetId(int Id)
        {
            var proveedor = await context.Proveedores.Include(f => f.facturas).FirstOrDefaultAsync(x => x.Id == Id);

            var dto = mapper.Map<ProveedorDTO>(proveedor);
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            Enlaces(dto, esAdmin.Succeeded);

            return dto;

        }

        private void Enlaces(ProveedorDTO proveedor, bool esAdmin)
        {
            proveedor.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("obtenerProveedor", new { proveedor.Id }),
                descripcion: "Self",
                metodo: "GET"));

            if (esAdmin)
            {
                proveedor.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("ActualizarProveedor", new { proveedor.Id }),
                    descripcion: "Proveedor - Actualizar",
                    metodo: "PUT"));
                proveedor.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("BorrarProveedor", new { proveedor.Id }),
                    descripcion: "Proveedor - Borrar",
                    metodo: "DELETE"));
            }

        }

        [HttpPost(Name = "CrearProveedor")]
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

            return CreatedAtRoute("obtenerProveedor", new { proveedor.Id }, proveedorDTO);


        }

        [HttpDelete("{Id:int}", Name = "BorrarProveedor")]
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

        [HttpPut("{Id:int}", Name = "ActualizarProveedor")]
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

        [HttpPatch("{Id:int}", Name = "ActParcialProveedor")]
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