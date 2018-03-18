namespace Righthand.Navigation.Sample.ViewModels
{
    public class ThirdPageViewModel: PageViewModel
    {
        public override string Title => "Third";
        public string InputText { get; set; }
        public ThirdPageViewModel(INavigationService<PageViewModel> navigationService) : 
            base(navigationService)
        {
        }
    }
}
