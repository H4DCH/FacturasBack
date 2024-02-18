using System.ComponentModel.DataAnnotations;

namespace Facturas2.Entidades.DTO.Usuario
{
    public class EditarAdminDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
