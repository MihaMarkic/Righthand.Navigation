using System.Threading.Tasks;

namespace Righthand.Navigation.Sample.ViewModels
{
    public abstract class PageViewModel : ViewModel, IPage<PageViewModel>
    {
        readonly protected INavigationService<PageViewModel> navigationService;
        public abstract string Title { get; }
        public PageViewModel(INavigationService<PageViewModel> navigationService)
        {
            this.navigationService = navigationService;
        }
        public virtual ValueTask<bool> CanNavigate(PageViewModel to)
        {
            return new ValueTask<bool>(true);
        }
        public virtual void Navigated(PageViewModel from, NavigationDirection direction)
        {}
        public virtual void Removed()
        {
            Dispose();
        }
    }
}
