﻿namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentValidation;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public static class GreaterThanOrEqualToExtensions
    {
        public static Field<TEntity, TProperty> GreaterThanOrEqualTo<TEntity, TProperty>(this Field<TEntity, TProperty> field, TProperty limit)
            where TProperty : IComparable, IComparable<TProperty>
        {
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).GreaterThanOrEqualTo(limit)));
            return field;
        }

        public static Field<TEntity, TProperty> GreaterThanOrEqualTo<TEntity, TProperty>(this Field<TEntity, TProperty> field, Expression<Func<TEntity, TProperty>> limit)
            where TProperty : IComparable, IComparable<TProperty>
        {
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).GreaterThanOrEqualTo(limit)));
            return field;
        }

        public static Field<TEntity, TProperty?> GreaterThanOrEqualTo<TEntity, TProperty>(this Field<TEntity, TProperty?> field, TProperty limit)
           where TProperty : struct, IComparable, IComparable<TProperty>
        {
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).GreaterThanOrEqualTo(limit)));
            return field;
        }

        public static Field<TEntity, TProperty?> GreaterThanOrEqualTo<TEntity, TProperty>(this Field<TEntity, TProperty?> field, Expression<Func<TEntity, TProperty?>> limit)
           where TProperty : struct, IComparable, IComparable<TProperty>
        {
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).GreaterThanOrEqualTo(limit).When(x => limit.Compile().Invoke(x).HasValue)));
            return field;
        }

        public static IList<Field<TEntity, TProperty>> GreaterThanOrEqualTo<TEntity, TProperty>(this IList<Field<TEntity, TProperty>> fields, TProperty limit)
            where TProperty : IComparable, IComparable<TProperty>
        {
            foreach (Field<TEntity, TProperty> field in fields)
            {
                field.GreaterThanOrEqualTo(limit);
            }

            return fields;
        }

        public static IList<Field<TEntity, TProperty>> GreaterThanOrEqualTo<TEntity, TProperty>(this IList<Field<TEntity, TProperty>> fields, Expression<Func<TEntity, TProperty>> limit)
            where TProperty : IComparable, IComparable<TProperty>
        {
            foreach (Field<TEntity, TProperty> field in fields)
            {
                field.GreaterThanOrEqualTo(limit);
            }

            return fields;
        }

        public static IList<Field<TEntity, TProperty?>> GreaterThanOrEqualTo<TEntity, TProperty>(this IList<Field<TEntity, TProperty?>> fields, TProperty limit)
           where TProperty : struct, IComparable, IComparable<TProperty>
        {
            foreach (Field<TEntity, TProperty?> field in fields)
            {
                field.GreaterThanOrEqualTo(limit);
            }

            return fields;
        }

        public static IList<Field<TEntity, TProperty?>> GreaterThanOrEqualTo<TEntity, TProperty>(this IList<Field<TEntity, TProperty?>> fields, Expression<Func<TEntity, TProperty?>> limit)
           where TProperty : struct, IComparable, IComparable<TProperty>
        {
            foreach (Field<TEntity, TProperty?> field in fields)
            {
                field.GreaterThanOrEqualTo(limit);
            }

            return fields;
        }
    }
}
