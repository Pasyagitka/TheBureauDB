using System.Data.Entity;

namespace TheBureau.Models
{
    public partial class Model : DbContext
    {
        public Model()
            : base("name=TheBureauConnection")
        {
        }

        public virtual DbSet<Accessory> Accessories { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Brigade> Brigades { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RequestEquipment> RequestEquipments { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Tool> Tools { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accessory>()
                .Property(e => e.equipmentId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Accessory>()
                .Property(e => e.price)
                .HasPrecision(6, 2);

            modelBuilder.Entity<Address>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.Address)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.contactNumber)
                .HasPrecision(12, 0);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.Client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.contactNumber)
                .HasPrecision(12, 0);

            modelBuilder.Entity<Equipment>()
                .Property(e => e.id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.Accessories)
                .WithRequired(e => e.Equipment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Equipment>()
                .HasMany(e => e.RequestEquipments)
                .WithRequired(e => e.Equipment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Request>()
                .HasMany(e => e.RequestEquipments)
                .WithRequired(e => e.Request)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RequestEquipment>()
                .Property(e => e.equipmentId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Role1)
                .HasForeignKey(e => e.role)
                .WillCascadeOnDelete(false);

        }
    }
}
