namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;

    internal sealed class ActivityConfiguration : BaseEntityConfiguration<Activity>
    {
        public ActivityConfiguration()
        {
            this.HasRequired(a => a.Property)
                .WithMany(p => p.Activities)
                .HasForeignKey(a => a.PropertyId)
                .WillCascadeOnDelete(false);

            this.HasRequired(a => a.ActivityStatus).WithMany().HasForeignKey(s => s.ActivityStatusId).WillCascadeOnDelete(false);
            
            this.HasMany(a => a.Contacts).WithMany().Map(
                cs =>
                    {
                        cs.MapLeftKey("ActivityId");
                        cs.MapRightKey("ContactId");
                    });


            this.Property(o => o.MarketAppraisalPrice)
                .HasPrecision(19, 4);

            this.Property(o => o.RecommendedPrice)
                .HasPrecision(19, 4);

            this.Property(o => o.VendorEstimatedPrice)
                .HasPrecision(19, 4);
        }
    }
}
