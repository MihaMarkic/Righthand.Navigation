using Android.OS;
using Android.Views;
using Android.Widget;
using Righthand.Navigation.Sample.ViewModels;

namespace Righthand.Navigation.Sample.Droid.Fragments
{
    public class FragmentThree : BaseFragment<ThirdPageViewModel>
    {
        EditText inputText;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Three, container, false);
            InitView(view);
            inputText = view.FindViewById<EditText>(Resource.Id.inputText);
            inputText.Text = ViewModel.InputText;
            inputText.TextChanged += (s, e) =>
                ViewModel.InputText = inputText.Text;
            return view;
        }
	}
}