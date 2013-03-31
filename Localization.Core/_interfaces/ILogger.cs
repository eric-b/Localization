using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Localization.Core
{
    public interface ILogger
    {
        void Debug(string message, params string[] args);
        void Info(string message, params string[] args);
        void Warn(string message, params string[] args);
        void Error(string message, params string[] args);

        void Error(string message, Exception exception);
        void Error(Exception exception);
    }
}
