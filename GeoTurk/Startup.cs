using GeoTurk.Migrations;
using GeoTurk.Models;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(GeoTurk.Startup))]
namespace GeoTurk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<GeoTurkDbContext, Configuration>());

            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);

            if (migrator.GetPendingMigrations().Any())
            {
                migrator.Update();
   
            }

            ConfigureAuth(app);
        }

    }
}
