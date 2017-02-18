﻿using GalaSoft.MvvmLight.Ioc;
using Heroes.Icons;
using HeroesStatTracker.Core.ViewServices;

namespace HeroesStatTracker.Core.ViewModels.Matches
{
    public class MatchesViewModel : HstViewModel, IMatchesTabService
    {
        private int _selectedMatchesTab;

        public MatchesViewModel(IHeroesIconsService heroesIcons)
            : base(heroesIcons)
        {
            SimpleIoc.Default.Register<IMatchesTabService>(() => this);
        }

        public int SelectedMatchesTab
        {
            get { return _selectedMatchesTab; }
            set
            {
                _selectedMatchesTab = value;
                RaisePropertyChanged();
            }
        }

        public void SwitchToTab(MatchesTabs selectedMatchesTab)
        {
            SelectedMatchesTab = (int)selectedMatchesTab;
        }
    }
}