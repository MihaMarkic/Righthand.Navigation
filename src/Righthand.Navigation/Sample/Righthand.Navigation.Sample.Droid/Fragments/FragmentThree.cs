using Android.OS;
using Android.Runtime;
using Android.Views;
using Righthand.Navigation.Sample.ViewModels;
using System;

namespace Righthand.Navigation.Sample.Droid.Fragments
{
    public class FragmentThree : BaseFragment<ThirdPageViewModel>
    {
        public FragmentThree()
        { }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Three, container, false);
            InitView(view);
            return view;
        }
    }
}