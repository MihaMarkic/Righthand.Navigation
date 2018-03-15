using Android.OS;
using Android.Views;
using Android.Widget;
using Righthand.Navigation.Sample.ViewModels;

namespace Righthand.Navigation.Sample.Droid.Fragments
{
    public class FragmentOne: BaseFragment<FirstPageViewModel>
    {
        Button button;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.One, container, false);
            InitView(view);
            button = view.FindViewById<Button>(Resource.Id.button);
            button.Click += Button_Click;
            return view;
        }
        public override void OnResume()
        {
            base.OnResume();
            UpdateButtonEnabled();
            ViewModel.NextPageCommand.CanExecuteChanged += NextPageCommand_CanExecuteChanged;
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