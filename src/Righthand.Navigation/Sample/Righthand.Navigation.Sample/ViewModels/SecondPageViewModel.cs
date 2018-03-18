using Righthand.Navigation.Sample.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Righthand.Navigation.Sample.ViewModels
{
    public class SecondPageViewModel: PageViewModel
    {
        public override string Title => "Second";
        public RelayCommand NextPageCommand { get; }
        public RelayCommand GoBackCommand { get; }
        public RelayCommand ClearHistoryCommand { get; }
        public string Result { get; private set; }
        public SecondPageViewModel(INavigationService<PageViewModel> navigationService) : base(navigationService)
        {
            NextPageCommand = new RelayCommand(() => { var ignore = NavigateToThirdAndWaitForResultAsync(); });
            GoBackCommand = new RelayCommand(() => { var ignore = navigationService.GoBackAsync(isManual: true); }, () => navigationService.NavigationDepth > 0);
            ClearHistoryCommand = new RelayCommand(() => navigationService.ClearHistory(), () => navigationService.NavigationDepth > 0);
            navigationService.NavigationHistoryCleared += NavigationService_NavigationHistoryCleared;
        }
        void NavigationService_NavigationHistoryCleared(object sender, NavigationHistoryClearedEventArgs e)
        {
            ClearHistoryCommand.RaiseCanExecuteChanged();
            GoBackCommand.RaiseCanExecuteChanged();
        }
        async Task NavigateToThirdAndWaitForResultAsync()
        {
            var (didNavigate, result) = await navigationService.NavigateAsync(new ThirdPageViewModel(navigationService),
                                                  waitFor: true, ct: CancellationToken.None);
            if (didNavigate)
            {
                Result = result.InputText;
            }
        }
        protected override void OnDispose(bool isDisposing)
        {
            if (isDisposing)
            {
                navigationService.NavigationHistoryCleared -= NavigationService_NavigationHistoryCleared;
            }
            base.OnDispose(isDisposing);
        }
    }
}
