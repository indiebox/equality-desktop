# Simple window example

{% hint style="warning" %}
This article assumes that you have already familiarized yourself with [the basics of MVVM](../mvvm.md) and [Catel framework](./). If not, it is highly recommended to do so.
{% endhint %}

{% hint style="info" %}
This article shows an example of MVVM components interacting using Catel.&#x20;

Behaviors, services, validation, etc. are not showed here.
{% endhint %}

After learning the basics of MVVM and Catel, let's make a small application that shows the interaction of all MVVM components:

{% code title="PhonesWindow.xaml" %}
```xml
<catel:window x:Class="MVVM.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:catel="http://schemas.catelproject.com"
        Height="350" Width="525">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
        
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*" />
            <ColumnDefinition Width="0.8*" />
        </Grid.ColumnDefinitions>
 
        <TextBlock Grid.Row="0" Text="{Binding Title}" />
 
        <ListBox Grid.Column="0" Grid.Row="1"
                 ItemsSource="{Binding Phones}"
                 SelectedItem="{Binding SelectedPhone}">
            <ListBox.ItemTemplate>
                <DataTemplate>
                    <StackPanel Margin="5">
                        <TextBlock FontSize="18" Text="{Binding Path=Title}" />
                        <TextBlock Text="{Binding Path=Company}" />
                        <TextBlock Text="{Binding Path=Price}" />
                    </StackPanel>
                </DataTemplate>
            </ListBox.ItemTemplate>
        </ListBox>
 
        <StackPanel Grid.Column="1" Grid.Row="1" DataContext="{Binding SelectedPhone}">
            <TextBlock Text="Selected item"  />
            <TextBlock Text="Model" />
            <TextBox Text="{Binding Title, UpdateSourceTrigger=PropertyChanged}" />
            <TextBlock Text="Company" />
            <TextBox Text="{Binding Company, UpdateSourceTrigger=PropertyChanged}" />
            <TextBlock Text="Price" />
            <TextBox Text="{Binding Price, UpdateSourceTrigger=PropertyChanged}" />
        </StackPanel>
        
        <Button Command="{Binding Order}" Content="Make an order" />
    </Grid>
</catel:window>
```
{% endcode %}

{% code title="PhoneModel.cs" %}
```csharp
public class Phone : ModelBase {
    public string Title { get; set; }
    
    public string Company { get; set; }
    
    public int Price { get; set; }
}
```
{% endcode %}

{% code title="PhonesWindowViewModel.cs" %}
```csharp
using System.Collections.ObjectModel;

using Equality.Core.ViewModel;

public class PhonesWindowViewModel : ViewModel
{
    public PhonesWindowViewModel() {
       Order = new TaskCommand(OnOrderExecute, () => SelectedPhone != null);
    }

    // This property is declared in the base ViewModel. 
    // It is responsible for the title of the window, but you can also 
    // perform direct binding to it, for example, to display the title
    // on the page itself.
    public override string Title => "Phones list";

    #region Properties

    public ObservableCollection<Phone> Phone { get; set; }
 
    [Model]
    [Expose("Title")]
    [Expose("Company")]
    [Expose("Price")]
    public Phone SelectedPhone { get; set; }
    
    #endregion
    
    #region Commands
    
    public TaskCommand Order { get; private set; }
    
    private Task OnOrderExecute()
    {
        // Handle order logic
    }
    
    #endregion
    
    // ...
}
```
{% endcode %}
