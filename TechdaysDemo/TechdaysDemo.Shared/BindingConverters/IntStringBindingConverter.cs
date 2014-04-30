using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace TechdaysDemo.BindingConverters
{
    public class IntStringBindingConverter : IBindingTypeConverter
    {
        public int GetAffinityForObjects(Type lhs, Type rhs)
        {
            if (lhs == typeof(int) && rhs == typeof(string))
            {
                return 100;
            }
            return -1;
        }

        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            if (toType == typeof(string) && from is int)
            {
                int number = (int)from;
                result = number.ToString();
                return true;
            }
            result = null;
            return false;
        }
    }
}
