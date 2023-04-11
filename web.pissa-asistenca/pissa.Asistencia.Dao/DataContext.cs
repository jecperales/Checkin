using MySql.Data.Entity;
using pissa.Asistencia.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pissa.Asistencia.Dao
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DataContext: DbContext
    {

        //public DataContext() : base("Data Source = 127.0.0.1; Port=3306;Initial Catalog = assistence_management; Persist Security Info=True;User ID = root; Password=personal;")
        //{

        //}

        //public DataContext() : base("Data Source=172.16.1.60;Port=3306;Initial Catalog=assistence_management;Persist Security Info=True;User ID=asispissa;Password=asispissa123#.;")
        //{

        //}

        //public DataContext() : base("Data Source = 172.16.102.254; Port=3306;Initial Catalog = assistence_management; Persist Security Info=True;User ID = asitenciaPissa; Password=asitencia1.;")
        //{

        //}

        //public DataContext() : base("Data Source = 189.195.136.238; Port=3306;Initial Catalog = assistence_management; Persist Security Info=True;User ID = asitenciaPissa; Password=asitencia1.;")
        //{

        //}

        //SERVIDOR VIRTUAL DE TOTAL PLAY
        public DataContext() : base("Data Source = 189.203.240.97; Port=3306;Initial Catalog = assistence_management; Persist Security Info=True;User ID = monitorWeb; Password=mWebGrp#.;")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //DONT DO THIS ANYMORE
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Vote>().ToTable("Votes")
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }


        public DbSet<control_asistencia> control_asistencia { get; set; }

        public DbSet<ingeniero> ingeniero { get; set; }

        public DbSet<movil> movil { get; set; }

        public DbSet<privilegios> privilegios { get; set; }

        public DbSet<proyecto> proyecto { get; set; }

        public DbSet<user> user { get; set; }

        public DbSet<catalogo_celulares> catalogo_celulares { get; set; }

        public DbSet<country> country { get; set; }


    }
}
