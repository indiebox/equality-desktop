using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Catel.IoC;
using Catel.MVVM;
using Catel.MVVM.Views;
using Catel.Services;
using Catel.Windows;

namespace Equality.Extensions
{
    public static class NavigationServiceExtension
    {
        private static readonly Dictionary<string, string> _registeredUris = new();

        /// <summary>
        /// Navigates the specified location using the view model type in the specified view model context.
        /// </summary>
        /// <typeparam name="TViewModelType">The view model type.</typeparam>
        /// <param name="this">The INavigationService.</param>
        /// <param name="viewModelContext">The view model context.</param>
        /// <param name="parameters">
        /// Dictionary of parameters, where the key is the name of the parameter, and the value is the value of the parameter.
        /// </param>
        /// <remarks>Finds first View of the specified context view model and perform navigation in it.</remarks>
        public static void Navigate<TViewModelType>(this INavigationService @this, IViewModel viewModelContext, Dictionary<string, object> parameters = null)
        {
            var view = ResolveView(viewModelContext);
            var viewContext = ((DependencyObject)view).FindVisualDescendant(e => e is Frame) as Frame;
            var viewModelType = typeof(TViewModelType);

            string fullName = viewModelType.FullName;
            lock (_registeredUris) {
                if (!_registeredUris.TryGetValue(fullName, out string uri)) {
                    uri = ResolveNavigationTarget(viewModelType);
                    _registeredUris.Add(fullName, uri);
                }

                MVVM.NavigationRootService.TemporaryNavigationRoot = viewContext;
                viewContext.Navigate(new Uri(uri, UriKind.RelativeOrAbsolute), parameters);
            }
        }

        /// <summary>
        /// Navigates the specified location using the view model type in the specified view model context.
        /// </summary>
        /// <param name="this">The INavigationService.</param>
        /// <param name="parameters">
        /// Dictionary of parameters, where the key is the name of the parameter, and the value is the value of the parameter.
        /// </param>
        /// <typeparam name="TViewModelType">The view model type.</typeparam>
        /// <typeparam name="TViewModelContextType">The view model context type.</typeparam>
        /// <remarks>Finds first View of the first context view model and perform navigation in it.</remarks>
        public static void Navigate<TViewModelType, TViewModelContextType>(this INavigationService @this, Dictionary<string, object> parameters = null)
            where TViewModelContextType : IViewModel
        {
            var viewModel = ResolveViewModel<TViewModelContextType>();

            @this.Navigate<TViewModelType>(viewModel, parameters);
        }

        /// <summary>
        /// Navigates back to the previous page.
        /// </summary>
        /// <param name="this">The INavigationService.</param>
        /// <param name="viewModelContext">The view model context.</param>
        public static void GoBack(this INavigationService @this, IViewModel viewModelContext)
        {
            if (@this.CanGoBack) {
                var view = ResolveView(viewModelContext);
                var viewContext = ((DependencyObject)view).FindVisualDescendant(e => e is Frame) as Frame;

                MVVM.NavigationRootService.TemporaryNavigationRoot = viewContext;
                viewContext.GoBack();
            }
        }

        /// <summary>
        /// Navigates back to the previous page.
        /// </summary>
        /// <param name="this">The INavigationService.</param>
        /// <typeparam name="TViewModelContextType">The view model context type.</typeparam>
        public static void GoBack<TViewModelContextType>(this INavigationService @this)
            where TViewModelContextType : IViewModel
        {
            var viewModel = ResolveViewModel<TViewModelContextType>();

            GoBack(@this, viewModel);
        }

        /// <summary>
        /// Navigates forward to the next page.
        /// </summary>
        /// <param name="this">The INavigationService.</param>
        /// <param name="viewModelContext">The view model context.</param>
        public static void GoForward(this INavigationService @this, IViewModel viewModelContext)
        {
            if (@this.CanGoForward) {
                var view = ResolveView(viewModelContext);
                var viewContext = ((DependencyObject)view).FindVisualDescendant(e => e is Frame) as Frame;

                MVVM.NavigationRootService.TemporaryNavigationRoot = viewContext;
                viewContext.GoForward();
            }
        }

        /// <summary>
        /// Navigates forward to the next page.
        /// </summary>
        /// <param name="this">The INavigationService.</param>
        /// <typeparam name="TViewModelContextType">Thew view model context type.</typeparam>
        public static void GoForward<TViewModelContextType>(this INavigationService @this)
            where TViewModelContextType : IViewModel
        {
            var viewModel = ResolveViewModel<TViewModelContextType>();

            GoForward(@this, viewModel);
        }

        /// <summary>
        /// Resolves the first instance of view model.
        /// </summary>
        /// <returns>The first view of view model.</returns>
        private static TViewModel ResolveViewModel<TViewModel>()
            where TViewModel : IViewModel
        {
            var viewModelManager = ServiceLocator.Default.ResolveType<IViewModelManager>();
            return viewModelManager.GetFirstOrDefaultInstance<TViewModel>();
        }

        /// <summary>
        /// Resolves the first view of view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>The first view of view model.</returns>
        private static IView ResolveView(IViewModel viewModel)
        {
            var viewManager = ServiceLocator.Default.ResolveType<IViewManager>();
            return viewManager.GetViewsOfViewModel(viewModel)[0];
        }

        /// <summary>
        /// Resolves the navigation target.
        /// </summary>
        /// <param name="viewModelType">The view model type.</param>
        /// <returns>The target to navigate to.</returns>
        private static string ResolveNavigationTarget(Type viewModelType)
        {
            var urlLocator = ServiceLocator.Default.ResolveType<IUrlLocator>();

            return urlLocator.ResolveUrl(viewModelType);
        }
    }
}
