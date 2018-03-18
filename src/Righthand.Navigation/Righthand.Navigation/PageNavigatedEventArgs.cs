using System;

namespace Righthand.Navigation
{
    public class PageNavigatedEventArgs<TPage> : EventArgs
        where TPage : IPage<TPage>
    {
        public readonly TPage From;
        public readonly TPage To;
        public readonly NavigationDirection Direction;
        public PageNavigatedEventArgs(TPage from, TPage to, NavigationDirection direction)
        {
            From = from;
            To = to;
            Direction = direction;
        }
    }
}
