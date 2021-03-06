﻿using System.ComponentModel;
using CoreGraphics;
using Righthand.Navigation.Sample.ViewModels;
using System;
using UIKit;

namespace Righthand.Navigation.Sample.iOS.ViewControllers
{
    public class SecondViewController: BaseViewController<SecondPageViewModel>
    {
        UILabel result;
        UIButton goBack;
        UIButton clearStack;
        public SecondViewController(SecondPageViewModel viewModel):base(viewModel)
        {
            View.BackgroundColor = UIColor.FromRGB(0x1a, 0x23, 0x7e);
        }
		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            var button = AddForwardButton("To Third");
            button.TouchUpInside += Button_TouchUpInside;
            result = new UILabel(new CGRect(10, button.Frame.Bottom+10, View.Bounds.Width, 40))
            {
                TextColor = UIColor.White,
                BackgroundColor = UIColor.Clear,
            };
            Add(result);
            UpdateResultLabel();
            goBack = AddAdditionalButton("Go back", result);
            goBack.TouchUpInside += GoBack_TouchUpInside;
            clearStack = AddAdditionalButton("Clear navigation stack", goBack);
            clearStack.TouchUpInside += ClearStack_TouchUpInside;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.GoBackCommand.CanExecuteChanged += GoBackCommand_CanExecuteChanged;
            viewModel.ClearHistoryCommand.CanExecuteChanged += ClearHistoryCommand_CanExecuteChanged;
        }
        void ClearHistoryCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            clearStack.Enabled = viewModel.ClearHistoryCommand.CanExecute(null);
        }
        void GoBackCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            goBack.Enabled = viewModel.GoBackCommand.CanExecute(null);
        }
        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            viewModel.GoBackCommand.CanExecuteChanged -= GoBackCommand_CanExecuteChanged;
            viewModel.ClearHistoryCommand.CanExecuteChanged -= ClearHistoryCommand_CanExecuteChanged;
        }
        void ClearStack_TouchUpInside(object sender, EventArgs e)
        {
            if (viewModel.ClearHistoryCommand.CanExecute(null))
            {
                viewModel.ClearHistoryCommand.Execute(null);
            }
            var navigationController = (UINavigationController)ParentViewController;
            Console.WriteLine($"NavigationController stack depth after clear {navigationController.ViewControllers.Length}");
        }
        void GoBack_TouchUpInside(object sender, EventArgs e)
        {
            if (viewModel.GoBackCommand.CanExecute(null))
            {
                viewModel.GoBackCommand.Execute(null);
            }
        }
        UIButton AddAdditionalButton(string title, UIView viewAbove)
        {
            var button = new UIButton
            {
                Frame = new CGRect(new CGPoint(10, viewAbove.Frame.Bottom + 10), CGSize.Empty)
            };
            button.SetTitle(title, UIControlState.Normal);
            button.SetTitleColor(UIColor.Gray, UIControlState.Disabled);
            button.SizeToFit();
            Add(button);
            return button;
        }
        void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(viewModel.Result):
                    UpdateResultLabel();
                    break;
            }
        }
        void UpdateResultLabel()
        {
            result.Text = $"Result from third is:'{viewModel.Result}'";
        }

        void Button_TouchUpInside(object sender, EventArgs e)
        {
            viewModel.NextPageCommand.Execute(null);
        }
	}
}
