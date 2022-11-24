using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Simplified
{

    // Словарь для хранения значений, чтобы не задавать поля для свойств.
    public abstract partial class ViewModelBase
    {
        private readonly IDictionary<Type, IDictionary> types = new Dictionary<Type, IDictionary>();

        protected T? Get<T>([CallerMemberName] string? propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            Dictionary<string, T> properties;
            {
                if (!types.TryGetValue(typeof(T), out var _properties))
                {
                    return default;
                }
                else
                {
                    properties = (Dictionary<string, T>)_properties;
                }
            }

            return properties.TryGetValue(propertyName, out T? value) ? value : default;
        }

        protected bool Set<T>(in T newValue, [CallerMemberName] string? propertyName = null)
            => Set(newValue, true, propertyName);

        protected bool Set<T>(in T newValue, bool isRaise, [CallerMemberName] string? propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            T? oldValue = Get<T>(propertyName);
            bool isDifferent = !(ReferenceEquals(oldValue, newValue) || EqualityComparer<T>.Default.Equals(oldValue, newValue));
            if (isDifferent)
            {
                Dictionary<string, T> properties;
                {
                    if (!types.TryGetValue(typeof(T), out var _properties))
                    {
                        properties = new Dictionary<string, T>();
                        types.Add(typeof(T), properties);
                    }
                    else
                    {
                        properties = (Dictionary<string, T>)_properties;
                    }
                }

                if (Equals(newValue, default))
                {
                    properties.Remove(propertyName);
                }
                else
                {
                    properties[propertyName] = newValue;
                }

                if (isRaise)
                    RaisePropertyChanged(propertyName);

                OnPropertyChanged(propertyName, oldValue, newValue);
            }

            return isDifferent;
        }

    }
}