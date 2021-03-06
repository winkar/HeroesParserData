﻿using HeroesParserData.DataQueries;
using System.Windows.Input;

namespace HeroesParserData.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _inputBattleTagError;

        public bool IsMinimizeToTray
        {
            get { return UserSettings.Default.IsMinimizeToTray; }
            set
            {
                UserSettings.Default.IsMinimizeToTray = value;
                RaisePropertyChangedEvent(nameof(IsMinimizeToTray));
            }
        }

        public bool IsAutoUpdates
        {
            get { return UserSettings.Default.IsAutoUpdates; }
            set
            {
                UserSettings.Default.IsAutoUpdates = value;

                RaisePropertyChangedEvent(nameof(IsAutoUpdates));
            }
        }

        public bool IsBattleTagsHidden
        {
            get { return UserSettings.Default.IsBattleTagHidden; }
            set
            {
                UserSettings.Default.IsBattleTagHidden = value;
                RaisePropertyChangedEvent(nameof(IsBattleTagsHidden));
            }
        }

        public string UserBattleTag
        {
            get { return UserSettings.Default.UserBattleTagName; }
            set
            {
                UserSettings.Default.UserBattleTagName = value;
                RaisePropertyChangedEvent(nameof(UserBattleTag));
            }
        }

        public string InputBattleTagError
        {
            get { return _inputBattleTagError; }
            set
            {
                _inputBattleTagError = value;
                RaisePropertyChangedEvent(nameof(InputBattleTagError));
            }
        }

        public ICommand SetUserBattleTagCommand
        {
            get { return new DelegateCommand(() => SetBattleTagName()); }
        }

        /// <summary>
        /// Contructor
        /// </summary>
        public SettingsViewModel()
            :base()
        { }

        private void SetBattleTagName()
        {
            if (string.IsNullOrEmpty(UserBattleTag))
            {
                UserSettings.Default.UserPlayerId = 0;
                InputBattleTagError = string.Empty;
            }
            else if (ValidateBattleTagName(UserBattleTag))
            {
                UserSettings.Default.UserPlayerId = Query.HotsPlayer.ReadPlayerIdFromBattleNetTag(UserBattleTag);
                InputBattleTagError = string.Empty;
            }
            else
            {
                UserSettings.Default.UserPlayerId = 0;
                InputBattleTagError = "BattleTag not found";
            }
        }

        private bool ValidateBattleTagName(string battleTagName)
        {
            return Query.HotsPlayer.IsValidBattleNetTagName(battleTagName);
        }
    }
}
