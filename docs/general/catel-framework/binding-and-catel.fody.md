# Binding & Catel.Fody

## Introduction

In native MVVM, to create properties that support binding, you need to write a large amount of code. Thanks to Cate.Fody this problem disappears.

**Catel.Fody** is an addin for Fody (see [https://github.com/Fody/Fody](https://github.com/Fody/Fody)), which is an extensible tool for weaving .NET assemblies. This addin will rewrite simple properties to the dependency-property alike properties that are used inside Catel.

## Example of usage

It will rewrite all properties on the `ObservableObject`, `ModelBase` and `ViewModelBase`. So, a property that is written as this:

```csharp
public string FirstName { get; set; }
```

will be weaved into:

**ModelBase & ViewModelBase**

```csharp
public string FirstName
{
    get { return GetValue<string>(FirstNameProperty); }
    set { SetValue(FirstNameProperty, value); }
}

public static readonly PropertyData FirstNameProperty = RegisterProperty("FirstName", typeof(string));
```

**ObservableObject**

```csharp
private string _firstName;

public string FirstName
{
    get { return _firstName; }
    set {
        _firstName = value;
	RaisePropertyChanged(nameof(FirstName));
    }
}
```

**Computed properties**

If a readonly computed property like this one exists:

```csharp
public string FullName
{
    get { return string.Format("{0} {1}", FirstName, LastName).Trim(); }
}
```

the `OnPropertyChanged` method will be also weaved into:

```csharp
protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
{
    base.OnPropertyChanged(e);
    
    if (e.PropertyName.Equals("FirstName"))
    {
	base.RaisePropertyChanged("FullName");
    }

    if (e.PropertyName.Equals("LastName"))
    {
	base.RaisePropertyChanged("FullName");
    }
}
```

As you can see, **Catel.Fody** makes creating properties for binding _much_ easier.

## Get changed notifications

The Fody plugin for Catel automatically searches for the `On[PropertyName]Changed` methods. If a method is found, it will automatically be called when the property has changed. For example, the `OnNameChanged` is automatically called when the `Name` property is changed in the example below:

{% code title="LoginWindowViewModel.cs" %}
```csharp
public string Name { get; set; }
 
private void OnNameChanged()
{
    // This method is automatically called when the Name property changes
}
```
{% endcode %}

## Set default value

By default, Catel uses `null` as default values for reference types. For value types, it will use `default(T)`. To specify the default value of a weaved property, use the `DefaultValue` attribute as shown in the example below:

```csharp
public class Person : ModelBase
{
    // This property actually gets value "Default" after constructor,
    // so this way to setup define value will trigger OnLastNameChanged().
    public string LastName { get; set; } = "Default";
    
    // This way will NOT trigger OnFirstNameChanged() event, so it is
    // actual default value, not just initial setup in constructor.
    [DefaultValue("Default")]
    public string FirstName { get; set; }
}
```

## Exposing properties on view models <a href="#exposing-properties-on-view-models" id="exposing-properties-on-view-models"></a>

The way to expose properties of a model to the view model in Catel is the `ViewModelToModelAttribute`.

The goal of these attributes is to easily map properties from a model to the view model so as much of the plumbing (setting/getting properties, rechecking validation, etc) is done automatically for the developer.&#x20;

Also this can be useful, if your property is used in `OnCommandCanExecute`. This method automatically handle property changes and update _IsEnabled_ state of UI element.

Using the `ViewModelToModelAttribute`, this is the syntax to map properties automatically:

{% code title="LoginWindowViewModel.cs" %}
```csharp
public TaskCommand SomeCommand { get; private set; } = new(..., OnSomeCommandCanExecute)

[Model]
public Person Person { get; set; }

// This property listen for any changes in Person.FirstName
// and updates self value. Also, if this property is changed, it
// automatically update Person.FirstName property.
[ViewModelToModel("Person")]
public string FirstName { get; set; }

private bool OnSomeCommandCanExecute() {
    // When FirstName will become empty, the element to which 
    // this command is bound will become unavailable.
    return FirstName != null;
    
    // This WILL NOT works, because we bindings dirrectly to Person.FirstName.
    // return Person.FirstName != null;
}
```
{% endcode %}

However, this can be simplified even more if you don't need actual properties in **ViewModel**.

Instead of this, you can use `ExposeAttribute`:

{% code title="LoginWindowViewModel.cs" %}
```csharp
[Model]
[Expose("FirstName")]
[Expose("MiddleName")]
[Expose("LastName")]
public Person Person { get; set; }
```
{% endcode %}

{% code title="LoginWindow.xaml" %}
```xml
<TextBlock Text="{Binding FirstName}" />
<TextBlock Text="{Binding MiddleName}" />
<TextBlock Text="{Binding LastName}" />
```
{% endcode %}

This is a very cool feature that allows you to protect your model without having to re-define all the properties on the view model. Also, the validation in the model is automatically synchronized with the view model when you use this attribute.

## Disabling weaving <a href="#disabling-weaving-for-specific-types-or-properties" id="disabling-weaving-for-specific-types-or-properties"></a>

In some cases, we don't want all our properties to be automatically weaved for binding and property changed notifications.

To disable the weaving of types or properties of a type, decorate it with the `NoWeaving` attribute as shown in the example below:

```csharp
using Catel.Fody;

[NoWeaving]
public class MyClass : ModelBase
{
    // ...
}

// or

public class MyClass : ModelBase
{
    [NoWeaving]
    public string Email { get; set; }
}
```
