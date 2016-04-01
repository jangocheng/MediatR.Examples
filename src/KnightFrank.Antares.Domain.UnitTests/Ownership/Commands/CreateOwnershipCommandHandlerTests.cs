﻿namespace KnightFrank.Antares.Domain.UnitTests.Ownership.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;
    using KnightFrank.Antares.Domain.Ownership.CommandHandlers;
    using KnightFrank.Antares.Domain.Ownership.Commands;
    using KnightFrank.Antares.Domain.UnitTests.FixtureExtension;

    using Moq;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class CreateOwnershipCommandHandlerTests : IClassFixture<BaseTestClassFixture>
    {
        [Theory]
        [AutoMoqData]
        public void Given_CorrectCommand_When_Handle_Then_ShouldReturnValidId(
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IDomainValidator<CreateOwnershipCommand>> ownershipDomainValidator,
            CreateOwnershipCommand command,
            CreateOwnershipCommandHandler commandHandler)
        {
            contactRepository.Setup(x => x.FindBy(It.IsAny<Expression<Func<Contact, bool>>>())).Returns(new List<Contact>().AsQueryable());
            ownershipDomainValidator.Setup(x=>x.Validate(It.IsAny<CreateOwnershipCommand>())).Returns(new ValidationResult());

            // Act
            commandHandler.Handle(command);

            // Assert
            ownershipRepository.VerifyAll();
            contactRepository.VerifyAll();
        }

        [Theory]
        [AutoMoqData]
        public void Given_IncorrectCommand_When_Handle_Then_ShouldReturnDomainException(
            [Frozen] Mock<IGenericRepository<Ownership>> ownershipRepository,
            [Frozen] Mock<IGenericRepository<Contact>> contactRepository,
            [Frozen] Mock<IDomainValidator<CreateOwnershipCommand>> ownershipDomainValidator,
            CreateOwnershipCommand command,
            CreateOwnershipCommandHandler commandHandler,
            Fixture fixture)
        {
            ownershipDomainValidator.Setup(x => x.Validate(It.IsAny<CreateOwnershipCommand>())).Returns(fixture.BuildValidationResult());
            
            Assert.Throws<DomainValidationException>(() => commandHandler.Handle(command));
            ownershipRepository.Verify(p => p.Save(), Times.Never);
        }
    }
}
