using Bonsai.Utils.Property;
using DasContract.Editor.Migrator.Interfaces;
using DasContract.Editor.Migrator.Migrations.GetSet;
using DasContract.Editor.Migrator.Migrations.UpDownMigration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DasContract.Editor.Migrator
{
    public class SimpleMigrator : IMigrator
    {
        readonly List<IMigration> migrations = new List<IMigration>();

        int stepsBack = 0;

        public event MigratorHandler OnMigrationsChange;

        public bool Recording { get; set; } = false;

        /// <summary>
        /// Tells the number if currently stored migrations
        /// </summary>
        /// <returns>Current number of migrations</returns>
        public int MigrationsCount()
        {
            return migrations.Count;
        }

        public bool HasStepBackward()
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
            if (!HasStepBackward())
                return;

            var migrationToRollback = migrations[stepsBack++];
            migrationToRollback.Down();
            InvokeMigratorChange();
        }

        public void StepForward()
        {
            if (!HasStepForward())
                return;

            var migrationToRollback = migrations[--stepsBack];
            migrationToRollback.Up();
            InvokeMigratorChange();
        }

        public void Notify<TType>(Expression<Func<TType>> propertyExpression, 
            Func<TType> propertyGetter, 
            Action<TType> propertySetter,
            MigratorMode mode = MigratorMode.Smart)
        {
            if (!Recording)
                return;

            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            //Remove all "future" steps
            RemoveFutureSteps();

            //Check if the previous migrations edits the same property
            if (migrations.Count > 0 && mode == MigratorMode.Smart
                && PropertyAttributeGetter.GetMemberInfo(propertyExpression.Body).HasSameMetadataDefinitionAs(
                    PropertyAttributeGetter.GetMemberInfo(migrations.First().PropertyExpression))
                )
            {
                //Do nothing
            }

            //Create new migration
            else
            {
                var newMigration = new GetSetValueMigration<TType>(propertyExpression.Body, propertyGetter, propertySetter);
                migrations.Insert(0, newMigration);
                InvokeMigratorChange();
            }
        }

        public void Notify<TType>(Expression<Func<TType>> propertyExpression, 
            Action<TType> propertySetter,
            MigratorMode mode = MigratorMode.Smart)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            Notify(propertyExpression, propertyExpression.Compile(), propertySetter, mode);
        }

        public void Notify<TType>(Expression<Func<TType>> propertyExpression, Action up, Action down, MigratorMode mode = MigratorMode.Smart)
        {
            if (!Recording)
                return;

            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            //Remove all "future" steps
            RemoveFutureSteps();

            //Check if the previous migrations edits the same property
            if (migrations.Count > 0 && mode == MigratorMode.Smart
                && PropertyAttributeGetter.GetMemberInfo(propertyExpression.Body).HasSameMetadataDefinitionAs(
                    PropertyAttributeGetter.GetMemberInfo(migrations.First().PropertyExpression))
                )
            {
                //Do nothing
            }

            //Create new migration
            else
            {
                var newMigration = new UpDownMigration<TType>(propertyExpression.Body, up, down);
                migrations.Insert(0, newMigration);
                InvokeMigratorChange();
            }
        }

        protected void RemoveFutureSteps()
        {
            for (; stepsBack > 0; stepsBack--)
                migrations.RemoveAt(0);
        }

        public void StartTracingSteps()
        {
            Recording = true;
        }

        public void StopTracingSteps()
        {
            Recording = false;
        }

        protected void InvokeMigratorChange()
        {
            OnMigrationsChange?.Invoke(this, new SimpleMigratorArgs());
        }
    }
}
