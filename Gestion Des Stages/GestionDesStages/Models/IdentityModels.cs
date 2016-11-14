using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GestionDesStages.Models
{
    // Vous pouvez ajouter des données de profil pour l'utilisateur en ajoutant plus de propriétés à votre classe ApplicationUser ; consultez http://go.microsoft.com/fwlink/?LinkID=317594 pour en savoir davantage.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Notez qu'authenticationType doit correspondre à l'élément défini dans CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Ajouter les revendications personnalisées de l’utilisateur ici
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<GestionDesStages.Models.Stage> Stages { get; set; }

        public System.Data.Entity.DbSet<GestionDesStages.Models.Encadrant> Encadrants { get; set; }

        public System.Data.Entity.DbSet<GestionDesStages.Models.Departement> Departements { get; set; }
        public System.Data.Entity.DbSet<GestionDesStages.Models.Stagiaire> Stagiaires { get; set; }
        public System.Data.Entity.DbSet<GestionDesStages.Models.Bureau> Bureaux { get; set; }
        public System.Data.Entity.DbSet<GestionDesStages.Models.Sujet> Sujets { get; set; }

    }
}