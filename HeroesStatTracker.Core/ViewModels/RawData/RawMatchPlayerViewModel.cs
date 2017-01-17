﻿using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchPlayerViewModel : RawDataContextBase<ReplayMatchPlayer>
    {
        public RawMatchPlayerViewModel(IRawDataQueries<ReplayMatchPlayer> iRawDataQueries)
            : base(iRawDataQueries)
        { }
    }
}
