using Facturas2.Entidades.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Facturas2.Controllers.V1
{
    [ApiController]
    [Route("api/V1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [HttpGet(Name = "ObtenerRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatoHATEOAS>>> Get()
        {
            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            var HATEOS = new List<DatoHATEOAS>();

            HATEOS.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerRoot", new { })
            , descripcion: "self", metodo: "GET"));

            if (esAdmin.Succeeded)
            {
                HATEOS.Add(new DatoHATEOAS(enlace: Url.Link("ObtenerProveedores", new { }),
                    descripcion: "proveedores", metodo: "GET"));
                HATEOS.Add(new DatoHATEOAS(enlace: Url.Link("CrearProveedor", new { }),
                    descripcion: "proveedor-crear", metodo: "POST"));
            }
            return HATEOS;
        }
    }
}
