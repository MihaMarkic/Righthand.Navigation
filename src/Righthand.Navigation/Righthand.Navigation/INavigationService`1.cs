using System;
using System.Threading;
using System.Threading.Tasks;

namespace Righthand.Navigation
{
    public interface INavigationService<TPage> where TPage : IPage<TPage>
    {
        void Clear();
        ValueTask<bool> GoBackAsync(object args = null);
        ValueTask<(bool didNavigate, TNextPage Result)>NavigateAsync<TNextPage>(TNextPage to, bool waitFor, CancellationToken ct)
            where TNextPage: TPage;
        int NavigationDepth { get; }
        event EventHandler<PageNavigatedEventArgs<TPage>> PageNavigated;
    }
}
