using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Catel.IoC;
using Catel.MVVM;
using Catel.MVVM.Views;
using Catel.Services;
using Catel.Windows;

namespace Equality.Core.Extensions
{
    public static class NavigationServiceExtension
    {
        private readonly static object _locker = new();

        /// <summary>
        /// Navigates the specified location using the view model type in the specified view model context.
        /// </summary>
        /// <typeparam name="TViewModelType">The view model type.</typeparam>
        /// <param name="this">INavigationService.</param>
        /// <param name="viewModelContext">The view model context.</param>
        /// <param name="parameters">
        /// Dictionary of parameters, where the key is the name of the parameter, and the value is the value of the parameter.
        /// </param>
        /// <remarks>Finds first View of the specified context view model and perform navigation in it.</remarks>
        public static void Navigate<TViewModelType>(this INavigationService @this, IViewModel viewModelContext, Dictionary<string, object> parameters = null)
            where TViewModelType : IViewModel
        {
            var viewManager = @this.GetDependencyResolver().Resolve<IViewManager>();
            var view = viewManager.GetViewsOfViewModel(viewModelContext)[0];

            var d = ((DependencyObject)view).FindVisualDescendant(e => e is Frame) as Frame;

            d.Navigate(new Uri(ResolveNavigationTarget(typeof(TViewModelType)), UriKind.RelativeOrAbsolute), parameters);
            //lock (_locker) {
            //    var temp = App.Current.MainWindow;
            //    App.Current.MainWindow = (System.Windows.Window)view;

            //    @this.Navigate<TViewModelType>(parameters);

            //    App.Current.MainWindow = temp;
            //}
        }

        /// <summary>
        /// Navigates the specified location using the view model type in the specified view model context.
        /// </summary>
        /// <typeparam name="TViewModelType">The view model type.</typeparam>
        /// <typeparam name="TViewModelContextType">The view model context type.</typeparam>
        /// <param name="this">INavigationService.</param>
        /// <param name="parameters">
        /// Dictionary of parameters, where the key is the name of the parameter, and the value is the value of the parameter.
        /// </param>
        /// <remarks>Finds first View of the first context view model and perform navigation in it.</remarks>
        public static void Navigate<TViewModelType, TViewModelContextType>(this INavigationService @this, Dictionary<string, object> parameters = null)
            where TViewModelType : IViewModel
            where TViewModelContextType : IViewModel
        {
            var viewModelManager = @this.GetDependencyResolver().Resolve<IViewModelManager>();
            var viewModel = viewModelManager.GetFirstOrDefaultInstance<TViewModelContextType>();

            Navigate<TViewModelType>(@this, viewModel, parameters);
        }

        /// <summary>
        /// Resolves the navigation target.
        /// </summary>
        /// <param name="viewModelType">The view model type.</param>
        /// <returns>The target to navigate to.</returns>
        private static string ResolveNavigationTarget(Type viewModelType)
        {
            string navigationTarget = null;

            var urlLocator = ServiceLocator.Default.ResolveType<IUrlLocator>();
            navigationTarget = urlLocator.ResolveUrl(viewModelType);

            return navigationTarget;
        }
    }
}
