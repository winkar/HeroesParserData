﻿using GalaSoft.MvvmLight;
using NLog;

namespace HeroesStatTracker.Core.ViewModels
{
    public class HstViewModel : ViewModelBase
    {
        protected HstViewModel()
        {
            ExceptionLog = LogManager.GetLogger(LogFileNames.Exceptions);

            // FailedReplaysLog = LogManager.GetLogger(LogFile.UnParsedReplaysLogFile);
            // SqlExceptionReplaysLog = LogManager.GetLogger(LogFile.SqlExceptionReplaysLogFile);

            WarningLog = LogManager.GetLogger(LogFileNames.WarningLogFileName);
            UnParsedReplaysLog = LogManager.GetLogger(LogFileNames.UnParsedReplaysLogFileName);

            // HotsLogsLog = LogManager.GetLogger(LogFileNames.);
        }

        protected Logger ExceptionLog { get; private set; }

        // protected Logger FailedReplaysLog { get; private set; }
        // protected Logger SqlExceptionReplaysLog { get; private set; }
        protected Logger WarningLog { get; private set; }
        protected Logger HotsLogsLog { get; private set; }
        protected Logger UnParsedReplaysLog { get; private set; }
    }
}