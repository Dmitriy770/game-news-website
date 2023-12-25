using GameNews.Articles.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameNews.Articles.Infrastructure.Repositories.PostgreSQL.Configurations;

public sealed class ArticleModelConfiguration : IEntityTypeConfiguration<ArticleModel>
{
    public void Configure(EntityTypeBuilder<ArticleModel> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasMany(a => a.Tags)
            .WithMany(t => t.Articles);
    }
}