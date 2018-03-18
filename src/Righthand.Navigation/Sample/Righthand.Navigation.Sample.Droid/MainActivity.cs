using Android.App;
using Android.OS;
using Android.Widget;
using Righthand.Navigation.Sample.Droid.Fragments;
using Righthand.Navigation.Sample.ViewModels;
using System;

namespace Righthand.Navigation.Sample.Droid
{
    [Activity(Label = "Righthand.Navigation.Sample.Droid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity, IActivityHost
    {
        const string ChildFragment = nameof(ChildFragment);
        static MasterViewModel viewModel;
        TextView title;
        static MainActivity()
        {
            viewModel = new MasterViewModel();
            viewModel.Init();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            title = FindViewById<TextView>(Resource.Id.title);
        }

        void UpdateFragment(bool isBack, bool isAnimated)
        {
            if (viewModel.CurrentPage != null)
            {
                Console.WriteLine($"Preparing fragment for {viewModel.CurrentPage.GetType().Name}");
                var fragmentManager = FragmentManager;
                // check if current fragment is already display the page
                var current = fragmentManager.FindFragmentByTag(ChildFragment);
                if (current is IFragment f)
                {
                    if (ReferenceEquals(viewModel.CurrentPage, f.ViewModel))
                    {
                        Console.WriteLine("Fragment is already present");
                        return;
                    }
                }

                title.Text = viewModel.CurrentPage.Title;
                var transaction = fragmentManager.BeginTransaction();
                if (isAnimated)
                {
                    int enter;
                    int exit;
                    if (isBack)
                    {
                        exit = Resource.Animator.slide_to_right;
                        enter = Resource.Animator.slide_from_left;
                    }
                    else
                    {
                        exit = Resource.Animator.slide_to_left;
                        enter = Resource.Animator.slide_from_right;
                    }
                    transaction.SetCustomAnimations(enter, exit);
                }
                var fragment = GetPageFragment(viewModel.CurrentPage);
                transaction.Replace(Resource.Id.host, fragment, ChildFragment);
                transaction.Commit();
                fragmentManager.ExecutePendingTransactions();
                Console.WriteLine($"Fragment for {viewModel.CurrentPage.GetType().Name} created");
                Console.WriteLine($"Fragment stack is {fragmentManager.BackStackEntryCount} deep");
            }
        }
        static Fragment GetPageFragment(PageViewModel page)
        {
            switch (page)
            {
                case FirstPageViewModel firstPage:
                    return new FragmentOne();
                case SecondPageViewModel secondPage:
                    return new FragmentTwo();
                case ThirdPageViewModel thirdPage:
                    return new FragmentThree();
                default:
                    throw new ArgumentException($"Not supported page {page.GetType().Name}", nameof(page));
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            UpdateFragment(isBack: false, isAnimated: false);
            viewModel.NavigationService.PageNavigated += NavigationService_PageNavigated;
        }

        void NavigationService_PageNavigated(object sender, PageNavigatedEventArgs<PageViewModel> e)
        {
            UpdateFragment(isBack: e.Direction.IsBack(), isAnimated: true);
        }

        protected override void OnPause()
        {
            base.OnPause();
            viewModel.NavigationService.PageNavigated -= NavigationService_PageNavigated;
        }

        public PageViewModel GetViewModel() => viewModel.CurrentPage;

        public override async void OnBackPressed()
        {
            if (!await viewModel.NavigationService.GoBackAsync(isManual: false))
            {
                base.OnBackPressed();
            }
        }
    }
}

