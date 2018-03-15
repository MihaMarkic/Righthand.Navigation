using System.Threading;

namespace Righthand.Navigation.Sample.ViewModels
{
    public class MasterViewModel: ViewModel
    {
        public INavigationService<PageViewModel> NavigationService { get; }
        public PageViewModel CurrentPage { get; private set; }
        public MasterViewModel()
        {
            NavigationService = new NavigationService<PageViewModel>();
            NavigationService.PageNavigated += NavigationService_PageNavigated;
        }
        public void Init()
        {
            NavigationService.NavigateAsync(new FirstPageViewModel(NavigationService), waitFor: false, ct: CancellationToken.None);
        }

        private void NavigationService_PageNavigated(object sender, PageNavigatedEventArgs<PageViewModel> e)
        {
            CurrentPage = e.To;
        }
    }
}
