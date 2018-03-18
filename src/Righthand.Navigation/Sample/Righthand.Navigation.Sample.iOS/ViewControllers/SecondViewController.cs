using System.ComponentModel;
using CoreGraphics;
using Righthand.Navigation.Sample.ViewModels;
using UIKit;

namespace Righthand.Navigation.Sample.iOS.ViewControllers
{
    public class SecondViewController: BaseViewController<SecondPageViewModel>
    {
        UILabel result;
        public SecondViewController(SecondPageViewModel viewModel):base(viewModel)
        {
            View.BackgroundColor = UIColor.FromRGB(0x1a, 0x23, 0x7e);
        }
		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            var button = AddButton("To Third");
            button.TouchUpInside += Button_TouchUpInside;
            result = new UILabel(new CGRect(10, button.Frame.Bottom+10, View.Bounds.Width, 40))
            {
                TextColor = UIColor.White,
                BackgroundColor = UIColor.Clear,
            };
            Add(result);
            UpdateResultLabel();
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
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

        void Button_TouchUpInside(object sender, System.EventArgs e)
        {
            viewModel.NextPageCommand.Execute(null);
        }
	}
}
