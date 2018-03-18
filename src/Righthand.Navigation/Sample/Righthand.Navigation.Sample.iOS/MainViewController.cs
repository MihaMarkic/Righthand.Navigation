using System;
using Righthand.Navigation.Sample.iOS.ViewControllers;
using Righthand.Navigation.Sample.ViewModels;
using UIKit;

namespace Righthand.Navigation.Sample.iOS
{
    public sealed class MainViewController : UINavigationController
    {
        readonly MasterViewModel viewModel;
        public MainViewController(MasterViewModel viewModel)
        {
            this.viewModel = viewModel;
            viewModel.Init();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            if (viewModel.CurrentPage != null)
            {
                UpdateViewController(viewModel.CurrentPage);
            }
            viewModel.NavigationService.PageNavigated += ViewModel_PageNavigated;
        }
        void ViewModel_PageNavigated(object sender, PageNavigatedEventArgs<PageViewModel> e)
        {
            if (!e.IsBack)
            {
                UpdateViewController(e.To);
            }
        }
        void UpdateViewController(PageViewModel pageViewModel)
        {
            var viewController = GetViewController(pageViewModel);
            PushViewController(viewController, animated: true);
        }
        UIViewController GetViewController(PageViewModel pageViewModel)
        {
            switch (pageViewModel)
            {
                case FirstPageViewModel first:
                    return new FirstViewController(first);
                case SecondPageViewModel second:
                    return new SecondViewController(second);
                case ThirdPageViewModel third:
                    return new ThirdViewController(third);
                default:
                    throw new ArgumentOutOfRangeException(nameof(pageViewModel),
                      $"No ViewController for page {pageViewModel} available");
            }
        }
		public override UIViewController PopViewController(bool animated)
		{
            // this can be asynchronous only when navigating forward
            // also this call is required, otherwise navigation stack is not synced 
            // with controller anymore
            var ignore = viewModel.NavigationService.GoBackAsync(null);
            return base.PopViewController(animated);
		}
		protected override void Dispose(bool disposing)
		{
            if (disposing)
            {
                viewModel.NavigationService.PageNavigated -= ViewModel_PageNavigated;
            }
            base.Dispose(disposing);
		}
	}
}
