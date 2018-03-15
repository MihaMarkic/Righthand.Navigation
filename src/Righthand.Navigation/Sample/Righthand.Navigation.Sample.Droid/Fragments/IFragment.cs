using Righthand.Navigation.Sample.ViewModels;

namespace Righthand.Navigation.Sample.Droid.Fragments
{
    public interface IFragment
    {
        PageViewModel ViewModel { get; }
    }
}