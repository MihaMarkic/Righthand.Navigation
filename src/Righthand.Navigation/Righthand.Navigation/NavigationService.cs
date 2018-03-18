using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Righthand.Navigation
{
    public class NavigationService<TPage> : INavigationService<TPage>
            where TPage : IPage<TPage>
    {
        readonly Stack<HistoryItem<TPage>> history = new Stack<HistoryItem<TPage>>();
        public event EventHandler<PageNavigatedEventArgs<TPage>> PageNavigated;
        HistoryItem<TPage> current;
        protected virtual void OnPageNavigated(PageNavigatedEventArgs<TPage> e) => PageNavigated?.Invoke(this, e);
        public int NavigationDepth=> history.Count;
        public void Clear()
        {
            history.Clear();
        }
        public async ValueTask<(bool didNavigate, TNextPage Result)> NavigateAsync<TNextPage>(TNextPage to, bool waitFor, CancellationToken ct)
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
