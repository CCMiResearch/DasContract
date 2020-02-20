using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Bonsai.Utils.Property;
using DasContract.Migrator.Interface;

namespace DasContract.Migrator
{
    public class SimpleMigrator : IMigrator
    {
        readonly List<IMigration> migrations = new List<IMigration>();

        int stepsBack = 0;

        public bool Recording { get; set; } = false;

        /// <summary>
        /// Tells the number if currently stored migrations
        /// </summary>
        /// <returns>Current number of migrations</returns>
        public int MigrationsCount()
        {
            return migrations.Count;
        }

        public bool HasStepBack()
        {
            int remainingSteps = migrations.Count - stepsBack;
            return remainingSteps > 0;
        }

        public bool HasStepForward()
        {
            return stepsBack != 0 && migrations.Count > 0;
        }

        public void StepBackward()
        {
            if (!HasStepBack())
                return;

            var migrationToRollback = migrations[stepsBack++];
            migrationToRollback.RevertLastKnownValue();
        }

        public void StepForward()
        {
            if (!HasStepForward())
                return;

            var migrationToRollback = migrations[--stepsBack];
            migrationToRollback.RevertLastKnownValue();
        }

        public void Notify<TType>(Expression<Func<TType>> propertyExpression, Func<TType> propertyGetter, Action<TType> propertySetter)
        {
            if (!Recording)
                return;

            //Remove all "future" steps
            for (; stepsBack > 0; stepsBack--)
                migrations.RemoveAt(0);

            //Check if the previous migrations edits the same property
            if (migrations.Count > 0 
                && PropertyAttributeGetter.GetMemberInfo(propertyExpression.Body).HasSameMetadataDefinitionAs(
                    PropertyAttributeGetter.GetMemberInfo(migrations.First().PropertyExpression))
                )
            {
                //Do nothing
            }

            //Create new migration
            else
            {
                var newMigration = new Migration<TType>(propertyExpression.Body, propertyGetter, propertySetter);
                migrations.Insert(0, newMigration);
            }
        }

        public void Notify<TType>(Expression<Func<TType>> propertyExpression, Action<TType> propertySetter)
        {
            Notify(propertyExpression, propertyExpression.Compile(), propertySetter);
        }

        public void StartTracingSteps()
        {
            Recording = true;
        }

        public void StopTracingSteps()
        {
            Recording = false;
        }

       
    }
}
