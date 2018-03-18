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
            bool didNavigate = await NavigateAsync(to, isBack: false, isAwaited: waitFor);
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
                if (e.IsBack && ReferenceEquals(e.To, page))
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
        async ValueTask<bool> NavigateAsync(TPage to, bool isBack, bool isAwaited)
        {
            bool canNavigate;
            if (current == null || isBack)
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
                if (!isBack)
                {
                    if (current != null)
                    {
                        history.Push(current);
                    }
                    current = new HistoryItem<TPage>(to, isAwaited);
                    var from = previousCurrent != null ? previousCurrent.Page : default(TPage);
                    OnPageNavigated(new PageNavigatedEventArgs<TPage>(from, to, isBack));
                }
                else
                {
                    var historyItem = history.Pop();
                    OnPageNavigated(new PageNavigatedEventArgs<TPage>(current.Page, historyItem.Page, isBack));
                    if (!current.IsAwaited)
                    {
                        current.Page.Removed();
                    }
                    current = historyItem;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public ValueTask<bool> GoBackAsync(object args = null)
        {
            if (history.Count > 0)
            {
                var previous = history.Peek();
                // isAwaited isn't used here - only when navigating forward
                return NavigateAsync(previous.Page, isBack: true, isAwaited: false);
            }
            else
            {
                return new ValueTask<bool>(false);
            }
        }
    }
}
