using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core.Extensions
{
    public static class FormatterExtensions
    {
        public static List<T> FormatList<T, TValue>(this List<T> list, Expression<Func<T, TValue>> memberLamda, TValue value)
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
