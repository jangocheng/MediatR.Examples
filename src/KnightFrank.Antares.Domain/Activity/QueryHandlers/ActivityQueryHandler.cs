﻿namespace KnightFrank.Antares.Domain.Activity.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Queries;

    using MediatR;

    public class ActivityQueryHandler : IRequestHandler<ActivityQuery, Activity>
    {
        private readonly IReadGenericRepository<Activity> activityRepository;

        public ActivityQueryHandler(IReadGenericRepository<Activity> activityRepository)
        {
            this.activityRepository = activityRepository;
        }

        public Activity Handle(ActivityQuery query)
        {
            Activity result =
                this.activityRepository.Get()
                    .Include(a => a.Property)
                    .Include(a => a.Property.Address)
                    .Include(a => a.Contacts)
                    .Include(a => a.Attachments)
                    .Include(a => a.Attachments.Select(at => at.User))
                    .Include(a => a.Viewings)
                    .Include(a => a.Viewings.Select(v => v.Attendees))
                    .Include(a => a.Viewings.Select(v => v.Negotiator))
                    .Include(a => a.Viewings.Select(v => v.Requirement))
                    .Include(a => a.Viewings.Select(v => v.Requirement.Contacts))
                    .Include(a => a.ActivityNegotiators)
                    .Include(a => a.ActivityNegotiators.Select(an => an.Negotiator))
                    .SingleOrDefault(a => a.Id == query.Id);

            return result;
        }
    }
}
