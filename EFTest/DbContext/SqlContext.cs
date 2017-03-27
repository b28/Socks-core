namespace EFTest.DbContext
{
    using System.Data.Entity;

    public class SqlContext : DbContext
    {
        // Your context has been configured to use a 'SqlContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'EFTest.DbContext.SqlContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SqlContext' 
        // connection string in the application configuration file.
        public SqlContext()
            : base("name=SqlContext")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Action> Actions { get; set; }
        //public DbSet<Box> Boxes { get; set; }


        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}