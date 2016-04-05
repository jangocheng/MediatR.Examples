﻿namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;

    public class CreateActivityValidator : AbstractValidator<CreateActivityCommand>
    {   
        public CreateActivityValidator(IGenericRepository<Property> propertyRepository, IReadGenericRepository<EnumTypeItem> enumTypeItemRepository, IGenericRepository<Contact> contactRepository)
        {
            Func<CreateActivityCommand, ValidationFailure> propertyExists = cmd =>
            {   
                Property property = propertyRepository.GetById(cmd.PropertyId);

                return property == null ? new ValidationFailure(nameof(cmd.PropertyId), "Property does not exist.") : null;
            };

            Func<CreateActivityCommand, ValidationFailure> activityStatusExists = cmd =>
            {
                bool isActivityStatus =
                    enumTypeItemRepository.Get()
                                          .Any(et => et.Id == cmd.ActivityStatusId && et.EnumType.Code == "ActivityStatus");

                return isActivityStatus ? null : new ValidationFailure(nameof(cmd.ActivityStatusId), "Activity Status does not exist.");
            };

            Func<CreateActivityCommand, ValidationFailure> isValidvendorsExist = cmd =>
            {
                var isValid = true;
                
                if (cmd.Vendors != null && cmd.Vendors.Any())
                {
                    IEnumerable<Guid> result = contactRepository.FindBy(c => cmd.Vendors.Contains(c.Id)).Select(x => x.Id);
                    isValid = !cmd.Vendors.Except(result).Any();
                }

                return isValid ? null : new ValidationFailure("Vendors", "Vendors are invalid.");
            };

            this.RuleFor(x => x.PropertyId).NotEmpty();
            this.RuleFor(x => x.ActivityStatusId).NotEmpty();

            this.Custom(propertyExists);
            this.Custom(activityStatusExists);
            this.Custom(isValidvendorsExist);
        }
    }
}