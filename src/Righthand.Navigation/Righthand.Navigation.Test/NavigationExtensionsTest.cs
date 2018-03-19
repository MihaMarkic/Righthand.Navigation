using NUnit.Framework;

namespace Righthand.Navigation.Test
{
    public class NavigationExtensionsTest
    {
        [TestFixture]
        public class IsBack: NavigationExtensionsTest
        {
            [TestCase(NavigationDirection.Forward, ExpectedResult = false)]
            [TestCase(NavigationDirection.AutomaticBack, ExpectedResult = true)]
            [TestCase(NavigationDirection.ManualBack, ExpectedResult = true)]
            public bool WhenGivenValue_ReturnsExpected(NavigationDirection direction)
            {
                return direction.IsBack();
            }
        }
    }
}
