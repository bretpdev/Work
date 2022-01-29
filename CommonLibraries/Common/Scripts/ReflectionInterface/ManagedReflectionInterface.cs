using System;

namespace Uheaa.Common.Scripts
{
    /// <summary>
    /// An IDisposable wrapper around ReflectionInterface.  Closes the session when disposed.
    /// </summary>
    public class ManagedReflectionInterface : ReflectionInterface, IDisposable
    {
        public void Dispose()
        {
            ReflectionSession.Exit();
        }
    }
}
