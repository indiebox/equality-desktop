# Dependency Injection

There is also a very important mechanism in Catel called **Dependency Injection (DI).**&#x20;

This is a very powerful feature that allows you to quickly replace classes if necessary, as well as create them quickly.

Below we will look at the main features of DI.

{% hint style="info" %}
More detailed documentation of DI in Catel can be found [here](https://docs.catelproject.com/5.x/catel-core/ioc/).
{% endhint %}

## Registering types

First we need to register our type so it can be resolved when we need it.

To register a new type for dependency injection we can use class **`ServiceLocator`**. We should register new types in **App.xaml.cs** file:

{% hint style="success" %}
If the one of the registered types contains **Dependency Injection** in its constructor, don't worry, they will automatically be resolved recursively!
{% endhint %}

{% hint style="info" %}
If the type is already registered, when re-registering, the previous registration will be deleted (last registered type will be actual). If you don't need it, use `RegisterTypeIfNotYetRegistered()` as shown below.
{% endhint %}

```csharp
using System.Windows;

using Catel.IoC;

using Equality.Services;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // ...

        /*
        |--------------------------------------------------------------------------
        | Register types
        |--------------------------------------------------------------------------
        |
        | Here we register custom types in the ServiceLocator for Dependency Injection.
        | 
        */
        var serviceLocator = ServiceLocator.Default;

        // Register type as Singleton
        serviceLocator.RegisterType<ISomeClass, SomeClass>(RegistrationType.Singleton);

        // Register type as Transient.
        serviceLocator.RegisterType<ISomeClass, SomeClass>(RegistrationType.Transient);

        // Register type as Singleton if not yet registered.
        serviceLocator.RegisterTypeIfNotYetRegistered<ISomeClass, SomeClass>();
    }
}
```

### Select registration type

Available registration types:

* **Singleton** Value by default. If the object has already been created, returns this object, if not, creates and returns it. One instance per application.
* **Transient** New object will be created every time.

### Register instance

In some situations we need to manually create instance of class and register it. For example, if this class has some config in constructor, that cant be defined at runtime. We can register an actual instance of type:

```csharp
using Catel.IoC;

using Equality.Services;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // ...

        /*
        |--------------------------------------------------------------------------
        | Register types
        |--------------------------------------------------------------------------
        |
        | Here we register custom types in the ServiceLocator for Dependency Injection.
        | 
        */
        var serviceLocator = ServiceLocator.Default;
        
        var serviceInstance = new SomeService(config);

        serviceLocator.RegisterInstance<ISomeService>(serviceInstance);

        // ...
    }
}
```

## Resolve types

For resolving registered types we can use one of two methods:

### Inject via constructor

Because **ViewModels** are created inside Catel, it automatically trying to resolve types in the **ViewModel** constructor. This method very simple and basically it **should be used** in all **ViewModels.**&#x20;

```csharp
using Equality.Core.ViewModel;

public class LoginViewModel : ViewModel
{ 
    protected IClassToGet ClassToGetInstance;
    
    public LoginViewModel(IClassToGet classToGetInstance)
    {
        ClassToGetInstance = classToGetInstance;
    }
}
```

### Manually resolve via DependencyResolver

In some situations can be useful resolve types outside the **ViewModel** constructor.

In this case, we need to use the `DependencyResolver`class for resolve types. Proper way to get instance of `DependencyResolver` is use `Catel.IoC` namespace and use `this.GetDependencyResolver()`:

{% hint style="warning" %}
You can also resolve types by using `ServiceLocator.Default`, but its not recommended, especially if you using custom ServiceLocator context.
{% endhint %}

{% hint style="success" %}
Namespace `Catel.IoC` needed for using extension method `GetDependencyResolver().`

This method available for all objects, so you can use this in your classes if you need.
{% endhint %}

```csharp
using Equality.Core.ViewModel;

using Catel.IoC;

public class LoginViewModel : ViewModel
{
    public void SomeMethod()
    {
        var resolver = this.GetDependencyResolver();
        var localResolvedInstance = resolver.Resolve<ISomeType>();
        
        // Or
        // ServiceLocator.Default.ResolveType<ISomeType>();
    }
}
```

## Create instance of type

In some situation we may need to create a **ViewModel** or other class with **Dependency Injection** manually, for example, to open a new window.&#x20;

In this case, we need to use the `TypeFactory` **** class for create instances. Proper way to get instance of `TypeFactory` is import `Catel.IoC` and use `this.GetTypeFactory()`:

{% hint style="warning" %}
You can also resolve types by using `TypeFactory.Default`, but its not recommended, especially if you using custom `TypeFactory` context.
{% endhint %}

{% hint style="success" %}
Namespace `Catel.IoC` needed for using extension method `GetDependencyResolver().`

This method available for all objects, so you can use this in your classes if you need.
{% endhint %}

```csharp
using Catel.IoC;

public class LoginPageViewModel : ViewModel
{
    protected IUIVisualizerService UISerivce;

    private async Task OnOpenModalWindowExecuteAsync()
    {
        var factory = this.GetTypeFactory();
        var vm = factory.CreateInstance<LoginViewModel>();
        
        await UISerivce.ShowDialogAsync(vm);
    }
}
```

Thanks to Catel, you can create and use types from anywhere, not only in **ViewModels:**

{% code title="FirstClass.cs" %}
```csharp
public class FirstClass : IFirstClass
{
    protected ISomeService Service;

    // Constructor 1
    public FirstClass(ISomeService service)
    {
        Service = service;
    }

    // Constructor 2
    public FirstClass(int val1, string val2)
    {
    }
    
    // Constructor 3
    // P.s: All custom parameters MUST go before injected parameters
    public FirstClass(int val1, string val2, ISomeService service)
    {
        Service = service;
    }
}

```
{% endcode %}

{% code title="SecondClass.cs" %}
```csharp
// We need this using for extension method .GetTypeFactory()
// This extension available for all objects, so use this where you need it.
using Catel.IoC;

public class SecondClass
{
    public SecondClass()
    {
        var factory = this.GetTypeFactory();
    
        // Сatel will create a class using the Constructor 1
        // (with dependency resolving | without parameters)
        var cls1 = factory.CreateInstance<FirstClass>();
        
        // Catel will create a class using the Constructor 2
        // (without dependency resolving | with parameters)
        var cls2 = factory.CreateInstanceWithParameters<FirstClass>(5, "test");
        
        // Catel will create a class using the Constructor 3
        // (with dependency resolving | with parameters)
        var cls3 = factory.CreateInstanceWithParametersAndAutoCompletion<FirstClass>(5, "test");
    }
}
```
{% endcode %}
