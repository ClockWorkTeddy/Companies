using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace Behaviors
{
    /// <summary>Для создания привязки от любого <see cref="DependencyProperty"/>,
    /// в том числе "только для чтения".</summary>
    public class BindingReadOnlyProperty : Behavior<DependencyObject>
    {

        private bool isSeal;
        private DependencyProperty? _property;
        private BindingBase? _sourceBinding;
        public BindingBase? SourceBinding
        {
            get => _sourceBinding;
            set
            {
                if (isSeal)
                    throw new InvalidOperationException("Запечатан!");
                _sourceBinding = value;
                if (value is null)
                {
                    BindingOperations.ClearBinding(this, SourceProperty);
                }
                else
                {
                    BindingOperations.SetBinding(this, SourceProperty, value);
                }
            }
        }


        public DependencyProperty? Property
        {
            get => _property;
            set
            {
                if (isSeal)
                    throw new InvalidOperationException("Запечатан!");
                _property = value;
            }
        }

        /// <summary><see cref="DependencyProperty"/> для свойства <see cref="Source"/>.</summary>
        private static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(object), typeof(BindingReadOnlyProperty), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            isSeal = true;
            if (Property is null)
                throw new NullReferenceException(nameof(Property));

            base.OnAttached();

            DependencyPropertyDescriptor
                .FromProperty(Property, AssociatedObject.GetType())
                .AddValueChanged(AssociatedObject, OnPropertyValueChanged);
            OnPropertyValueChanged(AssociatedObject, EventArgs.Empty);
        }

        private void OnPropertyValueChanged(object? sender, EventArgs e)
        {
            SetValue(SourceProperty, AssociatedObject.GetValue(Property));
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            DependencyPropertyDescriptor
                .FromProperty(Property, AssociatedObject.GetType())
                .RemoveValueChanged(AssociatedObject, OnPropertyValueChanged);
        }

    }
}
