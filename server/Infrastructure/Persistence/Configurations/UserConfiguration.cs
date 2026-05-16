using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Domain.Entities;

namespace server.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PasswordHash).HasMaxLength(512);
        builder.Property(u => u.CheckTerms).IsRequired().HasDefaultValue(false);
        builder.Property(u => u.IsEmailVerified).IsRequired().HasDefaultValue(false);
        builder.Property(u => u.OtpHash).HasMaxLength(512);
        builder.Property(u => u.RefreshTokenHash).HasMaxLength(512);
        builder.Property(u => u.PendingEmail).HasMaxLength(256);
        builder.Property(u => u.EmailChangeOtpHash).HasMaxLength(512);

        builder.HasIndex(u => u.Email).IsUnique();
    }
}
