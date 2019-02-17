using InfrastructureLight.Common.Extensions;
using System;
using System.Linq;
using System.Windows.Markup;

namespace InfrastructureLight.Wpf.Common.MarkupExtensions
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
