using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Api.Shared.Entities;
using UserManagement.Api.Shared.Extensions;

namespace UserManagement.Api.Database.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.Property(u => u.FirstName)
            //    .HasMaxLength(50)
            //    .HasColumnName("first_name");

            //builder.Property(u => u.LastName)
            //    .HasMaxLength(50)
            //    .HasColumnName("last_name");

            //builder.Property(u => u.IsBlocked)
            //    .HasColumnName("is_blocked")
            //    .HasConversion(x => x.ToString().ToLower(),
            //                   value => value.ToBoolean());

            //builder.HasIndex(u => u.Email);
        }
    }
}
