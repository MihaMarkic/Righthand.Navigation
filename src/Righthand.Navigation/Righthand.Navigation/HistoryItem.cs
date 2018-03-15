namespace Righthand.Navigation
{
    public class HistoryItem<TPage>
        where TPage : IPage<TPage>
    {
        public TPage Page { get; }
        /// <summary>
        /// When page is awaited, it won't get removed automatically, but it is caller's responsibility
        /// to remove it (dispose it).
        /// </summary>
        public bool IsAwaited { get; }
        public HistoryItem(TPage page, bool isAwaited)
        {
            Page = page;
            IsAwaited = isAwaited;
        }
    }
}
