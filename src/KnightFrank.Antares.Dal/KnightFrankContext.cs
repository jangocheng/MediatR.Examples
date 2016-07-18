﻿namespace KnightFrank.Antares.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Configuration;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.LatestView;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Portal;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Model.User;

    using Attribute = KnightFrank.Antares.Dal.Model.Attribute.Attribute;

    [ExcludeFromCodeCoverage]
    public class KnightFrankContext : DbContext
    {
        public KnightFrankContext() : base("Api.Settings.SqlConnectionString")
        {
        }

        public KnightFrankContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Properties<DateTime>()
                        .Configure(c => c.HasColumnType("datetime2"));
            this.LoadConfigurations(modelBuilder);
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactTitle> ContactTitles { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<RequirementType> RequirementTypes { get; set; }
        public DbSet<RequirementTypeLocalised> RequirementTypeLocaliseds{ get; set; }
        public DbSet<EnumType> EnumTypes { get; set; }
        public DbSet<EnumTypeItem> EnumTypeItems { get; set; }
        public DbSet<EnumLocalised> EnumLocaliseds { get; set; }
        public DbSet<Locale> Locales { get; set; }
        public DbSet<AddressFieldDefinition> AddressFieldDefinitions { get; set; }
        public DbSet<AddressFieldLabel> AddressFieldLabels { get; set; }
        public DbSet<AddressField> AddressFields { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressFormEntityType> AddressFormEntityTypes { get; set; }
        public DbSet<AddressForm> AddressForms { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryLocalised> CountryLocaliseds { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Ownership> Ownerships { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityUser> ActivityUsers { get; set; }
        public DbSet<ActivityTypeDefinition> ActivityTypeDefinitions { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<ActivityTypeLocalised> ActivityTypeLocaliseds { get; set; }
        public DbSet<ActivityDepartment> ActivityDepartment { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<PropertyTypeLocalised> PropertyTypeLocaliseds { get; set; }
        public DbSet<PropertyTypeDefinition> PropertyTypeDefinitions { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Attribute> Attributes { get; set; }
        public DbSet<PropertyAttributeForm> PropertyAttributeForms { get; set; }
        public DbSet<PropertyAttributeFormDefinition> PropertyAttributeFormDefinitions { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<CharacteristicGroup> CharacteristicGroups { get; set; }
        public DbSet<CharacteristicGroupItem> CharacteristicGroupItems { get; set; }
        public DbSet<CharacteristicGroupLocalised> CharacteristicGroupLocaliseds { get; set; }
        public DbSet<CharacteristicGroupUsage> CharacteristicGroupUsages { get; set; }
        public DbSet<CharacteristicLocalised> CharacteristicLocaliseds { get; set; }
        public DbSet<Viewing> Viewing { get; set; }
        public DbSet<Offer> Offer { get; set; }
        public DbSet<OfferType> OfferTypes { get; set; }
        public DbSet<OfferTypeLocalised> OfferTypeLocaliseds { get; set; }
        public DbSet<PropertyAreaBreakdown> PropertyAreaBreakdown { get; set; }
        public DbSet<LatestView> LatestView { get; set; }
        public DbSet<CompanyContact> CompanyContacts { get; set; }
        public DbSet<Portal> Portals { get; set; }
        public DbSet<PortalDefinition> PortalDefinitions { get; set; }
        public DbSet<ChainTransaction> ChainTransactions { get; set; }

        private void LoadConfigurations(DbModelBuilder modelBuilder)
        {
            IEnumerable<Type> mapTypes = from t in typeof(KnightFrankContext).Assembly.GetTypes()
                                         where t.BaseType != null
                                         && t.BaseType.IsGenericType
                                         && t.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityConfiguration<>)
                                         select t;

            foreach (Type mapType in mapTypes)
            {
                dynamic mapInstance = Activator.CreateInstance(mapType);
                modelBuilder.Configurations.Add(mapInstance);
            }
        }

    }
}