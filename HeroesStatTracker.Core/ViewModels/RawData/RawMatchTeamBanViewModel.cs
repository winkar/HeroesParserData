﻿using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchTeamBanViewModel : RawDataContextBase<ReplayMatchTeamBan>
    {
        public RawMatchTeamBanViewModel(IRawDataQueries<ReplayMatchTeamBan> iRawDataQueries)
            : base(iRawDataQueries)
        { }
    }
}