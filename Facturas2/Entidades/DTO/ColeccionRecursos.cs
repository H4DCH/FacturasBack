using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Facturas2.Entidades.DTO
{
    public class ColeccionRecursos<T> : Recurso where T : Recurso
    {
        public List<T> Valores { get; set; }
    }
}
