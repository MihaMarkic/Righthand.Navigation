namespace Righthand.Navigation
{
    /// <summary>
    /// Represents a point in history stack.
    /// </summary>
    /// <typeparam name="TPage">Type representing page.</typeparam>
    public class HistoryItem<TPage>
        where TPage : IPage<TPage>
    {
        /// <summary>
        /// Connected page to this item.
        /// </summary>
        public TPage Page { get; }
        /// <summary>
        /// When page is awaited, it won't get removed automatically, but it is caller's responsibility
        /// to remove it (dispose it).
        /// </summary>
        public bool IsAwaited { get; }
        /// <summary>
        /// Initializes a new instance of the HistoryItem&lt;TPage&gt; class.
        /// </summary>
        /// <param name="page">An instance of page.</param>
        /// <param name="isAwaited">Flag signaling whether page is awaited for.</param>
        /// <remarks>When page is awaited for it's <see cref="IPage{T}.Removed"/> method won't be called when popping from stack.</remarks>
        public HistoryItem(TPage page, bool isAwaited)
        {
            Page = page;
            IsAwaited = isAwaited;
        }
    }
}
