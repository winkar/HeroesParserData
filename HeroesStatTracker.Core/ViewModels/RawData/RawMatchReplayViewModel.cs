﻿using Heroes.Icons;
using HeroesStatTracker.Data.Models.Replays;
using HeroesStatTracker.Data.Queries.Replays;

namespace HeroesStatTracker.Core.ViewModels.RawData
{
    public class RawMatchReplayViewModel : RawDataBase<ReplayMatch>
    {
        public RawMatchReplayViewModel(IRawDataQueries<ReplayMatch> iRawDataQueries, IHeroesIconsService heroesIcons)
            : base(iRawDataQueries, heroesIcons)
        { }
    }
}