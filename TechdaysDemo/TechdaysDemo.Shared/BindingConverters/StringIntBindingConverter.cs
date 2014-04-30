using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace TechdaysDemo.BindingConverters
{
    public class StringIntBindingConverter : IBindingTypeConverter
    {
        public int GetAffinityForObjects(Type lhs, Type rhs)
        {
            if (lhs == typeof(string) && rhs == typeof(int))
            {
                return 100;
            }
            return -1;
        }

        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            if (toType == typeof(int) && from is string)
            {
                string str = (string)from;
                int intResult;
                if(int.TryParse(str, out intResult))
                {
                    result = intResult;
                    return true;
                }
                result = null;
                return false;
            }
            result = null;
            return false;
        }
    }
}
