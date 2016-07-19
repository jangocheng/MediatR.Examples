namespace KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations
{
    using System.Collections.Generic;
    using System.Linq;
    using KnightFrank.Antares.Dal.Model.Portal;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;

    public class ActivityPortalsMapper : IActivityReferenceMapper<Portal>
    {
        private readonly IGenericRepository<Portal> portalRepository;

        public ActivityPortalsMapper(IGenericRepository<Portal> portalRepository)
        {
            this.portalRepository = portalRepository;
        }

        public void ValidateAndAssign(ActivityCommandBase message, Activity activity)
        {
            ICollection<UpdateActivityPortal> advertisingPortals = message.AdvertisingPortals ?? new List<UpdateActivityPortal>();
            activity.AdvertisingPortals
                    .Where(x => advertisingPortals.Select(y => y.Id).Contains(x.Id) == false)
                    .ToList()
                    .ForEach(x => activity.AdvertisingPortals.Remove(x));

            advertisingPortals
                   .Where(x => activity.AdvertisingPortals.Select(y => y.Id).Contains(x.Id) == false)
                   .ToList()
                   .ForEach(x =>
                   {
                       Portal portal = this.portalRepository.GetById(x.Id);
                       activity.AdvertisingPortals.Add(portal);
                   });
        }
    }
}