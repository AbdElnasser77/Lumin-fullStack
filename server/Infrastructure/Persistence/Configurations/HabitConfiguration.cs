using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Domain.Entities;

namespace server.Infrastructure.Persistence.Configurations;

public class HabitConfiguration : IEntityTypeConfiguration<Habit>
{
    public void Configure(EntityTypeBuilder<Habit> builder)
    {
        builder.ToTable("Habits");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Name).IsRequired().HasMaxLength(256);
        builder.Property(h => h.Frequency)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(32);
        builder.Property(h => h.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(32);
        builder.Property(h => h.Streak).IsRequired().HasDefaultValue(0);

        builder.HasOne(h => h.User)
            .WithMany(u => u.Habits)
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(h => h.UserId);
    }
}
