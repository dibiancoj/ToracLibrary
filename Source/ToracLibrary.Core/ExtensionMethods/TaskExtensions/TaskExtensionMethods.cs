using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ExtensionMethods.TaskExtensions
{

    /// <summary>
    /// Task based extension methods
    /// </summary>
    public static class TaskExtensionMethods
    {

        /// <summary>
        /// Use a continuation based flow to run code after a task returns successfully
        /// </summary>
        /// <typeparam name="TTaskResult">Result type of the task</typeparam>
        /// <typeparam name="TMethodResult">Result of the method after the continuation</typeparam>
        /// <param name="Antecedent">Task to await</param>
        /// <param name="Continuation">continuation code to run and return the result of</param>
        /// <returns></returns>
        public static async Task<TMethodResult> Then<TTaskResult, TMethodResult>(this Task<TTaskResult> Antecedent, Func<TTaskResult, TMethodResult> Continuation)
        {
            //run the continuation and return the result
            return Continuation(await Antecedent);
        }

        /// <summary>
        /// Await the continuation func (inner continue). Note: this can be chained since this is an extension method
        /// </summary>
        /// <typeparam name="TTaskResult">Result type of the task</typeparam>
        /// <typeparam name="TMethodResult">Result of the method after the continuation</typeparam>
        /// <param name="antecedent">Task to await</param>
        /// <param name="Continuation">continuation code to run and return the result of</param>
        /// <returns>The end result task</returns>
        public static async Task<TMethodResult> Then<TTaskResult, TMethodResult>(this Task<TTaskResult> Antecedent, Func<TTaskResult, Task<TMethodResult>> Continuation)
        {
            //run the continuation and return the result
            return await Continuation(await Antecedent);
        }

        /// <summary>
        /// Use a continuation based flow to run code after a task returns successfully. Extension when you use ConfigureAwait(false)
        /// </summary>
        /// <typeparam name="TTaskResult">Result type of the task</typeparam>
        /// <typeparam name="TMethodResult">Result of the method after the continuation</typeparam>
        /// <param name="Antecedent">Task to await</param>
        /// <param name="Continuation">continuation code to run and return the result of</param>
        /// <returns>The end result task</returns>
        public static async Task<TMethodResult> Then<TTaskResult, TMethodResult>(this ConfiguredTaskAwaitable<TTaskResult> Antecedent, Func<TTaskResult, TMethodResult> Continuation)
        {
            //run the continuation and return the result
            return Continuation(await Antecedent);
        }

        /// <summary>
        /// Use a continuation based flow to run code after a task returns successfully. Extension when you use ConfigureAwait(false)
        /// </summary>
        /// <typeparam name="TTaskResult">Result type of the task</typeparam>
        /// <typeparam name="TMethodResult">Result of the method after the continuation</typeparam>
        /// <param name="Antecedent">Task to await</param>
        /// <param name="Continuation">continuation code to run and return the result of</param>
        /// <returns>The end result task</returns>
        public static async Task<TMethodResult> Then<TTaskResult, TMethodResult>(this ConfiguredTaskAwaitable<TTaskResult> Antecedent, Func<TTaskResult, ConfiguredTaskAwaitable<TMethodResult>> Continuation)
        {
            //run the continuation and return the result
            return await Continuation(await Antecedent);
        }

    }

}
