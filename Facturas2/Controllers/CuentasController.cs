using Facturas2.Entidades.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Facturas2.Controllers
{

    [ApiController]
    [Route("api/cuentas")]

    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacion>> Registro(Credenciales credenciales)
        {
            var usuario = new IdentityUser { UserName = credenciales.Email,
                Email = credenciales.Email };

            var resultado = await userManager.CreateAsync(usuario, credenciales.Contraseña);

            if (resultado.Succeeded)
            {
                return ConstruirToken(credenciales);

            }
            else
            {
                return BadRequest(resultado.Errors);
            }

        }
        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(Credenciales credenciales)
        {
            var res = await signInManager.PasswordSignInAsync(credenciales.Email, credenciales.Contraseña,
                isPersistent:false, lockoutOnFailure : false);

            if (res.Succeeded)
            {
                return ConstruirToken(credenciales);
            }
            else
            {
                return BadRequest("Credenciales Inconrrectas");
            }

        }


        private RespuestaAutenticacion ConstruirToken(Credenciales credenciales)
        {
            var claims = new List<Claim>()
            {
                new Claim("Email",credenciales.Email)
            };
            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Llave"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var security = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(security),
                Expiracion = expiracion
            };
        }


    }
}
