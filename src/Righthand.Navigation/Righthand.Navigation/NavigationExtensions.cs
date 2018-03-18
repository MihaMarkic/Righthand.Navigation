namespace Righthand.Navigation
{
    public static class NavigationExtensions
    {
        public  static bool IsBack(this NavigationDirection direction)
        {
            return direction == NavigationDirection.AutomaticBack || direction == NavigationDirection.ManualBack;
        }
    }
}
