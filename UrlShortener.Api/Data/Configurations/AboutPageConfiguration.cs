using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Api.Models;

namespace UrlShortener.Api.Data.Configurations;

public class AboutPageConfiguration : IEntityTypeConfiguration<AboutPage>
{
    public void Configure(EntityTypeBuilder<AboutPage> builder)
    {
        builder.HasKey(page => page.Id);

        builder.Property(page => page.Content)
            .IsRequired();

        builder.Property(page => page.UpdatedDate)
            .IsRequired();
    }
}