﻿using HeroesMatchData.Core.HotsLogs;

namespace HeroesMatchData.Core.ViewServices
{
    public interface IMainTabService
    {
        /// <summary>
        /// Sets the selected Main Tab
        /// </summary>
        /// <param name="selectedMainTab"></param>
        void SwitchToTab(MainPage selectedMainTab);

        void SetApplicationStatus(string status);

        void SetReplayParserStatus(ReplayParserStatus status);

        void SetReplayParserWatchStatus(ReplayParserWatchStatus status);

        void SetReplayParserHotsLogsStatus(ReplayParserHotsLogsStatus status);

        void SetTotalParsedReplays(long amount);
    }
}