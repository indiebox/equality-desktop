# Catel.MVVM

This section describes various additional elements built into Catel that can help when working with MVVM and Wpf.

## Services & Classes

Catel includes some frequently used services in applications.

The most useful of them are:

* **UIVisualizerService** Allows a developer to show (modal) windows or dialogs without actually referencing a specific view. Internally, the `UIVisualizerService` uses the `ViewLocator` to resolve views.
* **NavigationService** Allows a developer to navigate to other pages inside an application using view models only.
* **SchedulerService** Allows a developer to schedule an action in the relative or absolute future. The `SchedulerService` will use the `DispatcherTimer` to invoke the action.
* **SelectDirectoryService / OpenFileService / SaveFileService** Allows a developer to let the user choose a directory / file.
* **LanguageService** Allows a **** developer to control over the translation process in their applications. (See [https://docs.catelproject.com/5.x/catel-core/multilingual/](https://docs.catelproject.com/5.x/catel-core/multilingual/)).
* **ViewModelManager** Allows a developer to find active VM, first VM of type, VM by id, etc.
* **ViewModelFactory** Allows a developer to create VM. Actually, this is just wrapper for **TypeFactory**.
* **ViewManager** Allows a developer to find active Views, all views for VM, first View of type, etc.

{% hint style="info" %}
The full list of Catel services can be found [here](https://docs.catelproject.com/5.x/catel-mvvm/services/).
{% endhint %}

To use the any services, you need to use [Dependency Injection](dependency-injection.md) with the service interfaces:

* **UIVisualizerService - IUIVisualizerService**
* **NavigationService - INavigationService**
* etc.

As you can see, all services and interfaces follow the default naming convention, which you are also recommended to follow when creating your services.

### Resolving service example

```csharp
using Equality.Core.ViewModel;

public class LoginViewModel : ViewModel
{ 
    protected INavigationService NavigationService;
    
    public LoginViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }
}
```

## Behaviors & Triggers

Behaviors and triggers are very important to correctly separate the view from the view model. For example, to respond to an event in a view model, you cannot simply subscribe to the events in the view. The `EventToCommand` behavior is a great example to solve this problem.

Catel offers lots of behaviors out of the box, so it is definitely worth taking a look at the behaviors.

List of the most useful Behaviors and Triggers:

* `EventToCommand`
* `UpdateBindingOnPasswordChanged`
* `DelayBindingUpdate` / `UpdateBindingOnTextChange`
* `Focus` / `SelectTextOnFocus`
* `KeyPressToCommand`
* and more

{% hint style="info" %}
The full list of Catel behaviors and triggers can be found [here](https://docs.catelproject.com/5.x/catel-mvvm/behaviors-triggers/).
{% endhint %}

### EventToCommand

This is one of the most useful Triggers, as it allows you to bind a command to any UI element event.

If your command **can execute** function returns false, associated object will be disabled. If you dont need this, use `DisableAssociatedObjectOnCannotExecute="False"`.

{% hint style="warning" %}
For events like `MouseDown` and the like, which have _preview_ versions of these events (for example `PreviewMouseDown`), you need to use these versions.
{% endhint %}

```xml
<catel:Window ...
              xmlns:i="http://schemas.microsoft.com/xaml/behaviors"
              xmlns:catel="http://schemas.catelproject.com">
    <TabItem>
        <i:Interaction.Triggers>
            <i:EventTrigger EventName="PreviewMouseLeftButtonDown">
                <catel:EventToCommand Command="{Binding Logout}" />
            </i:EventTrigger>
        </i:Interaction.Triggers>
        <TabItem.Header>...</TabItem.Header>
    </TabItem>
</catel:Window>
```

### UpdateBindingOnPasswordChanged

This is necessary to bind the password from PasswordBox.

By default, it is forbidden to do this in Wpf due to security reasons. However, using this Behavior includes binding capabilities.

Also using this Behavior, the ability to [display validation errors automatically](../../equality.core/viewmodel/validation.md) is enabled, but for this you need to use another element:

```xml
<catel:Window ...
              xmlns:i="http://schemas.microsoft.com/xaml/behaviors"
              xmlns:catel="http://schemas.catelproject.com"
              xmlns:assists="clr-namespace:Equality.Assists">
    <!-- You need to use assists:PasswordBoxAssist.Password for displaying validation errors -->
    <PasswordBox assists:PasswordBoxAssist.Password="{Binding Password}">
        <i:Interaction.Behaviors>
            <catel:UpdateBindingOnPasswordChanged Password="{Binding Password, UpdateSourceTrigger=PropertyChanged}" />
        </i:Interaction.Behaviors>
    </PasswordBox>
</catel:Window>
```

### DelayBindingUpdate / UpdateBindingOnTextChange

These Behaviors are used to configure the delay in updating properties in the view model. Convenient, for example, for filtering by text.

### Focus / SelectTextOnFocus

To set the focus on a UI element, one must write code in the code-behind. With the `Focus` behavior, this is no longer necessary. This behavior sets the focus only once on the first time the associated object is loaded.

```xml
<catel:Window ...
              xmlns:i="http://schemas.microsoft.com/xaml/behaviors"
              xmlns:catel="http://schemas.catelproject.com">
    <TextBox x:Name="Email">
        <i:Interaction.Behaviors>
            <catel:Focus />
        </i:Interaction.Behaviors>
    </TextBox>
</catel:Window>
```

In some situations we need to select all text on focus, so user can easily replace this text. The `SelectTextOnFocus` behavior makes it easy to select all text when a TextBox receives the focus.

```xml
<catel:Window ...
              xmlns:i="http://schemas.microsoft.com/xaml/behaviors"
              xmlns:catel="http://schemas.catelproject.com">
    <TextBox x:Name="Email" Text="{Binding Email}"">
        <i:Interaction.Behaviors>
            <catel:SelectTextOnFocus />
        </i:Interaction.Behaviors>
    </TextBox>
</catel:Window>
```

{% hint style="success" %}
By default, `SelectTextOnFocus` will not work together with `Focus`.&#x20;

However, **Equality.MVVM** comes with `FocusAndSelectText` behavior, thanks to which you can select the entire text only once at the first focus.
{% endhint %}

### KeyPressToCommand

Sometimes you need to handle a key press and convert it to a command. An excellent example is a ListBox that should respond to an `Ctrl + Enter` key press, or TextBox for `Enter` and `Escape` keys.

```xml
<catel:Window ...
              xmlns:i="http://schemas.microsoft.com/xaml/behaviors"
              xmlns:catel="http://schemas.catelproject.com">
    <TextBox Text="{Binding Name, UpdateSourceTrigger=PropertyChanged}">
        <i:Interaction.Behaviors>
            <catel:KeyPressToCommand Command="{Binding OkCommand}"
                                     Key="Enter" />
            <catel:KeyPressToCommand Command="{Binding CloseCommand}"
                                     Key="Esc" />
        </i:Interaction.Behaviors>
    </TextBox>
<catel:Window/>
```

## Converters

In MVVM, there will be some point where you will need to use converters. Most of these converters are used in any project, so we have decided to add them to Catel.

The most commonly used converters are listed below.

{% hint style="info" %}
Full list of Catel converters can be found [here](https://github.com/Catel/Catel/tree/5.12.22/src/Catel.MVVM/MVVM/Converters).

Also don't forget that some packages (like _MaterialDesign_) can have built-in converters that are not described here, but you can also use them via `{StaticResource` `...}`
{% endhint %}

<table><thead><tr><th>Name</th><th>Description</th><th data-hidden>Example</th><th data-hidden></th></tr></thead><tbody><tr><td><mark style="color:orange;">*</mark>BooleanToCollapsingVisibilityConverter<br><mark style="color:orange;">*</mark>BooleanToHidingVisibilityConverter</td><td>Convert from bool to <em>Visibility.Collapsed</em> / <em>Visibility.Hidden</em> and back.</td><td></td><td></td></tr><tr><td><mark style="color:orange;">*</mark>EmptyStringToCollapsingVisibilityConverter<br><mark style="color:orange;">*</mark>EmptyStringToHidingVisibilityConverter</td><td>Converts a string to <em>Visibility</em>. If the string is empty, it will return <em>Visibility.Collapsed</em> / <em>Visibility.Hidden.</em></td><td></td><td></td></tr><tr><td><mark style="color:orange;">*</mark>ReferenceToCollapsingVisibilityConverter<br><mark style="color:orange;">*</mark>ReferenceToHidingVisibilityConverter</td><td>Converts a reference to <em>Visibility</em>. If the reference is <em>null</em>, it will return <em>Visibility.Collapsed / Visibility.Hidden.</em></td><td></td><td></td></tr><tr><td>ViewModelToViewConverter</td><td>Converts a view model to a view. Great way to locate a view based on a view model inside xaml.</td><td></td><td></td></tr><tr><td>AreEqualMultiValueConverter</td><td>Converts the comparison of 2 values to a boolean.<br>P.s: for this converter you must use <code>MultiBinding</code>.</td><td></td><td></td></tr><tr><td>IntToStringConverter<br>StringToIntConverter</td><td>Converts an inteteger to a string and back, or inverse.</td><td></td><td></td></tr><tr><td>MethodToValueConverter</td><td>Converts the result of a method to a value. This makes it possible to bind to a method.</td><td></td><td></td></tr></tbody></table>

<mark style="color:orange;">\*</mark> - converter supports _reverse_ conversion.

### Revert conversion

In addition, some of converters support _reverse_ conversion, to use which you need to put `True` to `ConverterParameter.`

### Examples

```xml
<!-- xmlns:catel="http://schemas.catelproject.com" -->

<!-- Border will be hidden if the line is empty, visible otherwise. -->
<Border Visibility="{Binding ErrorMessage, Converter={catel:EmptyStringToHidingVisibility}}" />

<!-- Border will be visible if the line is empty, hidden otherwise. -->
<Border Visibility="{Binding ErrorMessage, Converter={catel:EmptyStringToHidingVisibility}, ConverterParameter='True'}" />

<!-- Border will be collapsed if the HasErrors == false, visible otherwise. -->
<Border Visibility="{Binding HasError, Converter={catel:BooleanToCollapsingVisibilityConverter}" />

<!-- ViewModel from CurrentViewModel would be converted to View. -->
<ContentControl Content="{Binding CurrentViewModel, Converter={catel:ViewModelToViewConverter}}" />

<!-- Using built-in Converters -->
<Border Visibility="{Binding HasError, Converter={StaticResource BuiltInConverter}" />
```

## Hooking up everything together

See how to work with **Catel** and his features like Binding, Commands, Services, Dependency Injections, Behaviors, etc.

{% content-ref url="equality-window-example.md" %}
[equality-window-example.md](equality-window-example.md)
{% endcontent-ref %}
