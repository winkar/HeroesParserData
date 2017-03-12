﻿using System.Collections.Generic;

namespace HeroesMatchData.Data.Migrations
{
    internal interface IMigrationCommand
    {
        void Command(Dictionary<int, List<string>> migrations, Dictionary<int, List<IMigrationAddon>> migrationAddons);
    }
}
