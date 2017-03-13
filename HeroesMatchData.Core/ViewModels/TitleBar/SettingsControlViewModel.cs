﻿using GalaSoft.MvvmLight;
using HeroesMatchData.Data;

namespace HeroesMatchData.Core.ViewModels.TitleBar
{
    public class SettingsControlViewModel : ViewModelBase
    {
        private IDatabaseService Database;

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsControlViewModel(IDatabaseService database)
        {
            Database = database;
        }

        public bool IsMinimizeToTray
        {
            get => Database.SettingsDb().UserSettings.IsMinimizeToTray;
            set
            {
                Database.SettingsDb().UserSettings.IsMinimizeToTray = value;
                RaisePropertyChanged();
            }
        }

        public bool IsAutoUpdates
        {
            get => Database.SettingsDb().UserSettings.IsAutoUpdates;
            set
            {
                Database.SettingsDb().UserSettings.IsAutoUpdates = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBattleTagsHidden
        {
            get => Database.SettingsDb().UserSettings.IsBattleTagHidden;
            set
            {
                Database.SettingsDb().UserSettings.IsBattleTagHidden = value;
                RaisePropertyChanged();
            }
        }
    }
}