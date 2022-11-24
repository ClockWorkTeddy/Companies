using Companies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Companies
{
    public static class WinHelper
    {
        public static Type CompaniesType { get; } = typeof(IEnumerable<Company>);
    }

    [MarkupExtensionReturnType(typeof(Type))]
    public class CompaniesTypeExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return typeof(IEnumerable<Company>);
        }
    }
}
