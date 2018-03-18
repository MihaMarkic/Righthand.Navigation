using System.ComponentModel;
using Android.OS;
using Android.Views;
using Android.Widget;
using Righthand.Navigation.Sample.ViewModels;
using System;

namespace Righthand.Navigation.Sample.Droid.Fragments
{
    public class FragmentTwo : BaseFragment<SecondPageViewModel>
    {
        Button forward;
        Button goBack;
        Button clearStack;
        TextView result;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Two, container, false);
            InitView(view);
            forward = view.FindViewById<Button>(Resource.Id.button);
            forward.Click += Button_Click;
            goBack = view.FindViewById<Button>(Resource.Id.go_back);
            goBack.Click += GoBack_Click;
            clearStack = view.FindViewById<Button>(Resource.Id.clear_stack);
            clearStack.Click += ClearStack_Click;
            result = view.FindViewById<TextView>(Resource.Id.result);
            return view;
        }

        void ClearStack_Click(object sender, EventArgs e)
        {
            if (ViewModel.ClearHistoryCommand.CanExecute(null))
            {
                ViewModel.ClearHistoryCommand.Execute(null);
            }
        }

        private void GoBack_Click(object sender, EventArgs e)
        {
            if (ViewModel.GoBackCommand.CanExecute(null))
            {
                ViewModel.GoBackCommand.Execute(null);
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            UpdateButtonEnabled();
            UpdateResultLabel();
            ViewModel.NextPageCommand.CanExecuteChanged += NextPageCommand_CanExecuteChanged;
            ViewModel.ClearHistoryCommand.CanExecuteChanged += ClearHistoryCommand_CanExecuteChanged;
            ViewModel.GoBackCommand.CanExecuteChanged += GoBackCommand_CanExecuteChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }
        void GoBackCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            goBack.Enabled = ViewModel.GoBackCommand.CanExecute(null);
        }
        void ClearHistoryCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            clearStack.Enabled = ViewModel.ClearHistoryCommand.CanExecute(null);
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
        void NextPageCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            UpdateButtonEnabled();
        }

        void UpdateButtonEnabled()
        {
            forward.Enabled = ViewModel.NextPageCommand.CanExecute(null);
        }

        void Button_Click(object sender, EventArgs e)
        {
            ViewModel.NextPageCommand.Execute(null);
        }

        public override void OnPause()
        {
            base.OnPause();
            ViewModel.NextPageCommand.CanExecuteChanged -= NextPageCommand_CanExecuteChanged;
            ViewModel.ClearHistoryCommand.CanExecuteChanged -= ClearHistoryCommand_CanExecuteChanged;
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.GoBackCommand.CanExecuteChanged -= GoBackCommand_CanExecuteChanged;
        }
    }
}