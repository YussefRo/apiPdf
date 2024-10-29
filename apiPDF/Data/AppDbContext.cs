using apiPDF.Models;
using Microsoft.EntityFrameworkCore;

namespace apiPDF.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Tb_detalle_demoras> Tb_Detalle_Demoras { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tb_detalle_demoras>().ToTable("TB_DETALLE_DEMORAS"); 
        }
    }
}
