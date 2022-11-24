using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Converters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class TypeEqualsConverter : IValueConverter
    {
        public Type Type { get; }

        private TypeEqualsConverter(Type type)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return Visibility.Collapsed;
            }
            return value.GetType().IsAssignableTo(Type);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private readonly static Dictionary<Type, TypeEqualsConverter> converters = new();
        public static TypeEqualsConverter GetInstance(Type type)
        {
            if (!converters.TryGetValue(type, out TypeEqualsConverter? converter))
            {
                converter = new TypeEqualsConverter(type);
                converters.Add(type, converter);
            }
            return converter;
        }
    }

    [MarkupExtensionReturnType(typeof(TypeEqualsConverter))]
    public class TypeEqualsExtension : MarkupExtension
    {
        public Type? Type { get; set; }

        public TypeEqualsExtension(Type type)
        {
            Type = type;
        }

        public TypeEqualsExtension()
        { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return TypeEqualsConverter.GetInstance(Type ?? throw new NullReferenceException(nameof(Type)));
        }
    }
}