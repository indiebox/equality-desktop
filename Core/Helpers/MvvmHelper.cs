using System.Windows;

using Catel.IoC;
using Catel.MVVM;
using Catel.MVVM.Views;

namespace Equality.Core.Helpers
{
    public static class MvvmHelper
    {
        private static IViewManager _viewManager => ServiceLocator.Default.ResolveType<IViewManager>();

        private static IViewModelManager _viewModelManager => ServiceLocator.Default.ResolveType<IViewModelManager>();

        /// <summary>
        /// Gets the first or default instance of the specified view model.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <returns>The Catel.MVVM.IViewModel or null if the view model is not registered.</returns>
        public static TViewModel GetFirstInstanceOfViewModel<TViewModel>()
            where TViewModel : IViewModel
        {
            return _viewModelManager.GetFirstOrDefaultInstance<TViewModel>();
        }

        /// <summary>
        /// Gets the first or default instance of the specified view type.
        /// </summary>
        /// <typeparam name="TView">Type of the view.</typeparam>
        /// <returns>The Catel.MVVM.IViewModel or null if the view model is not registered.</returns>
        public static TView GetFirstInstanceOfView<TView>()
            where TView : IView
        {
            return (TView)_viewManager.GetFirstOrDefaultInstance(typeof(TView));
        }

        /// <summary>
        /// Gets the first view of view model.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <returns>First view linked to the view model.</returns>
        public static IView GetFirstViewOfViewModel<TViewModel>()
            where TViewModel : IViewModel
        {
            return _viewManager.GetViewsOfViewModel(GetFirstInstanceOfViewModel<TViewModel>())[0];
        }

        /// <summary>
        /// Constructs the view with the specified type of view model.
        /// First, this method tries to find view type for specified view model type.
        /// Second, this method creates view model of the specified type with viewModelData.
        /// Third, this method construct view and link it to created view model.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model.</typeparam>
        /// <param name="viewModelData">The data context for view model.</param>
        /// <returns>The constructed view or null if it was not possible to construct the view.</returns>
        public static FrameworkElement CreateViewWithViewModel<TViewModel>(object viewModelData = null)
            where TViewModel : IViewModel
        {
            var vmFactory = ServiceLocator.Default.ResolveType<IViewModelFactory>();
            var viewModel = vmFactory.CreateViewModel<TViewModel>(viewModelData);

            var viewLocator = ServiceLocator.Default.ResolveType<IViewLocator>();
            var view = viewLocator.ResolveView(typeof(TViewModel));

            return ViewHelper.ConstructViewWithViewModel(view, viewModel);
        }
    }
}
