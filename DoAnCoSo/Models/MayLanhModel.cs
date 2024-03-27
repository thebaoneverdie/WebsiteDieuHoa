using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DoAnCoSo.Models
{
    public partial class MayLanhModel : DbContext
    {
        public MayLanhModel()
            : base("name=MayLanhModel")
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<FeedBack> FeedBack { get; set; }
        public virtual DbSet<Galery> Galery { get; set; }
        public virtual DbSet<Order_Details> Order_Details { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Tokens> Tokens { get; set; }
        public virtual DbSet<User> User { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Product)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.category_id);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Product1)
                .WithOptional(e => e.Category1)
                .HasForeignKey(e => e.category_id);

            modelBuilder.Entity<Orders>()
                .HasMany(e => e.Order_Details)
                .WithOptional(e => e.Orders)
                .HasForeignKey(e => e.order_id);

            modelBuilder.Entity<Orders>()
                .HasMany(e => e.Order_Details1)
                .WithOptional(e => e.Orders1)
                .HasForeignKey(e => e.order_id);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Galery)
                .WithOptional(e => e.Product)
                .HasForeignKey(e => e.product_id);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Galery1)
                .WithOptional(e => e.Product1)
                .HasForeignKey(e => e.product_id);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Order_Details)
                .WithOptional(e => e.Product)
                .HasForeignKey(e => e.product_id);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Order_Details1)
                .WithOptional(e => e.Product1)
                .HasForeignKey(e => e.product_id);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.User)
                .WithOptional(e => e.Role)
                .HasForeignKey(e => e.role_id);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.User1)
                .WithOptional(e => e.Role1)
                .HasForeignKey(e => e.role_id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.user_id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.user_id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tokens)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Tokens1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);
        }
    }
}
