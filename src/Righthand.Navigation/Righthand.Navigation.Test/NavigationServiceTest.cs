using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Righthand.Navigation.Test
{
    public class NavigationServiceTest
    {
        protected NavigationService<TestViewModel> target;
        [SetUp]
        public void SetUp()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            target = new NavigationService<TestViewModel>();
        }
        [TestFixture]
        public class ClearHistory: NavigationServiceTest
        {
            [Test]
            public void WhenNoHistory_NoNavigationHistoryClearedEventIsRisen()
            {
                bool wasCalled = false;
                target.NavigationHistoryCleared += (s, e) => wasCalled = true;

                target.ClearHistory();

                Assert.That(wasCalled, Is.False);
            }
            [Test]
            public async Task WhenSingleItemOnStack_NavigationHistoryClearedEventIsRisen()
            {
                bool wasCalled = false;
                target.NavigationHistoryCleared += (s, e) => wasCalled = true;
                // after running twice, navigation depth is 1
                for (int i = 0; i < 2; i++)
                {
                    await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);
                }

                target.ClearHistory();

                Assert.That(wasCalled, Is.True);
            }
            [Test]
            public async Task WhenSingleItemOnStack_NavigationHistoryClearedEventArgumentsAreCorrect()
            {
                NavigationHistoryClearedEventArgs args = null;
                target.NavigationHistoryCleared += (s, e) => args = e;
                // after running twice, navigation depth is 1
                for (int i = 0; i < 2; i++)
                {
                    await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);
                }

                target.ClearHistory();

                Assert.That(args.From, Is.Zero);
                Assert.That(args.Count, Is.EqualTo(1));
            }
            [Test]
            public async Task WhenSingleItemOnStack_PageRemovedIsCalledOnlyOnIt()
            {
                bool wasRemovedCalledOnFirst = false;
                bool wasRemovedCalledOnSecond = false;
                var first = new TestViewModel { WasRemoved = () => wasRemovedCalledOnFirst = true };
                var second = new TestViewModel { WasRemoved = () => wasRemovedCalledOnSecond = true };
                // after running twice, navigation depth is 1
                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);
                await target.NavigateAsync(second, waitFor: false, ct: CancellationToken.None);

                target.ClearHistory();

                Assert.That(wasRemovedCalledOnFirst, Is.True);
                Assert.That(wasRemovedCalledOnSecond, Is.False);
            }
        }
        [TestFixture]
        public class NavigateAsync: NavigationServiceTest
        {
            [Test]
            public async Task WhenNavigatingForwardFirstTime_NoExceptionIsThrown()
            {
                await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);
            }
            [Test]
            public async Task WhenNavigatingForward_CanNavigateIsCalled()
            {
                bool wasCalled = false;
                var first = new TestViewModel
                {
                    CanNavigateTo = (vm) =>
                    {
                        wasCalled = true;
                        return true;
                    }
                };
                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);

                await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);

                Assert.That(wasCalled, Is.True);
            }
            [Test]
            public async Task WhenNavigatingForwardAndCanNavigateIsFalse_DidNavigateIsFalse()
            {
                var first = new TestViewModel
                {
                    CanNavigateTo = (vm) => false
                };
                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);

                var actual = await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);

                Assert.That(actual.DidNavigate, Is.False);
            }
            [Test]
            public async Task WhenNavigatingForwardAndCanNavigateIsFalse_NavigationDepthIsZero()
            {
                var first = new TestViewModel
                {
                    CanNavigateTo = (vm) => false
                };
                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);

                await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);

                Assert.That(target.NavigationDepth, Is.Zero);
            }
            [Test]
            public async Task WhenNavigatingForwardAndCanNavigateIsFalse_PageNavigatedIsNotRisen()
            {
                bool wasCalled = false;
                var first = new TestViewModel
                {
                    CanNavigateTo = (vm) => false
                };
                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);
                target.PageNavigated += (s, e) => wasCalled = true;

                await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);

                Assert.That(wasCalled, Is.False);
            }
            [Test]
            public async Task WhenNavigatingForwardAndCanNavigateIsTrue_DidNavigateIsTrue()
            {
                var first = new TestViewModel
                {
                    CanNavigateTo = (vm) => true
                };
                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);

                var actual = await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);

                Assert.That(actual.DidNavigate, Is.True);
            }
            [Test]
            public async Task WhenNavigatingForward_PageNavigatedIsRisen()
            {
                bool wasCalled = false;
                target.PageNavigated += (s, e) => wasCalled = true;
                await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);

                Assert.That(wasCalled, Is.True);
            }
            [Test]
            public async Task WhenNavigatingForward_PageNavigatedEventHasCorrectDirection()
            {
                PageNavigatedEventArgs<TestViewModel> args = null;
                target.PageNavigated += (s, e) => args = e;
                var first = new TestViewModel();

                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);

                Assert.That(args.Direction, Is.EqualTo(NavigationDirection.Forward));
            }
            [Test]
            public async Task WhenNavigatingForward_PageNavigatedEventHasCorrectTo()
            {
                PageNavigatedEventArgs<TestViewModel> args = null;
                target.PageNavigated += (s, e) => args = e;
                var first = new TestViewModel();

                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);

                Assert.That(args.To, Is.SameAs(first));
            }
            [Test]
            public async Task WhenWaitForIsTrue_ResultIsCorrect()
            {
                var first = new TestViewModel();
                var second = new TestViewModel();
                var ignore = Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(100);
                    await target.GoBackAsync(isManual: true);
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);

                var actual = await target.NavigateAsync(second, waitFor: true, ct: CancellationToken.None);

                Assert.That(actual.Result, Is.SameAs(second));
            }
            [Test]
            public async Task WhenWaitForIsTrue_WaitsForNavigationBack()
            {
                int step = 0;
                var first = new TestViewModel();
                var second = new TestViewModel();
                var ignore = Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(100);
                    step = 2;
                    await target.GoBackAsync(isManual: true);
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);
                step = 1;

                await target.NavigateAsync(second, waitFor: true, ct: CancellationToken.None);

                Assert.That(step, Is.EqualTo(2));
            }
        }
        [TestFixture]
        public class NavigationDepth: NavigationServiceTest
        {
            [Test]
            public void IsZeroByDefault()
            {
                var actual = target.NavigationDepth;

                Assert.That(actual, Is.Zero);
            }
            [Test]
            public async Task WhenOneNavigationForward_IsZero()
            {
                await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);
                var actual = target.NavigationDepth;

                Assert.That(actual, Is.Zero);
            }
            [Test]
            public async Task WhenTwoNavigationsForward_IsOne()
            {
                for (int i = 0; i < 2; i++)
                {
                    await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);
                }

                var actual = target.NavigationDepth;

                Assert.That(actual, Is.EqualTo(1));
            }
            [Test]
            public async Task WhenTwoNavigationsForwardAndOneManualBack_IsZero()
            {
                for (int i = 0; i < 2; i++)
                {
                    await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);
                }
                await target.GoBackAsync(isManual: true);

                var actual = target.NavigationDepth;

                Assert.That(actual, Is.Zero);
            }
            [Test]
            public async Task WhenTwoNavigationsForwardAndOneAutomaticBack_IsZero()
            {
                for (int i = 0; i < 2; i++)
                {
                    await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);
                }
                await target.GoBackAsync(isManual: false);

                var actual = target.NavigationDepth;

                Assert.That(actual, Is.Zero);
            }
        }
        [TestFixture]
        public class GoBackAsync : NavigationServiceTest
        {
            [TestCase(false, ExpectedResult = NavigationDirection.AutomaticBack)]
            [TestCase(true, ExpectedResult = NavigationDirection.ManualBack)]
            public async Task<NavigationDirection> WhenNavigatingBack_PageNavigatedEventHasCorrectDirection(bool isManual)
            {
                await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);
                await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);
                PageNavigatedEventArgs<TestViewModel> args = null;
                target.PageNavigated += (s, e) => args = e;
                await target.GoBackAsync(isManual);

                return args.Direction;
            }
            [TestCase(false)]
            [TestCase(true)]
            public async Task WhenNavigatingBack_PageNavigatedEventHasCorrectTo(bool isManual)
            {
                var first = new TestViewModel();
                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);
                await target.NavigateAsync(new TestViewModel(), waitFor: false, ct: CancellationToken.None);
                PageNavigatedEventArgs<TestViewModel> args = null;
                target.PageNavigated += (s, e) => args = e;

                await target.GoBackAsync(isManual);

                Assert.That(args.To, Is.SameAs(first));
            }
            [TestCase(false)]
            [TestCase(true)]
            public async Task WhenNavigatingBackAndPageIsNotAwaited_PageRemoveIsCalled(bool isManual)
            {
                bool wasCalled = false;
                var first = new TestViewModel();
                var second = new TestViewModel { WasRemoved = () => wasCalled = true };
                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);
                await target.NavigateAsync(second, waitFor: false, ct: CancellationToken.None);

                await target.GoBackAsync(isManual);

                Assert.That(wasCalled, Is.True);
            }
            [TestCase(false)]
            [TestCase(true)]
            public async Task WhenNavigatingBackAndPageIsAwaited_PageRemoveIsNotCalled(bool isManual)
            {
                bool wasCalled = false;
                var first = new TestViewModel();
                var second = new TestViewModel { WasRemoved = () => wasCalled = true };
                await target.NavigateAsync(first, waitFor: false, ct: CancellationToken.None);
                var ignore = Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(100);
                    await target.GoBackAsync(isManual: true);
                }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
                await target.NavigateAsync(second, waitFor: true, ct: CancellationToken.None);

                await target.GoBackAsync(isManual);

                Assert.That(wasCalled, Is.False);
            }
        }
    }

    public class TestViewModel : IPage<TestViewModel>
    {
        public Action WasRemoved { get; set; }
        public Func<TestViewModel, bool> CanNavigateTo { get; set; }
        public Action<TestViewModel, NavigationDirection> DidNavigate { get; set; }
        public ValueTask<bool> CanNavigate(TestViewModel to)
        {
            bool result = CanNavigateTo != null ? CanNavigateTo(to): true;
            return new ValueTask<bool>(result);
        }

        public void Navigated(TestViewModel from, NavigationDirection direction)
        {
            DidNavigate?.Invoke(from, direction);
        }

        public void Removed()
        {
            WasRemoved?.Invoke();
        }
    }
}
