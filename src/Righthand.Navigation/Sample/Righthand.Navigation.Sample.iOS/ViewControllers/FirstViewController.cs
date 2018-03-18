using Righthand.Navigation.Sample.ViewModels;
using UIKit;

namespace Righthand.Navigation.Sample.iOS.ViewControllers
{
    public class FirstViewController: BaseViewController<FirstPageViewModel>
    {
        public FirstViewController(FirstPageViewModel viewModel):base(viewModel)
        {
            View.BackgroundColor = UIColor.FromRGB(0x99, 0x2c, 0x2c);
        }
		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            var button = AddButton("To Second");
            button.TouchUpInside += Button_TouchUpInside;
		}

        void Button_TouchUpInside(object sender, System.EventArgs e)
        {
            viewModel.NextPageCommand.Execute(null);
        }
	}
}
