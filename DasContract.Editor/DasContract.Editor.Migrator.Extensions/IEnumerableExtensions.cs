using System;
using System.Collections.Generic;
using DasContract.Editor.Migrator.Interfaces;

namespace DasContract.Editor.Migrator.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TContent> WithMigrator<TContent, TMigrator>(this IEnumerable<TContent> iEnumerable, TMigrator migrator)
           where TMigrator : IMigrator
           where TContent : IMigratableComponent<TContent, IMigrator>
        {
            return WithMigrator<IEnumerable<TContent>, TContent, TMigrator>(iEnumerable, migrator);
        }


        public static TEnumerable WithMigrator<TEnumerable, TContent, TMigrator>(this TEnumerable iEnumerable, TMigrator migrator)
            where TMigrator : IMigrator
            where TEnumerable : IEnumerable<TContent>
            where TContent: IMigratableComponent<TContent, IMigrator>
        {
            foreach (var item in iEnumerable)
                item.WithMigrator(migrator);

            return iEnumerable;
        }

    }
}
