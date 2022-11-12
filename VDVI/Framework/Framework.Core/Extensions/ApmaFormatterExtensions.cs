using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core.ApmaExtensions
{
    public static class ApmaFormatterExtensions
    {
        public static List<T> ApmaFormatList<T, TValue>(this List<T> list, Expression<Func<T, TValue>> memberLamda, TValue value)
        {
            list.ForEach(item => item.SetPropertyValue(memberLamda, value));
            return list;
        }

        public static void SetPropertyValue<T, TValue>(this T target, Expression<Func<T, TValue>> memberLamda, TValue value)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(target, value, null);
                }
            }
        }
    }
}
