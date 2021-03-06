﻿using System.Windows;

namespace HeroesParserData.Models.MatchModels
{
    public class MatchScores : MatchPlayerInfoBase
    {
        public int? SoloKills { get; set; }
        public int? Assists { get; set; }
        public int? Deaths { get; set; }
        public int? SiegeDamage { get; set; }
        public FontWeight HighestSiegeFont { get; set;}
        public int? HeroDamage { get; set; }
        public FontWeight HighestHeroDamageFont { get; set; }
        public int? Role { get; set; }
        public int? ExperienceContribution { get; set; }
        public FontWeight HighestExpFont { get; set; }

        public MatchScores() { }

        public MatchScores(MatchPlayerInfoBase matchPlayerInfoBase)
        {
            LeaderboardPortrait = matchPlayerInfoBase.LeaderboardPortrait;
            PartyIcon = matchPlayerInfoBase.PartyIcon;
            PlayerName = matchPlayerInfoBase.PlayerName;
            CharacterName = matchPlayerInfoBase.CharacterName;
            CharacterLevel = matchPlayerInfoBase.CharacterLevel;
            PlayerSilenced = matchPlayerInfoBase.PlayerSilenced;
            MvpAward = matchPlayerInfoBase.MvpAward;
            MvpAwardName = matchPlayerInfoBase.MvpAwardName;
            RowBackColor = matchPlayerInfoBase.RowBackColor;
            PlayerSearchContextMenu = matchPlayerInfoBase.PlayerSearchContextMenu;
        }
    }
}
