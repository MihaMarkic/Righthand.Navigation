using System.ComponentModel;
using System.Threading.Tasks;

namespace Righthand.Navigation
{
    public interface IPage<T>
    {
        void Navigated(T from, bool isBack);
        ValueTask<bool> CanNavigate(T to);
        // dispose here
        void Removed();
    }
}
