using Microsoft.EntityFrameworkCore;

namespace GuessingGame.Data
{
    public interface IMappingConfiguration
    {
        void ApplyConfiguration(ModelBuilder modelBuilder);
    }
}
