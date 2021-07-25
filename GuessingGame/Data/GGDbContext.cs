using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace GuessingGame.Data
{
    public class GGDbContext : DbContext
    {
        public GGDbContext(DbContextOptions<GGDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //dynamically load all entity and query type configurations
            System.Collections.Generic.IEnumerable<Type> typeConfigurations = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type =>
                    (type.BaseType?.IsGenericType ?? false)
                        && (type.BaseType.GetGenericTypeDefinition() == typeof(GGEntityTypeConfiguration<>)));

            foreach (Type typeConfiguration in typeConfigurations)
            {
                IMappingConfiguration configuration = (IMappingConfiguration)Activator.CreateInstance(typeConfiguration);
                configuration.ApplyConfiguration(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
