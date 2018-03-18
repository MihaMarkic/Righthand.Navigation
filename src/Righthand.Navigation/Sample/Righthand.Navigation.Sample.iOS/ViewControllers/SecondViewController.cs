using Righthand.Navigation.Sample.ViewModels;
using UIKit;

namespace Righthand.Navigation.Sample.iOS.ViewControllers
{
    public class SecondViewController: BaseViewController<SecondPageViewModel>
    {
        public SecondViewController(SecondPageViewModel viewModel):base(viewModel)
        {
            View.BackgroundColor = UIColor.FromRGB(0x1a, 0x23, 0x7e);
        }
		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            var button = AddButton("To Third");
            button.TouchUpInside += Button_TouchUpInside;
        }

        void Button_TouchUpInside(object sender, System.EventArgs e)
        {
            viewModel.NextPageCommand.Execute(null);
        }
	}
}
