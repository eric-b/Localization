using System;

namespace Localization.Core
{
    /// <summary>
    /// Empty implementation of <see cref="ILogger"/>.
    /// </summary>
    public sealed class EmptyLogger : ILogger
    {
        public void Debug(string message, params string[] args)
        {
            
        }

        public void Info(string message, params string[] args)
        {
            
        }

        public void Warn(string message, params string[] args)
        {
            
        }

        public void Error(string message, params string[] args)
        {
            
        }

        public void Error(string message, Exception exception)
        {
            
        }

        public void Error(Exception exception)
        {
            
        }
    }
}
