# Catel framework

{% hint style="warning" %}
This documentation implies that you have familiarized yourself with the [MVVM](../mvvm.md) and know concept and its basic principles.
{% endhint %}

Catel is an application development platform with the focus on MVVM (WPF, Universal Windows Platform, Xamarin.Android, Xamarin.iOS and Xamarin.Forms). The goal of Catel is to provide a complete set of modular functionality for Line of Business applications written in any .NET technology, from client to server.

Catel distinguishes itself by unique features to aid in the development of MVVM applications and server-side application development. Since Catel focuses on Line of Business applications, it provides professional support and excellent documentation which ensures a safe bet by professional companies and developers.

{% hint style="info" %}
Full Catel documentation can be found [here](https://docs.catelproject.com/5.x/).
{% endhint %}

## Main Catel features

Catel comes with a huge number of features and capabilities that are available "right out of the box".

List of some features:

* Data manipulation and validation
* IoC (Dependency Injection)
* **ViewModel,** **ModelBase** and **Command** classes
* Services, Behaviors, Converters, etc.
* Properties binding, automatically mappings from VM to Model properties
* and much more.

## ModelBase

The `ModelBase` class is a generic base class that can be used for all your data classes.

Some of the main parts of `ModelBase`:

* **Fully serializable** It is now really easy to store objects on disk or serialize them into memory, either binary or in XML. The data object supports this out of the box, and automatically handles the (de)serialization.
* **Support property changed notifications** The class supports the _INotifyPropertyChanging_ and _INotifyPropertyChanged_ interfaces so this class can easily be used in applications to reflect changes to the user.
* **Backwards compatibility** When serializing your objects to binary, it is hard to maintain the right versions. When you add a new property to a binary class, or change a namespace, the object cannot be loaded any longer. The data object base takes care of this issue and supports backwards compatibility.
* **Backup & revert** The class implements the _IEditableObject_ interface which makes it possible to create a state of the object. Then all properties can be edited, and finally, the changes can be applied or cancelled.

All our models are extends from this class, so they supports property changed notifications out of the box which allows you to bind them to ViewModels properties without any problems.

{% hint style="info" %}
Also there are other classes that extends **ModelBase** and add additional functionality.

You can find documentation of this classes in [Catel documentation](https://docs.catelproject.com/5.x/catel-core/data-handling/).
{% endhint %}

## ViewModelBase

{% hint style="warning" %}
All ViewModels in our application must be inherited from `Equality.Core.ViewModel`, which in turn extends from this class and adds additional features, such as [improved validation](../../equality.core/viewmodel/validation.md).
{% endhint %}

Like almost every other MVVM framework, the base class for all ViewModels is `ViewModelBase`. This base class is derived from the `ModelBase` class, which gives the following advantages:

* Dependency property a-like property registration;
* Automatic change notification;
* Support for field and business errors.

Because the class derives from `ModelBase`, you can simply add field and business errors that are automatically being reflected to the UI. Writing ViewModels has never been so easy!

### Title property

Each **ViewModelBase** contains `Title` property.

This property is responsible for the `Window.Title`. You can also bind to this property in **View**.

```csharp
public class StartPageViewModel : ViewModelBase {
    public StartPageViewModel() {
        Debug.WriteLine("1");
    } 
    
    public override string Title => "Equality";
   
    //...
}
```

### InitializeAsync & CloseAsync

Each **ViewModelBase** contains `InitializeAsync()` and `CloseAsync()` methods.

**InitializeAsync** Initializes the view model. Normally the initialization is done in the constructor, but sometimes this must be delayed to a state where the associated UI element (user control, window, etc.) is actually loaded. This method is called as soon as the associated UI element is loaded and constructed. Also good for subscribe to **ViewModel** events.

**CloseAsync** Closes this instance. Always called after the `Catel.MVVM.ViewModelBase.CancelAsync` or `Catel.MVVM.ViewModelBase.SaveAsync` method. Good for unsubscribe from **ViewModel** events.

So, in example below, the order of code execution is indicated:

{% code title="StartPageViewModel.cs" %}
```csharp
public class StartPageViewModel : ViewModelBase {
    public StartPageViewModel() {
        Debug.WriteLine("1");
    }
    
    protected override async Task InitializeAsync() {
        await base.InitializeAsync();
        
        Debug.WriteLine("3");
        
        // TODO: subscribe to events here
    }

    protected override async Task CloseAsync()
    {
        // TODO: unsubscribe from events here

        await base.CloseAsync();
    }
}
```
{% endcode %}

{% code title="StartPage.xaml.cs" %}
```csharp
public partial class AuthorizationWindow
{
    public AuthorizationWindow()
    {
        InitializeComponent();

        Debug.WriteLine("2");
    }
}
```
{% endcode %}

You need to consider this order if you have any code in the **View**.

## Resolving ViewModels and Views

Catel automatically resolves **ViewModel** class for **View** and back using locators and naming conventions.

### Naming conventions

Basically, all **Views** and **ViewModels** are located in `Views/` and `ViewModels/`.&#x20;

Main naming convention in our app:

<table><thead><tr><th>View</th><th>ViewModel</th><th data-hidden></th></tr></thead><tbody><tr><td>LoginWindow</td><td>LoginWindowViewModel</td><td></td></tr><tr><td>LoginFormControl</td><td>LoginFormControlViewModel</td><td></td></tr><tr><td>StartPage</td><td>StartPageViewModel</td><td></td></tr></tbody></table>

So if **View** called `LoginWindow`, Catel try to resolve `LoginWindowViewModel`, etc. However, Catel supports more naming conventions, as well as the ability to add your own.

{% hint style="info" %}
You can read more about naming conventions and check the default conventions [here](https://docs.catelproject.com/5.x/catel-mvvm/locators-naming-conventions/).
{% endhint %}

### Locators

Locators in Catel is responsible for resolving path or type for **View/ViewModels**.

Usually, you are unlikely to have to use them directly, but a large number of services use them.

* UrlLocator
* ViewLocator
* ViewModelLocator

#### UrlLocator

The `IUrlLocator` class is responsible for resolving the right urls for the xaml views for a view model in navigation based applications.&#x20;

The `NavigationService` internally uses the `IUrlLocator` to resolve the views.

```csharp
// Folder structure:
// ViewModels/
//    MyWindowViewModel.cs
// Views/
//    MyWindow.xaml

var urlLocator = ServiceLocator.Instance.ResolveType<IUrlLocator>();

// Returns '/Views/MyWindow.xaml'
var url = urlLocator.ResolveUrl(typeof(MyWindowViewModel));
```

{% hint style="info" %}
More info about **UrlLocator** can be found [here](https://docs.catelproject.com/5.x/catel-mvvm/locators-naming-conventions/url-locator/).
{% endhint %}

#### ViewLocator

The `IViewLocator` class is responsible for resolving the right views for a view model.

The `UIVisualizerService` internally uses the `IViewLocator` to resolve the views.&#x20;

```csharp
// Folder structure:
// ViewModels/
//    MyWindowViewModel.cs - namespace: Project.ViewModels
// Views/
//    MyWindow.xaml - namespace: Project.Views

var viewLocator = ServiceLocator.Default.ResolveType<IViewLocator>();

// Returns Project.Views.MyWindow type.
var viewType = viewLocator.ResolveView(typeof(MyWindowViewModel));
```

{% hint style="info" %}
More info about **ViewLocator** can be found [here](https://docs.catelproject.com/5.x/catel-mvvm/locators-naming-conventions/view-locator/).
{% endhint %}

#### ViewModelLocator

The `IViewModelLocator` class is responsible for resolving the right view models for a view.

This locator is used to create a view model for the start application page.

```csharp
// Folder structure:
// ViewModels/
//    MyWindowViewModel.cs - namespace: Project.ViewModels
// Views/
//    MyWindow.xaml - namespace: Project.Views

var viewModelLocator = ServiceLocator.Default.ResolveType<IViewModelLocator>();

// Returns Project.ViewModels.MyWindowViewModel type.
var viewModelType = viewModelLocator.ResolveViewModel(typeof(MyWindow));
```

{% hint style="info" %}
More info about **ViewModelLocator** can be found [here](https://docs.catelproject.com/5.x/catel-mvvm/locators-naming-conventions/view-model-locator/).
{% endhint %}

### Custom naming conventions

In some situations, you may need to create your own naming convention.

For example, if there are a lot of **Views** and **ViewModels** in the project, it will be difficult to navigate in the folder if the number of files is huge. Therefore, you may want to split **Views** and **ViewModels** into different subfolders to make it easier to navigate.

Example: Views/Authorization/... and ViewModels/Authorization/...

To work correctly, you need to add your own naming conventions.

You can register naming conventions in 3 different places:&#x20;

* IUrlLocator (for INavigationService)
* IViewLocator (for IUIVisualizerService)
* IViewModelLocator (to search for a VM for a View)

In fact, `IViewLocator` and `IViewModelLocator` use _types_ (namespaces), while `IUrlLocator` uses _path_. Therefore, if you split **ViewModels** and **Views** into subfolders, but their namespace remained untouched (`[Assembly].ViewModels` or `[Assembly].Views`), then you don't need to add your own rules to `IViewLocator` and `IViewModelLocator`.

If you are not using `INavigationService`, you actually dont need add custom naming conventions event to `IUrlLocator`.

But if we need, we should add new naming conventions in **App.xaml.cs** file:

```csharp
// Folder structure:
// ViewModels/
//    Authorization/
//        MyWindowViewModel.cs - namespace: Project.ViewModels
// Views/
//    Authorization/
//        MyWindow.xaml - namespace: Project.Views
 
var urlLocator = serviceLocator.ResolveType<IUrlLocator>();
urlLocator.NamingConventions.Add("/Views/Authorization/[VM].xaml");
```

{% hint style="info" %}
\[VM] constant here is a name of **ViewModel** without 'ViewModel' postfix.

See the list of all constants [here](https://docs.catelproject.com/5.x/catel-mvvm/locators-naming-conventions/naming-conventions/).
{% endhint %}

## Next steps

See how to create properties for binding and commands in the next articles.
