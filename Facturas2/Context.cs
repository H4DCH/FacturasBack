using Facturas2.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Facturas2
{
    public class Context : IdentityDbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
       public  DbSet<Proveedor> Proveedores { get; set; }
       public  DbSet<Factura> Facturas { get; set; }
    }
}
