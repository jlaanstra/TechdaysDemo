using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Windows.UI.Xaml;

namespace TechdaysDemo.BindingConverters
{
    public class OIntStringBindingConverter : IBindingTypeConverter
    {
        public int GetAffinityForObjects(Type lhs, Type rhs)
        {
            if (lhs == typeof(bool) && rhs == typeof(Visibility))
            {
                return 100;
            }
            return -1;
        }

        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            if (toType == typeof(Visibility) && from is bool)
            {
                bool visible = (bool)from;
                result = visible ? Visibility.Visible : Visibility.Collapsed;
                return true;
            }
            result = null;
            return false;
        }
    }
}
