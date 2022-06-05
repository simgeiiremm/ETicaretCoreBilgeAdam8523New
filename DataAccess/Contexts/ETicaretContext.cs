using AppCore.DataAccess.Configs;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts
{
    public class ETicaretContext : DbContext
    {
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Kategori> Kategoriler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Rol> Roller { get; set; }
        public DbSet<KullaniciDetayi> KullaniciDetaylari { get; set; }
        public DbSet<Ulke> Ulkeler { get; set; }
        public DbSet<Sehir> Sehirler { get; set; }
        public DbSet<Magaza> Magazalar { get; set; }
        public DbSet<UrunMagaza> UrunMagazalar { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Windows Authentication
            //string connectionString = "server=.;database=BA_ETicaretCore8523;trusted_connection= true;multipleactiveresultsets=true;";

            // SQL Server Authentication
            string connectionString = "server=.;database=BA_ETicaretCore8523;user id=sa;password=123;multipleactiveresultsets=true;";

            if(!string.IsNullOrWhiteSpace(ConnectionConfig.ConnectionString))
                connectionString = ConnectionConfig.ConnectionString;

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Urun>()
                //.ToTable("ETicaretUrunler")
                .HasOne(urun => urun.Kategori)
                .WithMany(kategori => kategori.Urunler)
                .HasForeignKey(urun => urun.KategoriId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Kullanici>()
                //.ToTable("ETicaretKullanicilar")
                .HasOne(k => k.Rol)
                .WithMany(r => r.Kullanicilar)
                .HasForeignKey(k => k.RolId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<KullaniciDetayi>()
                .HasOne(x => x.Kullanici)
                .WithOne(x => x.KullaniciDetayi)
                .HasForeignKey<KullaniciDetayi>(x => x.KullaniciId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<KullaniciDetayi>()
               .HasOne(kullaniciDetayi => kullaniciDetayi.Ulke)
               .WithMany(ulke => ulke.KullaniciDetaylari)
               .HasForeignKey(kullaniciDetayi => kullaniciDetayi.UlkeId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<KullaniciDetayi>()
                .HasOne(kullaniciDetayi => kullaniciDetayi.Sehir)
                .WithMany(sehir => sehir.KullaniciDetaylari)
                .HasForeignKey(kullaniciDetayi => kullaniciDetayi.SehirId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Sehir>()
                .HasOne(sehir => sehir.Ulke)
                .WithMany(ulke => ulke.Sehirler)
                .HasForeignKey(sehir => sehir.UlkeId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UrunMagaza>()
                .HasKey(urunMagaza => new { urunMagaza.UrunId, urunMagaza.MagazaId });

            modelBuilder.Entity<Urun>()
                .HasMany(u => u.UrunMagazalar)
                .WithOne(um => um.Urun)
                .HasForeignKey(um => um.UrunId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Magaza>()
                .HasMany(u => u.UrunMagazalar)
                .WithOne(um => um.Magaza)
                .HasForeignKey(um => um.MagazaId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
