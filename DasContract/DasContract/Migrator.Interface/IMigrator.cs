using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DasContract.Migrator.Interface
{
    public interface IMigrator
    {
        /// <summary>
        /// Notifies the migrator about property a change
        /// </summary>
        /// <typeparam name="TType">The changed property data type</typeparam>
        /// <param name="propertyExpression">The changed property expression</param>
        /// <param name="propertyGetter">Changed property getter</param>
        /// <param name="propertySetter">Changed property setter</param>
        void Notify<TType>(Expression<Func<TType>> propertyExpression, Func<TType> propertyGetter, Action<TType> propertySetter);

        /// <summary>
        /// Notifies the migrator about property a change
        /// </summary>
        /// <typeparam name="TType">The changed property data type</typeparam>
        /// <param name="propertyExpression">The changed property expression and getter</param>
        /// <param name="propertySetter">Changed property setter</param>
        void Notify<TType>(Expression<Func<TType>> propertyExpression, Action<TType> propertySetter);

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
        bool HasStepBack();

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
