﻿namespace KnightFrank.Antares.Domain.Common.Enums
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;

    public class EnumMapper
    {
        public static RequirementType GetRequirementType(OfferType offerType)
        {
            IEnumerable<RequirementType> requirementTypes = EnumExtensions.GetValues<RequirementType>();
            return requirementTypes.Single(x => x.ToString() == offerType.ToString());
        }

        public static OfferType GetOfferType(RequirementType requirementType)
        {
            IEnumerable<OfferType> offerTypes = EnumExtensions.GetValues<OfferType>();
            return offerTypes.Single(x => x.ToString() == requirementType.ToString());
        }

        public static TOut GetEnum<TIn, TOut>(TIn inputType) 
            where TIn: struct
            where TOut: struct
        {
            IEnumerable<TOut> outputTypes = EnumExtensions.GetValues<TOut>();
            return outputTypes.Single(x => x.ToString() == inputType.ToString());
        }
    }
}
