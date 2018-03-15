using System;

namespace Righthand.Navigation
{
    public class PageNavigatedEventArgs<TPage> : EventArgs
        where TPage : IPage<TPage>
    {
        public readonly TPage From;
        public readonly TPage To;
        public readonly bool IsBack;
        public PageNavigatedEventArgs(TPage from, TPage to, bool isBack)
        {
            From = from;
            To = to;
            IsBack = isBack;
        }
    }
}
