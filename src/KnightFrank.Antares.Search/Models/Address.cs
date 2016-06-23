﻿namespace KnightFrank.Antares.Search.Models
{
    public class Address
    {
        public string Id { get; set; }

        public string CountryId { get; set; }

        public Country Country { get; set; }

        public string AddressFormId { get; set; }

        public string PropertyNumber { get; set; }

        public string PropertyName { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Line3 { get; set; }

        public string Postcode { get; set; }

        public string City { get; set; }

        public string County { get; set; }
    }
}