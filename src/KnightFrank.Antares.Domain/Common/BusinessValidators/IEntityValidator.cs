﻿namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;

    using KnightFrank.Antares.Dal.Model;

    public interface IEntityValidator
    {
        void EntityExists<T>(Guid entity) where T : BaseEntity;

        void EntityExists<T>(T entity, Guid entityId) where T : BaseEntity;
    }
}