﻿namespace KnightFrank.Antares.Domain.Requirement.QueryHandlers
{
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Requirement.Queries;

    using MediatR;

    public class RequirementQueryHandler : IRequestHandler<RequirementQuery, Requirement>
    {
        private readonly IReadGenericRepository<Requirement> requirementRepository;

        public RequirementQueryHandler(IReadGenericRepository<Requirement> requirementRepository)
        {
            this.requirementRepository = requirementRepository;
        }

        public Requirement Handle(RequirementQuery message)
        {
            Requirement requirement =
                this.requirementRepository
                    .Get()
                    .FirstOrDefault(req => req.Id == message.Id);

            return requirement;
        }
    }
}