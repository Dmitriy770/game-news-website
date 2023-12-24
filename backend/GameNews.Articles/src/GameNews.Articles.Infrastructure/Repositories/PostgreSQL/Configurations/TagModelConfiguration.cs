using GameNews.Articles.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameNews.Articles.Infrastructure.Repositories.PostgreSQL.Configurations;

public class TagModelConfiguration : IEntityTypeConfiguration<TagModel>
{
    public void Configure(EntityTypeBuilder<TagModel> builder)
    {
        builder.HasKey(t => t.Id);

    }
}