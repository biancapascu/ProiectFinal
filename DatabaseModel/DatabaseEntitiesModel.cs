using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DatabaseModel
{
    public partial class DatabaseEntitiesModel : DbContext
    {
        public DatabaseEntitiesModel()
            : base("name=DatabaseEntitiesModel")
        {
        }

        public virtual DbSet<Cazare> Cazare { get; set; }
        public virtual DbSet<Clienti> Clienti { get; set; }
        public virtual DbSet<Contracte> Contracte { get; set; }
        public virtual DbSet<Destinatii> Destinatii { get; set; }
        public virtual DbSet<Transport> Transport { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cazare>()
                .HasMany(e => e.Contracte)
                .WithOptional(e => e.Cazare)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Clienti>()
                .HasMany(e => e.Contracte)
                .WithOptional(e => e.Clienti)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Destinatii>()
                .HasMany(e => e.Cazare)
                .WithOptional(e => e.Destinatii)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Transport>()
                .HasMany(e => e.Contracte)
                .WithOptional(e => e.Transport)
                .WillCascadeOnDelete();
        }
    }
}
