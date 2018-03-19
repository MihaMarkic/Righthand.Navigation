using System;

namespace Righthand.Navigation
{
    /// <summary>
    /// Provides data for <see cref="INavigationService{TPage}.PageNavigated"/> event.
    /// </summary>
    /// <typeparam name="TPage">Type of page.</typeparam>
    public class PageNavigatedEventArgs<TPage> : EventArgs
        where TPage : IPage<TPage>
    {
        /// <summary>
        /// Navigation origin.
        /// </summary>
        public readonly TPage From;
        /// <summary>
        /// Navigation destination.
        /// </summary>
        public readonly TPage To;
        /// <summary>
        /// Navigation direction.
        /// </summary>
        public readonly NavigationDirection Direction;
        /// <summary>
        /// Initializes an instance of PageNavigatedEventArgs&lt;TPage&gt; class.
        /// </summary>
        /// <param name="from">Navigation origin.</param>
        /// <param name="to">Navigation destination.</param>
        /// <param name="direction">Navigation direction.</param>
        public PageNavigatedEventArgs(TPage from, TPage to, NavigationDirection direction)
        {
            From = from;
            To = to;
            Direction = direction;
        }
    }
}
