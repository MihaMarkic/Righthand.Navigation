﻿using System;
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
            viewModel.NavigationService.NavigationHistoryCleared += NavigationService_NavigationHistoryCleared;
        }
        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            viewModel.NavigationService.PageNavigated -= ViewModel_PageNavigated;
            viewModel.NavigationService.NavigationHistoryCleared -= NavigationService_NavigationHistoryCleared;
        }
        void NavigationService_NavigationHistoryCleared(object sender, NavigationHistoryClearedEventArgs e)
        {
            for (int i = 0; i < e.Count; i++)
            {
                UIViewController[] newControllers = new UIViewController[] { ViewControllers[ViewControllers.Length - 1] };
                SetViewControllers(newControllers, animated: false);
            }
        }
        bool isManualBack;
        void ViewModel_PageNavigated(object sender, PageNavigatedEventArgs<PageViewModel> e)
        {
            switch(e.Direction)
            {
                case NavigationDirection.Forward:
                    UpdateViewController(e.To);
                    break;
                case NavigationDirection.ManualBack:
                    isManualBack = true;
                    try
                    {
                        PopViewController(animated: true);
                    }
                    finally
                    {
                        isManualBack = false;
                    }
                    break;
            }
            Console.WriteLine($"NavigationController stack depth after navigation {ViewControllers.Length}");
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
            // also this call is required, otherwise navigation stack is not synchronized 
            // with controller anymore
            if (!isManualBack)
            {
                var ignore = viewModel.NavigationService.GoBackAsync(isManual: false);
            }
            var result = base.PopViewController(animated);
            Console.WriteLine($"History depth after navigation is {viewModel.NavigationService.NavigationDepth}");
            return result;
		}
	}
}
