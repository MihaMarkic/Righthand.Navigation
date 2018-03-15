using Righthand.Navigation.Sample.Core;
using System.Threading;

namespace Righthand.Navigation.Sample.ViewModels
{
    public class FirstPageViewModel: PageViewModel
    {
        public override string Title => "First";
        public RelayCommand NextPageCommand { get; }
        public FirstPageViewModel(INavigationService<PageViewModel> navigationService): base(navigationService)
        {
            NextPageCommand = new RelayCommand(() => 
                navigationService.NavigateAsync(new SecondPageViewModel(navigationService), waitFor: false, ct: CancellationToken.None));
        }
    }
}
