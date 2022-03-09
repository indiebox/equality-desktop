namespace Equality.Data.Wpf
{
    // This classes are used to setup DataTemplate for some UNIQUE elements in controls like ListBox, etc.
    // We can create this objects as many as we need. This objects are reusable in other ListBoxes, but if
    // you need more UNIQUE elements in ONE ListBox, we need to create more objects.
    // For example:
    // <ListBox>
    //     <!-- Templates for custom objects. -->
    //     <ListBox.Resources>
    //         <DataTemplate DataType="{x:Type models:Project}">
    //             ...
    //         </DataTemplate>
    //         <DataTemplate DataType="{x:Type objects:DataObject}">
    //             <ContentControl Margin="10,0,10,20"
    //                             Visibility="{Binding DataContext.CreateProjectVm, RelativeSource={RelativeSource AncestorType=equality:Page}}"
    //         </DataTemplate>
    //         <CollectionViewSource x:Key="ProjectsCollection"
    //                               Source="{Binding Projects}" />
    //     </ListBox.Resources>
    //     <ListBox.ItemsSource>
    //         <CompositeCollection>
    //             <CollectionContainer Collection="{Binding Source={StaticResource ProjectsCollection}}" />
    //             <objects:DataObject />
    //         </CompositeCollection>
    //     </ListBox.ItemsSource>
    // </ListBox>

    internal class DataObject
    {
    }
}
