using System;
using Righthand.Navigation.Sample.ViewModels;
using UIKit;

namespace Righthand.Navigation.Sample.iOS
{
    public partial class MainViewController : UINavigationController
    {
        readonly MasterViewModel viewModel;
        public MainViewController(MasterViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var text = new UITextView { Text = "No page" };
            Add(text);
        }
    }
}
