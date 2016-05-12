﻿using System;

namespace KnightFrank.Antares.Domain.Offer.Commands
{
    using MediatR;

    public class CreateOfferCommand : IRequest<Guid>
    {
        public Guid ActivityId { get; set; }

        public Guid RequirementId { get; set; }

        public Guid StatusId { get; set; }

        public decimal Price { get; set; }

        public DateTime OfferDate { get; set; }

        public DateTime? ExchangeDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public string SpecialConditions { get; set; }
    }
}
