using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UserManagement.Api.Shared.Entities;
using UserManagement.Api.Shared.Extensions;

namespace UserManagement.Api.Database
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(u => u.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(u => u.IsBlocked)
                .HasColumnName("is_blocked")
                .HasConversion(x => x.ToString().ToLower(),
                               value => value.ToBoolean());

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Изменение имен таблиц
                var tableName = entity.GetTableName();
                entity.SetTableName(tableName.ToSnakeCase());

                // Изменение имен колонок
                foreach (var property in entity.GetProperties())
                {
                    var columnName = property.GetColumnName(
                        StoreObjectIdentifier.Table(
                            entity.GetTableName(),
                            entity.GetSchema()));
                    property.SetColumnName(columnName.ToSnakeCase());
                }

                // Изменение имен ключей
                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                // Изменение имен индексов
                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(
                        index.GetDatabaseName().ToSnakeCase());
                }

                // Изменение имен внешних ключей
                foreach (var foreignKey in entity.GetForeignKeys())
                {
                    foreignKey.SetConstraintName(
                        foreignKey.GetConstraintName().ToSnakeCase());
                }
            }
        }
    }
}
