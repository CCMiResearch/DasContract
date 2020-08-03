using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DasContract.Editor.Migrator.Interfaces
{
    public interface IMigrator
    {
        event MigratorHandler OnMigrationsChange;

        /// <summary>
        /// Notifies the migrator about property change
        /// </summary>
        /// <typeparam name="TType">The changed property data type</typeparam>
        /// <param name="propertyExpression">The changed property expression</param>
        /// <param name="propertyGetter">Changed property getter</param>
        /// <param name="propertySetter">Changed property setter</param>
        /// <param name="mode">How should the migrator behave</param>
        void Notify<TType>(Expression<Func<TType>> propertyExpression, Func<TType> propertyGetter, Action<TType> propertySetter, MigratorMode mode);

        /// <summary>
        /// Notifies the migrator about property change
        /// </summary>
        /// <typeparam name="TType">The changed property data type</typeparam>
        /// <param name="propertyExpression">The changed property expression and getter</param>
        /// <param name="propertySetter">Changed property setter</param>
        /// <param name="propertySetter">How should the migrator behave</param>
        void Notify<TType>(Expression<Func<TType>> propertyExpression, Action<TType> propertySetter, MigratorMode mode);

        /// <summary>
        /// Notifies the migrator about property change
        /// </summary>
        /// <typeparam name="TType">The changed property data type</typeparam>
        /// <param name="propertyExpression">The changed property expression</param>
        /// <param name="up">What should the migrator do on step forward / version up</param>
        /// <param name="down">What should the migrator do on step back / version down</param>
        /// <param name="mode">How should the migrator behave</param>
        void Notify<TType>(Expression<Func<TType>> propertyExpression, Action up, Action down, MigratorMode mode);

        /// <summary>
        /// Moves the model version one step back, of there is one.
        /// </summary>
        void StepBackward();

        /// <summary>
        /// Moves the model version one step forward, if there is one.
        /// </summary>
        void StepForward();

        /// <summary>
        /// Indicates if there is a possible step back
        /// </summary>
        /// <returns>True if there is a possible step back, else false</returns>
        bool HasStepBackward();

        /// <summary>
        /// Indicates if there is a step forward
        /// </summary>
        /// <returns>True if there is a possible step back, else false</returns>
        bool HasStepForward();

        /// <summary>
        /// Starts tracing steps
        /// </summary>
        /// <returns>This</returns>
        void StartTracingSteps();

        /// <summary>
        /// Stops tracing steps
        /// </summary>
        /// <returns>This</returns>
        void StopTracingSteps();
    }
}
