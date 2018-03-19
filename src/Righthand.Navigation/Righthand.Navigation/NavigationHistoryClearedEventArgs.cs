using System;

namespace Righthand.Navigation
{
    /// <summary>
    /// Provides data for <see cref="INavigationService{TPage}.NavigationHistoryCleared"/> event.
    /// </summary>
    public class NavigationHistoryClearedEventArgs: EventArgs
    {
        /// <summary>
        /// First index.
        /// </summary>
        public readonly int From;
        /// <summary>
        /// Number of items removed.
        /// </summary>
        public readonly int Count;
        /// <summary>
        /// Initializes an instance of NavigationHistoryClearedEventArgs class.
        /// </summary>
        /// <param name="from">First index.</param>
        /// <param name="count">Number of items.</param>
        public NavigationHistoryClearedEventArgs(int from, int count)
        {
            From = from;
            Count = count;
        }
    }
}
