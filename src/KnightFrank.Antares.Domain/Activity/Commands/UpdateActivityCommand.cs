﻿namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    using MediatR;

    public class UpdateActivityCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public Guid ActivityStatusId { get; set; }

        public decimal? MarketAppraisalPrice { get; set; }

        public decimal? RecommendedPrice { get; set; }

        public decimal? VendorEstimatedPrice { get; set; }

        public Guid ActivityTypeId { get; set; }
    }
}