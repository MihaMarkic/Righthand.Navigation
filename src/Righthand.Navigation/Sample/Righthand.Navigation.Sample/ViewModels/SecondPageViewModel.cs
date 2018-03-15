using Righthand.Navigation.Sample.Core;
using System.Threading;

namespace Righthand.Navigation.Sample.ViewModels
{
    public class SecondPageViewModel: PageViewModel
    {
        public override string Title => "Second";
        public RelayCommand NextPageCommand { get; }
        public SecondPageViewModel(INavigationService<PageViewModel> navigationService) : base(navigationService)
        {
            NextPageCommand = new RelayCommand(() =>
                navigationService.NavigateAsync(new ThirdPageViewModel(navigationService), waitFor: false, ct: CancellationToken.None));
        }
    }
}
