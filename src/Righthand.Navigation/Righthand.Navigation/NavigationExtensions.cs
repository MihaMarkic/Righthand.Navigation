namespace Righthand.Navigation
{
    /// <summary>
    /// Extensions.
    /// </summary>
    public static class NavigationExtensions
    {
        /// <summary>
        /// Checks if direction is backward.
        /// </summary>
        /// <param name="direction">A <see cref="NavigationDirection"/> to test.</param>
        /// <returns>True when it is backward, false otherwise.</returns>
        public  static bool IsBack(this NavigationDirection direction)
        {
            return direction == NavigationDirection.AutomaticBack || direction == NavigationDirection.ManualBack;
        }
    }
}
