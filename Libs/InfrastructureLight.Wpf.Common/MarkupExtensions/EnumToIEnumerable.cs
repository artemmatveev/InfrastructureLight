using System;
using System.Windows.Markup;
using System.Linq;

using InfrastructureLight.Common.Extensions;

namespace nfrastructureLight.Wpf.Common.MarkupExtensions
{    
    public class EnumToIEnumerable : MarkupExtension
    {
        private readonly Type _type;
        public EnumToIEnumerable(Type type)
        {
            _type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_type)
                .Cast<object>()
                .Select(e => new { Value = (int)e, Description = ((Enum)e).GetDescription() });
        }
    }
}
