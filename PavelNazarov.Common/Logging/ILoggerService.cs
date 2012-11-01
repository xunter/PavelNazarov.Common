using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PavelNazarov.Common.Logging
{
    public interface ILoggerService
    {
        bool IsTraceEnabled { get; }
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarningEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }

        void Trace(string message);
        void Trace(string format, params object[] args);

        void Debug(string message);
        void Debug(string format, params object[] args);

        void Info(string message);
        void Info(string format, params object[] args);

        void Warning(string message);
        void Warning(string format, params object[] args);

        void Error(string message);
        void Error(string message, Exception ex);
        void Error(string format, params object[] args);

        void Fatal(string message);
        void Fatal(string message, Exception ex);
        void Fatal(string format, params object[] args);
    }
}
