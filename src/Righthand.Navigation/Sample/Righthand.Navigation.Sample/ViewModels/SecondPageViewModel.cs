﻿using Righthand.Navigation.Sample.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Righthand.Navigation.Sample.ViewModels
{
    public class SecondPageViewModel: PageViewModel
    {
        public override string Title => "Second";
        public RelayCommand NextPageCommand { get; }
        public string Result { get; private set; }
        public SecondPageViewModel(INavigationService<PageViewModel> navigationService) : base(navigationService)
        {
            NextPageCommand = new RelayCommand(
                () => { 
                var ignore = NavigateAndWaitForResultAsync(); 
            });
        }
        async Task NavigateAndWaitForResultAsync()
        {
            var (didNavigate, result) = await navigationService.NavigateAsync(new ThirdPageViewModel(navigationService),
                                                  waitFor: true, ct: CancellationToken.None);
            if (didNavigate)
            {
                Result = result.InputText;
            }
        }
    }
}
