# Commands

{% hint style="warning" %}
This article also affects asynchronous operations. If you are not familiar with them, please study them.
{% endhint %}

Commanding is supported by Catel. Catel supports Command classes, which is also known as `RelayCommand` or `DelegateCommand` in other frameworks.

Each of the command classes takes 2 main parameters: **the execution function** (required) and the function responsible for whether the command **can be executed** (optional).

Until the command is completed, the UI element to which it is bound will get the `IsEnabled="False"` property. Also, if **can be executed** function returns false, UI element will also get this property. Therefore, it is useful to add validation errors checks to these functions. If there are not validation errors, we allow the user to click on the button, thereby activating our command.

## Command types

There are 2 types of commands:

* Command
* TaskCommand

### Command

This is the base class that is used for **synchronous** operations in the same thread.

When executing such commands, the application "freezes" until this command is executed, so there should be fast operations in these commands.

### TaskCommand

This is the base class that is used for **asynchronous** operations in parallel.

Basically, it is this type of commands that should be used to open other windows, make API requests, etc.

## Example

{% hint style="info" %}
All base classes of commands are located in the namespace `Catel.MVVM`
{% endhint %}

Defining a command on a view model is very easy, as you can see in the code below:

{% code title="UserPageViewModel" %}
```csharp
using Catel.MVVM;

public class UserPageViewModel : ViewModel {

    public UserPageViewModel() {
        Edit = new Command(OnEditExecute, OnEditCanExecute);
        OpenOtherPage = new TaskCommand(OnOpenOtherPageExecute);
    }
    
    /// <summary>
    /// Gets the Edit command.
    /// </summary>
    public Command Edit { get; private set; }

    /// <summary>
    /// Method to check whether the Edit command can be executed.
    /// </summary>
    private bool OnEditCanExecute()
    {
        return !HasErrors;
    }

    /// <summary>
    /// Method to invoke when the Edit command is executed.
    /// </summary>
    private void OnEditExecute()
    {
        // Handle command logic here
    }
    
    /// <summary>
    /// Gets the OpenOtherPage command.
    /// </summary>
    public TaskCommand OpenOtherPage { get; private set; }

    /// <summary>
    /// Method to invoke when the OnOpenOtherPageExecute command is executed.
    /// </summary>
    private async Task OnOpenOtherPageExecute()
    {
        await NavigationService.GoForward();
    }
}


```
{% endcode %}

## Simple window example

See how to work with **Binding** and **Commands** in simple window example.

{% content-ref url="simple-window-example.md" %}
[simple-window-example.md](simple-window-example.md)
{% endcontent-ref %}
