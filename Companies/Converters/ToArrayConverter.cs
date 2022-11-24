using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Converters
{
    [ValueConversion(typeof(object), typeof(Array))]
    public class ToArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Array.Empty<object>();
            var array = Array.CreateInstance(value.GetType(), 1);
            array.SetValue(value, 0);
            return array; ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private ToArrayConverter() { }
        public static ToArrayConverter Instance { get; } = new ToArrayConverter();
    }

    [MarkupExtensionReturnType(typeof(ToArrayConverter))]
    public class ToArrayExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return ToArrayConverter.Instance;
        }
    }
}