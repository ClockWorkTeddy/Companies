namespace Simplified
{

    /// <summary>Реализация RelayCommand для методов с обобщённым параметром.</summary>
    /// <typeparam name="T">Тип параметра методов.</typeparam>
    public class RelayCommand<T> : RelayCommand
    {
        /// <summary> Command constructor. </summary>
        /// <param name = "execute"> Command method to execute. </param>
        /// <param name = "canExecute"> Method that returns the state of the command. </param>
        /// <param name="converter">Optional converter to convert <see cref="object"/> to <typeparamref name="T"/>. <br/>
        /// It is called when the parameter
        /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/is">
        /// is not compatible</see> with a <typeparamref name="T"/> type.
        /// </param>
        public RelayCommand(ExecuteHandler<T> execute, CanExecuteHandler<T>? canExecute, ConverterFromObjectHandler<T>? converter = null)
            : base
            (
                  p =>
                  {
                      if (p is T t ||
                        (converter != null && converter(p, out t)))
                      {
                          execute(t);
                      }
                  },
                  p => ((p is T t) || (converter != null && converter(p, out t))) &&
                        (canExecute?.Invoke(t) ?? true)
            )
        { }

        /// <summary> Command constructor. </summary>
        /// <param name = "execute"> Command method to execute. </param>
        /// <param name="converter">Optional converter to convert <see cref="object"/> to <typeparamref name="T"/>.</param>
        public RelayCommand(ExecuteHandler<T> execute, ConverterFromObjectHandler<T>? converter = null)
           : this(execute, null, converter)
        { }
    }
}
