using Righthand.Navigation.Sample.ViewModels;

namespace Righthand.Navigation.Sample.Droid
{
    public interface IActivityHost
    {
        PageViewModel GetViewModel();
    }
}