using System;
using System.Threading;
using System.Threading.Tasks;

namespace Righthand.Navigation
{
    public interface INavigationService<TPage> where TPage : IPage<TPage>
    {
        void Clear();
        ValueTask<bool> GoBackAsync(object args = null);
        ValueTask<(bool didNavigate, TPage Result)> NavigateAsync(TPage to, bool waitFor, CancellationToken ct);
        int NavigationDepth { get; }
        event EventHandler<PageNavigatedEventArgs<TPage>> PageNavigated;
    }
}
