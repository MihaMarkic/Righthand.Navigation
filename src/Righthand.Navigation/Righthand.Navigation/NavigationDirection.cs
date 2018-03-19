namespace Righthand.Navigation
{
    /// <summary>
    /// Navigation directions.
    /// </summary>
    /// <remarks>
    /// There are two Back values because in iOS there is no way to intercept back navigation and it has 
    /// to be handled a bit differently. See iOS sample.
    /// </remarks>
    public enum NavigationDirection
    {
        /// <summary>
        /// Forward.
        /// </summary>
        Forward,
        /// <summary>
        /// Back navigation invoked implicitly (i.e. from OS).
        /// </summary>
        AutomaticBack,
        /// <summary>
        /// Back navigation invoked explicitly.
        /// </summary>
        ManualBack
    }
}
