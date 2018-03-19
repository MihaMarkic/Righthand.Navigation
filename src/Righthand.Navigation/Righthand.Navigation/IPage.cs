using System.ComponentModel;
using System.Threading.Tasks;

namespace Righthand.Navigation
{
    public interface IPage<T>
    {
        void Navigated(T from, NavigationDirection direction);
        ValueTask<bool> CanNavigate(T to);
        // dispose here
        void Removed();
    }
}
