using GuessingGame.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuessingGame.Data.Mappings
{
    public class PlayerMapping : GGEntityTypeConfiguration<Player>
    {
        public override void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable(nameof(Player));

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name).HasMaxLength(40);

            builder.Ignore(p => p.Score);
            
            base.Configure(builder);
        }
    }
}
