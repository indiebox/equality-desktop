using System.Runtime.InteropServices;
using System.Windows.Markup;

// В проектах SDK, таких как этот, некоторые атрибуты сборки, которые ранее определялись
// в этом файле, теперь автоматически добавляются во время сборки и заполняются значениями,
// заданными в свойствах проекта. Подробные сведения о том, какие атрибуты включены
// и как настроить этот процесс, см. на странице: https://aka.ms/assembly-info-properties.


// При установке значения false для параметра ComVisible типы в этой сборке становятся
// невидимыми для компонентов COM. Если вам необходимо получить доступ к типу в этой
// сборке из модели COM, установите значение true для атрибута ComVisible этого типа.

[assembly: ComVisible(false)]

// Следующий GUID служит для идентификации библиотеки типов typelib, если этот проект
// будет видимым для COM.

[assembly: Guid("2547cc14-c6d6-4903-a2e5-6b3ef2852101")]

[assembly: XmlnsPrefix("http://equality.indiebox.ru/schemas", "equality")]
[assembly: XmlnsDefinition("http://equality.indiebox.ru/schemas", "Equality.Converters")]
[assembly: XmlnsDefinition("http://equality.indiebox.ru/schemas", "Equality.Behaviors")] 