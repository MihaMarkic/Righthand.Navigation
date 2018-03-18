using System;

namespace Righthand.Navigation
{
    public class NavigationHistoryClearedEventArgs: EventArgs
    {
        public readonly int From;
        public readonly int Count;
        public NavigationHistoryClearedEventArgs(int from, int count)
        {
            From = from;
            Count = count;
        }
    }
}
