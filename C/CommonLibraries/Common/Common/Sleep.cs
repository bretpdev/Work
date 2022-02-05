using System;
using System.Diagnostics;
using System.Threading;

namespace Uheaa.Common
{
    /// <summary>
    /// More English-friendly wrapper for thread sleeping.  Usage: Sleep.For(20).Minutes();  Sleep.For(44).Seconds();
    /// </summary>
    public class Sleep
    {
        public static Sleep For(int duration)
        {
            return For((decimal)duration);
        }
        public static Sleep For(decimal duration)
        {
            return new Sleep(duration);
        }

        public static void ForOneSecond()
        {
            Sleep.For(1).Seconds(); //don't expose bad grammar to the public
        }

        public static void ForOneMinute()
        {
            Sleep.For(1).Minutes(); //don't expose bad grammar to the public
        }

        protected decimal Duration { get; set; }
        public Sleep(decimal duration)
        {
            Duration = duration;
        }
        public void Seconds()
        {
            Thread.Sleep((int)(Duration * 1000));
        }
        public void Minutes()
        {
            Thread.Sleep((int)(Duration * 1000 * 60));
        }

        /// <summary>
        /// Sleep until the given condition is met, or the given timeout expires.
        /// </summary>
        /// <param name="condition">When condition() evaluates to true, this method ends.</param>
        /// <param name="millisecondTimeout">The amount of milliseconds to wait until exiting.</param>
        public static void Until(Func<bool> condition, int millisecondTimeout)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (!condition() && watch.ElapsedMilliseconds <= millisecondTimeout)
                Thread.Sleep(10);
        }
    }
}
