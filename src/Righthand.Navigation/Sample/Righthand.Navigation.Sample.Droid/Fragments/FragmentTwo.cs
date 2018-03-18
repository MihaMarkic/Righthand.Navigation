using System.ComponentModel;
using Android.OS;
using Android.Views;
using Android.Widget;
using Righthand.Navigation.Sample.ViewModels;

namespace Righthand.Navigation.Sample.Droid.Fragments
{
    public class FragmentTwo : BaseFragment<SecondPageViewModel>
    {
        Button button;
        TextView result;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Two, container, false);
            InitView(view);
            button = view.FindViewById<Button>(Resource.Id.button);
            button.Click += Button_Click;
            result = view.FindViewById<TextView>(Resource.Id.result);
            return view;
        }
        public override void OnResume()
        {
            base.OnResume();
            UpdateButtonEnabled();
            UpdateResultLabel();
            ViewModel.NextPageCommand.CanExecuteChanged += NextPageCommand_CanExecuteChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }
        void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.Result):
                    UpdateResultLabel();
                    break;
            }
        }
        void UpdateResultLabel()
        {
            result.Text = $"Result from third is:'{ViewModel.Result}'";
        }
        void NextPageCommand_CanExecuteChanged(object sender, System.EventArgs e)
        {
            UpdateButtonEnabled();
        }

        void UpdateButtonEnabled()
        {
            button.Enabled = ViewModel.NextPageCommand.CanExecute(null);
        }

        void Button_Click(object sender, System.EventArgs e)
        {
            ViewModel.NextPageCommand.Execute(null);
        }

        public override void OnPause()
        {
            base.OnPause();
            ViewModel.NextPageCommand.CanExecuteChanged -= NextPageCommand_CanExecuteChanged;
        }
    }
}