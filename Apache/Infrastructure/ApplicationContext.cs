using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace Apache.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
        {
            try
            {
                Database.EnsureCreated();
            }
            catch
            {
                MessageBox.Show("Нет подключения к базе");
            }
        }
        public DbSet<Categories>? Categories { get; set; }
        public DbSet<Checks>? Checks { get; set; }
        public DbSet<CheckDetails>? CheckDetails { get; set; }
        public DbSet<Places>? Places { get; set; }
        public DbSet<Products>? Products { get; set; }
        public DbSet<ProductsPrihod>? ProductsPrihod { get; set; }
        public DbSet<Staffs>? Staffs { get; set; }
        public DbSet<Settings>? Settings { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySql("server=localhost;user=root;password=dimalox;database=apachiDB;",
        //         new MySqlServerVersion(new Version(5, 7, 39))
        //     );
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var stri = "server=192.168.8.67;user=root;password=;database=apachiDB;";
            var autoVersion = ServerVersion.AutoDetect(stri);
            optionsBuilder.UseMySql(stri, autoVersion
             );
        }
    }
}
