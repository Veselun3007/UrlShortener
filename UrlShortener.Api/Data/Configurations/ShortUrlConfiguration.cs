using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Data.Configurations;

public class ShortUrlConfiguration : IEntityTypeConfiguration<ShortUrl>
{
    public void Configure(EntityTypeBuilder<ShortUrl> builder)
    {
        builder.HasKey(shortUrl => shortUrl.Id);

        builder.Property(shortUrl => shortUrl.OriginalUrl)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(shortUrl => shortUrl.ShortCode)
            .IsRequired()
            .HasMaxLength(16);

        builder.Property(shortUrl => shortUrl.CreatedDate)
            .IsRequired();

        builder.Property(shortUrl => shortUrl.CreatedById)
            .IsRequired();

        builder.HasIndex(shortUrl => shortUrl.OriginalUrl)
            .IsUnique();

        builder.HasIndex(shortUrl => shortUrl.ShortCode)
            .IsUnique();
        
        builder.HasOne(x => x.CreatedBy)
            .WithMany(x => x.ShortUrls)
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}