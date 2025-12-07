using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cuentas.Models;

namespace Cuentas.Data
{
    public class CuentasContext : DbContext
    {
        public CuentasContext (DbContextOptions<CuentasContext> options)
            : base(options)
        {
        }

        public DbSet<Cuentas.Models.Cuenta> Cuenta { get; set; } = default!;
    }
}
