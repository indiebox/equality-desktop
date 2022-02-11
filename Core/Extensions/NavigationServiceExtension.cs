using System.Collections.Generic;

using Catel.IoC;
using Catel.MVVM;
using Catel.MVVM.Views;
using Catel.Services;

namespace Equality.Core.Extensions
{
    public static class NavigationServiceExtension
    {
        /// <summary>
        /// Navigates the specified location in the specified view model context registered using the view model type.
        /// </summary>
        /// <typeparam name="TViewModelType">The view model type.</typeparam>
        /// <param name="this">INavigationService.</param>
        /// <param name="viewModelContext">The view model context.</param>
        /// <param name="parameters">
        /// Dictionary of parameters, where the key is the name of the parameter, and the value is the value of the parameter.
        /// </param>
        /// <remarks>Find first View of the specified context view model and perform navigation in it.</remarks>
        public static void NavigateInViewModelContext<TViewModelType>(this INavigationService @this, IViewModel viewModelContext, Dictionary<string, object> parameters = null)
            where TViewModelType : IViewModel
        {
            var viewManager = @this.GetDependencyResolver().Resolve<IViewManager>();
            var view = viewManager.GetViewsOfViewModel(viewModelContext)[0];

            var temp = App.Current.MainWindow;
            App.Current.MainWindow = (System.Windows.Window)view;

            @this.Navigate<TViewModelType>(parameters);

            App.Current.MainWindow = temp;
        }

        /// <summary>
        /// Navigates the specified location in the specified view model context registered using the view model type.
        /// </summary>
        /// <typeparam name="TViewModelType">The view model type.</typeparam>
        /// <typeparam name="TViewModelContextType">The view model context type.</typeparam>
        /// <param name="this">INavigationService.</param>
        /// <param name="parameters">
        /// Dictionary of parameters, where the key is the name of the parameter, and the value is the value of the parameter.
        /// </param>
        /// <remarks>Find first View of the first context view model and perform navigation in it.</remarks>
        public static void NavigateInViewModelContext<TViewModelType, TViewModelContextType>(this INavigationService @this, Dictionary<string, object> parameters = null)
            where TViewModelType : IViewModel
            where TViewModelContextType : IViewModel
        {
            var viewModelManager = @this.GetDependencyResolver().Resolve<IViewModelManager>();
            var viewModel = viewModelManager.GetFirstOrDefaultInstance<TViewModelContextType>();

            NavigateInViewModelContext<TViewModelType>(@this, viewModel, parameters);
        }
    }
}
