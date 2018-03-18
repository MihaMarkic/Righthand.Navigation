using System;
using CoreGraphics;
using Foundation;
using Righthand.Navigation.Sample.ViewModels;
using UIKit;

namespace Righthand.Navigation.Sample.iOS.ViewControllers
{
    public class ThirdViewController : BaseViewController<ThirdPageViewModel>
    {
        NSObject textChangeObserver;
        public ThirdViewController(ThirdPageViewModel viewModel) : base(viewModel)
        {
            View.BackgroundColor = UIColor.FromRGB(0x45, 0x5a, 0x64);
        }

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            var inputText = new UITextField(
                new CGRect(pageTitle.Frame.Left, pageTitle.Frame.Bottom + 10,
                           300, 40))
            {
                BackgroundColor = UIColor.White,
                Placeholder = "Type something",
            };
            Add(inputText);
            inputText.Text = viewModel.InputText;

            textChangeObserver = NSNotificationCenter.DefaultCenter.AddObserver(UITextField.TextFieldTextDidChangeNotification,
               (notification) =>
                {
                    if (notification.Object == inputText)
                    {
                        viewModel.InputText = inputText.Text;
                    }
                }
            );
		}
		public override void ViewDidUnload()
		{
            base.ViewDidUnload();
            NSNotificationCenter.DefaultCenter.RemoveObserver(textChangeObserver);
		}
	}
}
