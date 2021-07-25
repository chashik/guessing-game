using GuessingGame.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuessingGame.Data.Mappings
{
    public class GameMapping : GGEntityTypeConfiguration<Game>
    {
        public override void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable(nameof(Game));

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Secret).IsRequired().HasMaxLength(4);

            builder.Property(p => p.IsOver).IsRequired();

            base.Configure(builder);
        }
    }
}
