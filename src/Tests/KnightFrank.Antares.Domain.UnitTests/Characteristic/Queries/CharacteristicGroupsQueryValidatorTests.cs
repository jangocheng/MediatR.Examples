﻿namespace KnightFrank.Antares.Domain.UnitTests.Characteristic.Queries
{
    using System;

    using FluentValidation.Results;

    using KnightFrank.Antares.Domain.Characteristic.Queries;
    using KnightFrank.Antares.Tests.Common.Extensions.AutoFixture.Attributes;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    using Xunit;

    [Trait("FeatureTitle", "Characteristic")]
    [Collection("CharacteristicGroupsQuery")]
    public class CharacteristicGroupsQueryValidatorTests : IClassFixture<BaseTestClassFixture>
    {
        private readonly CharacteristicGroupsQuery query;

        public CharacteristicGroupsQueryValidatorTests()
        {
            IFixture fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            this.query = fixture.Build<CharacteristicGroupsQuery>()
                                                   .With(x => x.PropertyTypeId, Guid.NewGuid())
                                                   .Create();
        }

        [Theory]
        [AutoMoqData]
        public void Given_CorrectCharacteristicGroupsQuery_When_Validating_Then_NoValidationErrors(
            CharacteristicGroupsQueryValidator validator)
        {
            // Act
            ValidationResult validationResult = validator.Validate(this.query);

            // Assert
            Assert.True(validationResult.IsValid);
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectPropertyAttributesQueryWithNoCountryCode_When_Validating_Then_ValidationErrors(
           CharacteristicGroupsQueryValidator validator)
        {
            this.query.CountryId = Guid.Empty;

            TestIncorrectCommand(validator, this.query, nameof(this.query.CountryId));
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectPropertyAttributesQueryWithNoPropertyTypeId_When_Validating_Then_ValidationErrors(
           CharacteristicGroupsQueryValidator validator)
        {
            this.query.PropertyTypeId = Guid.Empty;

            TestIncorrectCommand(validator, this.query, nameof(this.query.PropertyTypeId));
        }

        private static void TestIncorrectCommand(CharacteristicGroupsQueryValidator validator, CharacteristicGroupsQuery query,
            string testedPropertyName)
        {
            // Act
            ValidationResult validationResult = validator.Validate(query);

            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Contains(validationResult.Errors, failure => failure.PropertyName == testedPropertyName);
        }
    }
}