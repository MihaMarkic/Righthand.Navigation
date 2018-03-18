using CoreGraphics;
using Righthand.Navigation.Sample.ViewModels;
using UIKit;

namespace Righthand.Navigation.Sample.iOS.ViewControllers
{
    public abstract class BaseViewController<TViewModel>: UIViewController
        where TViewModel: PageViewModel
    {
        protected TViewModel viewModel;
        UITextView pageTitle;
        public BaseViewController(TViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            pageTitle = new UITextView(new CGRect(10, 50, View.Bounds.Width, 40)) { 
                Text = viewModel.Title,
                TextColor = UIColor.White,
                BackgroundColor = UIColor.Clear,
            };
            Add(pageTitle);
		}
        protected UIButton AddButton(string title)
        {
            var button = new UIButton();
            button.SetTitle(title, UIControlState.Normal);
            button.Frame = new CGRect(new CGPoint(10, pageTitle.Frame.Bottom + 10),
                                      button.Frame.Size);
            button.SizeToFit();
            Add(button);
            return button;
        }
	}
}
