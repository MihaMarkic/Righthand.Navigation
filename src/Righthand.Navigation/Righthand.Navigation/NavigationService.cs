using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Righthand.Navigation
{
    /// <summary>
    /// Defines a navigation service.
    /// </summary>
    /// <typeparam name="TPage">Type of page.</typeparam>
    public class NavigationService<TPage> : INavigationService<TPage>
            where TPage : IPage<TPage>
    {
        readonly Stack<HistoryItem<TPage>> history = new Stack<HistoryItem<TPage>>();
        /// <summary>
        /// Occurs after a page has been navigated wither forward or backward.
        /// </summary>
        public event EventHandler<PageNavigatedEventArgs<TPage>> PageNavigated;
        /// <summary>
        /// Occurs when navigation history or a part of has been manually cleared.
        /// </summary>
        public event EventHandler<NavigationHistoryClearedEventArgs> NavigationHistoryCleared;
        HistoryItem<TPage> current;
        /// <summary>
        /// Raises <see cref="PageNavigated"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnPageNavigated(PageNavigatedEventArgs<TPage> e) => PageNavigated?.Invoke(this, e);
        /// <summary>
        /// Raises <see cref="NavigationHistoryCleared"/> event.
        /// </summary>
        /// <param name="e">Event argumetns.</param>
        protected virtual void OnNavigationHistoryCleared(NavigationHistoryClearedEventArgs e) => NavigationHistoryCleared?.Invoke(this, e);
        // <summary>
        /// Current depth of navigation history. Current page not included.
        /// </summary>
        public int NavigationDepth => history.Count;
        /// <summary>
        /// Clears navigation history.
        /// </summary>
        public void ClearHistory()
        {
            int count = history.Count;
            while (history.Count > 0)
            {
                var item = history.Pop();
                item.Page.Removed();
                OnNavigationHistoryCleared(new NavigationHistoryClearedEventArgs(0, count));
            }
        }
        /// <summary>
        /// Navigates forward.
        /// </summary>
        /// <typeparam name="TNextPage">Type of <paramref name="to"/>.</typeparam>
        /// <param name="to">An instance of next page.</param>
        /// <param name="waitFor">True when call should await for navigation back (results), false otherwise.</param>
        /// <param name="ct">The cancellation token that will be checked prior to completing the returned task.</param>
        /// <returns>A value representing the navigation success (DidNavigate) and a navigation result (Result) when page is awaited.</returns>
        public async ValueTask<(bool DidNavigate, TNextPage Result)> NavigateAsync<TNextPage>(TNextPage to, bool waitFor, CancellationToken ct)
            where TNextPage : TPage        
        {
            var returnsTo = current;
            bool didNavigate = await NavigateAsync(to, NavigationDirection.Forward, isAwaited: waitFor);
            if (!didNavigate)
            {
                return (false, default(TNextPage));
            }
            else
            {
                if (waitFor)
                {
                    try
                    {
                        var from = await WaitForBackToAsync(returnsTo.Page, ct);
                        return (true, (TNextPage)from);
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                }
                else
                {
                    return (true, default(TNextPage));
                }
            }
        }
        Task<TNextPage> WaitForBackToAsync<TNextPage>(TNextPage page, CancellationToken ct)
            where TNextPage: TPage
        {
            var tcs = new TaskCompletionSource<TNextPage>();
            EventHandler<PageNavigatedEventArgs<TPage>> handler = null;
            handler = (s, e) =>
            {
                if (e.Direction.IsBack() && ReferenceEquals(e.To, page))
                {
                    PageNavigated -= handler;
                    tcs.TrySetResult((TNextPage)e.From);
                }
            };
            ct.Register(() =>
            {
                PageNavigated -= handler;
                tcs.SetCanceled();
            });
            PageNavigated += handler;
            return tcs.Task;
        }
        async ValueTask<bool> NavigateAsync(TPage to, NavigationDirection direction, bool isAwaited)
        {
            bool canNavigate;
            if (current == null || direction.IsBack())
            {
                canNavigate = true;
            }
            else
            {
                canNavigate = await current.Page.CanNavigate(to);
            }
            if (canNavigate)
            {
                var previousCurrent = current;
                switch (direction)
                {
                    case NavigationDirection.Forward:
                        if (current != null)
                        {
                            history.Push(current);
                        }
                        current = new HistoryItem<TPage>(to, isAwaited);
                        var from = previousCurrent != null ? previousCurrent.Page : default(TPage);
                        OnPageNavigated(new PageNavigatedEventArgs<TPage>(from, to, NavigationDirection.Forward));
                        break;
                    default:
                        var historyItem = history.Pop();
                        OnPageNavigated(new PageNavigatedEventArgs<TPage>(current.Page, historyItem.Page, direction));
                        if (!current.IsAwaited)
                        {
                            current.Page.Removed();
                        }
                        current = historyItem;
                        break;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Navigates back if possible.
        /// </summary>
        /// <param name="isManual">True when navigation is manually triggered, false otherwise.</param>
        /// <returns>True when navigation occurred, false otherwise.</returns>
        public ValueTask<bool> GoBackAsync(bool isManual)
        {
            if (history.Count > 0)
            {
                var previous = history.Peek();
                var direction = isManual ? NavigationDirection.ManualBack : NavigationDirection.AutomaticBack;
                // isAwaited isn't used here - only when navigating forward
                return NavigateAsync(previous.Page, direction, isAwaited: false);
            }
            else
            {
                return new ValueTask<bool>(false);
            }
        }
    }
}
