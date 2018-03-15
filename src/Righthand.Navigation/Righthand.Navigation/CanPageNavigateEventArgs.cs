using System;
using System.Threading.Tasks;

namespace Righthand.Navigation
{
    public class CanPageNavigateEventArgs<TPage> : EventArgs
    {
        readonly TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        public readonly TPage From;
        public readonly TPage To;
        public readonly object Args;
        public readonly bool IsBack;
        public Task<bool> IsAllowed => tcs.Task;
        public CanPageNavigateEventArgs(TPage from, TPage to, object args, bool isBack)
        {
            From = from;
            To = to;
            Args = args;
            IsBack = isBack;
        }
        public void SetResult(bool canNavigate)
        {
            tcs.SetResult(canNavigate);
        }
    }
}
