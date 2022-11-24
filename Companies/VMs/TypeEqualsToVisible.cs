using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Companies.Models;

namespace Companies.VMs
{
    public class TypeEqualsToVisibleConverter : IValueConverter
    {
        public Type? Type { get; }

        public TypeEqualsToVisibleConverter(Type? type)
        {
            Type = type;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return Visibility.Collapsed;
            }
            return value.GetType().IsAssignableTo(Type) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class TypeEqualsToVisible : MarkupExtension
    {
        public Type? Type { get; set; }

        public TypeEqualsToVisible(Type? type)
        {
            Type = type;
        }

        public TypeEqualsToVisible()
        { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new TypeEqualsToVisibleConverter(Type);
        }
    }
}
