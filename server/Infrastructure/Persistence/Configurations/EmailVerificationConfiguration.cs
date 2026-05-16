using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Domain.Entities;

namespace server.Infrastructure.Persistence.Configurations;

public class EmailVerificationConfiguration : IEntityTypeConfiguration<EmailVerification>
{
    public void Configure(EntityTypeBuilder<EmailVerification> builder)
    {
        builder.ToTable("EmailVerifications");

        builder.HasKey(ev => ev.Id);

        builder.Property(ev => ev.OtpHash).IsRequired().HasMaxLength(512);
        builder.Property(ev => ev.ExpiresAt).IsRequired();
        builder.Property(ev => ev.CreatedAt).IsRequired();

        builder.HasOne(ev => ev.User)
            .WithMany(u => u.EmailVerifications)
            .HasForeignKey(ev => ev.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ev => ev.UserId);
    }
}
