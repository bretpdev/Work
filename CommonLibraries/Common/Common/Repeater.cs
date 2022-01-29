using System;
using System.Collections.Generic;
using System.Threading;

namespace Uheaa.Common
{
    public class Repeater
    {
        /// <summary>
        /// Execute the given action.  If an exception of type E occurs, delay and try the action again.
        /// </summary>
        /// <typeparam name="E">The type of Exception you want to catch.</typeparam>
        /// <param name="a">The action you want executed.</param>
        /// <param name="maxNumberOfTries">The total number of times to try executing the action.</param>
        /// <param name="millisecondDelay">The amount of milliseconds to wait between each retry</param>
        /// <param name="increaseMillisecondDelay">If true, increases the delay by the original amount each time a retry fails.</param>
        /// <returns>A results object describing the success or failure of the procedure, as well as any errors caught.</returns>
        public static RepeatResults<Exception> TryRepeatedly(Action a, int maxNumberOfTries = 5, int millisecondDelay = 3000, bool increaseMillisecondDelay = false)
        {
            return TryRepeatedly<Exception>(a, maxNumberOfTries, millisecondDelay, increaseMillisecondDelay);
        }

        /// <summary>
        /// Execute the given action.  If an exception of type E occurs, delay and try the action again.
        /// </summary>
        /// <typeparam name="E">The type of Exception you want to catch.</typeparam>
        /// <param name="a">The action you want executed.</param>
        /// <param name="maxNumberOfTries">The total number of times to try executing the action.</param>
        /// <param name="millisecondDelay">The amount of milliseconds to wait between each retry</param>
        /// <param name="increaseMillisecondDelay">If true, increases the delay by the original amount each time a retry fails.</param>
        /// <returns>A results object describing the success or failure of the procedure, as well as any errors caught.</returns>
        public static RepeatResults<E> TryRepeatedly<E>(Action a, int maxNumberOfTries = 5, int millisecondDelay = 3000, bool increaseMillisecondDelay = false) where E : Exception
        {
            var results = new RepeatResults<E>();
            for (int timesTried = 0; timesTried < maxNumberOfTries; timesTried++)
            {
                try
                {
                    a();
                    results.Successful = true;
                    break;
                }
                catch (Exception ex)
                {
                    if (typeof(E) == typeof(Exception) || typeof(E) == ex.GetType())
                    {
                        Console.WriteLine("Number of Times Tried: {0}; Exception Message: {1}; Stack Trace: {2}", (timesTried + 1), ex.Message, ex.StackTrace);
                        if (increaseMillisecondDelay)
                            Thread.Sleep(millisecondDelay * (timesTried + 1));
                        else
                            Thread.Sleep(millisecondDelay);
                        results.CaughtExceptions.Add((E)ex);
                        continue;
                    }
                    else
                        throw;
                }
            }
            return results;
        }
    }

    public class RepeatResults<E> where E : Exception
    {
        public bool Successful { get; set; }
        public List<E> CaughtExceptions { get; set; }
        public RepeatResults()
        {
            CaughtExceptions = new List<E>();
        }
    }
}
