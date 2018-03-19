using System;
using System.Threading;
using System.Threading.Tasks;

namespace Righthand.Navigation
{
    public interface INavigationService<TPage> where TPage : IPage<TPage>
    {
        event EventHandler<PageNavigatedEventArgs<TPage>> PageNavigated;
        event EventHandler<NavigationHistoryClearedEventArgs> NavigationHistoryCleared;
        int NavigationDepth { get; }
        ValueTask<bool> GoBackAsync(bool isManual);
        ValueTask<(bool DidNavigate, TNextPage Result)>NavigateAsync<TNextPage>(TNextPage to, bool waitFor, CancellationToken ct)
            where TNextPage: TPage;
        void ClearHistory();
    }
}
