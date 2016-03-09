namespace KnightFrank.Antares.Dal.Migrations
{
    using KnightFrank.Antares.Dal.Model;

    internal sealed class CountryConfiguration : BaseEntityConfiguration<Country>
    {
        public CountryConfiguration()
        {
            this.Property(p => p.Code).HasMaxLength(100);

            this.HasMany(p => p.AddressForms)
                .WithRequired(p => p.Country)
                .WillCascadeOnDelete(false);

            this.HasMany(p => p.Addresses)
                .WithRequired(p => p.Country)
                .WillCascadeOnDelete(false);
        }
    }
}