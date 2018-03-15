
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Righthand.Navigation.Sample.ViewModels;

namespace Righthand.Navigation.Sample.Droid.Fragments
{
    public abstract class BaseFragment<TViewModel>: Fragment, IFragment
        where TViewModel: PageViewModel
    {
        protected TViewModel ViewModel { get; private set; }
        PageViewModel IFragment.ViewModel => ViewModel;
        protected TextView title;
        public BaseFragment()
        { }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (Activity is IActivityHost activityHost)
            {
                ViewModel = (TViewModel)activityHost.GetViewModel();
            }
        }
        protected virtual void InitView(View view)
        {
            title = view.FindViewById<TextView>(Resource.Id.title);
            if (ViewModel != null)
            {
                title.Text = ViewModel.Title;
            }
        }
    }
}