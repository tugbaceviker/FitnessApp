using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using FitnessApp1;


namespace DataAccessLayer.Concrete
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=serversamethan.database.windows.net;Database=FitnessDataBase;User Id=samethankazanbas;Password=5322192Sk.;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
           
            modelBuilder.Entity<antrenor1>().ToTable("antrenor1");
            modelBuilder.Entity<kullanici>().ToTable("kullanici");
            modelBuilder.Entity<beslenme_programi>().ToTable("beslenme_programi");
            modelBuilder.Entity<egzersiz_programi>().ToTable("egzersiz_programi");
            modelBuilder.Entity<guncel_versiyon>().ToTable("guncel_versiyon");
            modelBuilder.Entity<mesaj>().ToTable("mesaj");






            // Diğer konfigürasyonlar
        }

        public DbSet<kullanici> Kullanicilar { get; set; }

        public DbSet<antrenor1> Antrenorler { get; set; }

        public DbSet<beslenme_programi> BeslenmeProgramlari { get; set; }

        public DbSet<egzersiz_programi> EgzersizProgramlari { get; set; }

        public DbSet<guncel_versiyon> GuncelVersiyon { get; set; }
        public DbSet<mesaj> Mesajlarım { get; set; }


    }
}
