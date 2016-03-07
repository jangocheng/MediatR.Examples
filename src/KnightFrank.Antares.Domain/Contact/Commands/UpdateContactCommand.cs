﻿namespace KnightFrank.Antares.Domain.Contact.Commands
{
    using MediatR;

    public class UpdateContactCommand : IRequest
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Title { get; set; }
    }
}