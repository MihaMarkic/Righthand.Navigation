using Righthand.Navigation.Sample.ViewModels;
using UIKit;

namespace Righthand.Navigation.Sample.iOS.ViewControllers
{
    public class ThirdViewController : BaseViewController<ThirdPageViewModel>
    {
        public ThirdViewController(ThirdPageViewModel viewModel) : base(viewModel)
        {
            View.BackgroundColor = UIColor.FromRGB(0x45, 0x5a, 0x64);

        }
    }
}
