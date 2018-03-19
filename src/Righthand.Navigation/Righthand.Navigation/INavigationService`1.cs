using System;
using System.Threading;
using System.Threading.Tasks;

namespace Righthand.Navigation
{
    /// <summary>
    /// Defines a navigation service.
    /// </summary>
    /// <typeparam name="TPage">Type of page.</typeparam>
    public interface INavigationService<TPage> where TPage : IPage<TPage>
    {
        /// <summary>
        /// Occurs after a page has been navigated wither forward or backward.
        /// </summary>
        event EventHandler<PageNavigatedEventArgs<TPage>> PageNavigated;
        /// <summary>
        /// Occurs when navigation history or a part of has been manually cleared.
        /// </summary>
        event EventHandler<NavigationHistoryClearedEventArgs> NavigationHistoryCleared;
        /// <summary>
        /// Current depth of navigation history. Current page not included.
        /// </summary>
        int NavigationDepth { get; }
        /// <summary>
        /// Navigates back if possible.
        /// </summary>
        /// <param name="isManual">True when navigation is manually triggered, false otherwise.</param>
        /// <returns>True when navigation occurred, false otherwise.</returns>
        ValueTask<bool> GoBackAsync(bool isManual);
        /// <summary>
        /// Navigates forward.
        /// </summary>
        /// <typeparam name="TNextPage">Type of <paramref name="to"/>.</typeparam>
        /// <param name="to">An instance of next page.</param>
        /// <param name="waitFor">True when call should await for navigation back (results), false otherwise.</param>
        /// <param name="ct">The cancellation token that will be checked prior to completing the returned task.</param>
        /// <returns>A value representing the navigation success (DidNavigate) and a navigation result (Result) when page is awaited.</returns>
        ValueTask<(bool DidNavigate, TNextPage Result)>NavigateAsync<TNextPage>(TNextPage to, bool waitFor, CancellationToken ct)
            where TNextPage: TPage;
        /// <summary>
        /// Clears navigation history.
        /// </summary>
        void ClearHistory();
    }
}
