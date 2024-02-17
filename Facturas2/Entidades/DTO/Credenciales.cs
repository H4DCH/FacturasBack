using System.ComponentModel.DataAnnotations;

namespace Facturas2.Entidades.DTO
{
    public class Credenciales
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]  
        public string Contraseña { get; set; }  
    }
}
