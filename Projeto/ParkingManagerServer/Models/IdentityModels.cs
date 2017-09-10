using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
namespace ParkingManagerServer.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }


    

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }
       
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           

            modelBuilder.Entity<UsuarioModel>()
                .HasMany<VeiculoModel>(s => s.Veiculos)
                .WithMany(c => c.Usuarios)
                .Map(cs =>
                {
                    cs.MapLeftKey("UsuarioRefId");
                    cs.MapRightKey("VeiculoRefId");
                    cs.ToTable("UsuarioVeiculo");
                });
            // modelBuilder.Entity<VagaModel>().HasOptional<UsuarioModel>(v => v.Responsavel).WithRequired();
            modelBuilder.Entity<VagaModel>().HasOptional(i => i.Responsavel).WithOptionalDependent();
            modelBuilder.Entity<EstacionamentoModel>().HasMany<PontoModel>(i => i.Pontos).WithRequired();
            
            modelBuilder.Entity<PontoModel>().HasMany(i => i.PontosConectados).WithOptional();
            modelBuilder.Entity<PontoModel>().HasMany(i => i.VagasConectadas).WithOptional();

        }

        public System.Data.Entity.DbSet<ParkingManagerServer.Models.VeiculoModel> VeiculoModels { get; set; }

        public System.Data.Entity.DbSet<ParkingManagerServer.Models.UsuarioModel> UsuarioModels { get; set; }

        public System.Data.Entity.DbSet<ParkingManagerServer.Models.VagaModel> VagaModels { get; set; }

        public System.Data.Entity.DbSet<ParkingManagerServer.Models.OcupacaoModel> OcupacaoModels { get; set; }

        public System.Data.Entity.DbSet<ParkingManagerServer.Models.ReservaModel> ReservaModels { get; set; }

        public System.Data.Entity.DbSet<ParkingManagerServer.Models.EstacionamentoModel> EstacionamentoModels { get; set; }
        public System.Data.Entity.DbSet<ParkingManagerServer.Models.PontoModel> PontoModels { get; set; }

    }
}