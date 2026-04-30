using eLab.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eLab.DAL.Configurations
{
    public class ResultConfiguration : IEntityTypeConfiguration<Result>
    {
        public void Configure(EntityTypeBuilder<Result> builder)
        {
            builder.Property(r => r.ResultFlags)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(r => r.Status)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}