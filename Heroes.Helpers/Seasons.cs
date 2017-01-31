﻿using System;
using System.Collections.Generic;

namespace Heroes.Helpers
{
    [Flags]
    public enum Season
    {
        Lifetime = 0,
        Preseason = 1 << 0,
        Season1 = 1 << 1,
        Season2 = 1 << 2,
        Season3 = 1 << 3,
    }

    public static partial class HeroesHelpers
    {
        public static class Seasons
        {
            public static List<string> GetSeasonList()
            {
                List<string> list = new List<string>();
                list.Add("Preseason");
                list.Add("Season 1");
                list.Add("Season 2");
                list.Add("Season 3");

                return list;
            }
        }
    }
}