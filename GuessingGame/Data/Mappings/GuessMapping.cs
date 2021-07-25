using GuessingGame.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GuessingGame.Data.Mappings
{
    public class GuessMapping : GGEntityTypeConfiguration<Guess>
    {
        public override void Configure(EntityTypeBuilder<Guess> builder)
        {
            builder.ToTable(nameof(Guess));

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.FirstDigit).IsRequired();

            builder.Property(p => p.SecondDigit).IsRequired();

            builder.Property(p => p.ThirdDigit).IsRequired();

            builder.Property(p => p.FourthDigit).IsRequired();

            builder.Property(p => p.GameId).IsRequired();

            base.Configure(builder);
        }
    }
}
