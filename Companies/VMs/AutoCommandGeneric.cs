using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Companies.VMs
{
    public delegate void ExecuteHandler<T>(T parameter);
    public delegate bool CanExecuteHandler<T>(T parameter);

    public class AutoCommand<T> : AutoCommand
    {
        public AutoCommand(ExecuteHandler<T> execute, CanExecuteHandler<T> canExecute = null)
            : base
            (
                  p =>
                  {
                      if (p is T t)
                          execute(t);
                  },
                  p => (p is T t) && (canExecute?.Invoke(t) ?? true)
            )
        { }
    }
}
