using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SR5Builder.Helpers
{
    public static class ExpressionUtilities
    {
        public static Dictionary<string, HashSet<string>> FindMembers<TScope, TResult>(Expression<Func<TScope, TResult>> ex, TScope scope)
        {
            var set = FindMembersTraverse(ex, scope);
            var dict = new Dictionary<string, HashSet<string>>();
            foreach (KeyValuePair<string, string> kvp in set)
            {
                if (dict.ContainsKey(kvp.Key))
                {
                    dict[kvp.Key].Add(kvp.Value);
                }
                else
                {
                    dict.Add(kvp.Key, new HashSet<string>(new string[] { kvp.Value }));
                }
            }
            return dict;
        }

        private static HashSet<KeyValuePair<string, string>> FindMembersTraverse<TScope>(
            Expression ex, TScope scope)
        {

            if (ex is MemberExpression)
            {
                var me = ex as MemberExpression;
                string memberName = null;
                string container = null;

                if (me.Member.MemberType == MemberTypes.Property)
                {
                    var pi = me.Member as PropertyInfo;
                    memberName = me.Member.Name;
                    container = EvaluateContainer(me.Expression, scope);
                }

                if (container != null)
                {
                    return new HashSet<KeyValuePair<string, string>>(
                        new KeyValuePair < string, string>[] {
                            new KeyValuePair<string, string>(container, memberName)
                        });
                }
            }
            else if (ex is BinaryExpression)
            {
                var be = ex as BinaryExpression;
                var left = FindMembersTraverse(be.Left, scope);
                var right = FindMembersTraverse(be.Right, scope);
                left.UnionWith(right);
                return left;
            }
            else if (ex is UnaryExpression)
            {
                var ue = ex as UnaryExpression;
                return FindMembersTraverse(ue.Operand, scope);
            }
            return new HashSet<KeyValuePair<string, string>>();
        }

        static string EvaluateContainer<TScope>(Expression ex, TScope scope)
        {
            var scopeParam = GetParameterExpression(ex);
            try
            {
                var lambda = Expression.Lambda<Func<TScope, string>>(
                    WrapExpression<string>(ex),
                    new ParameterExpression[] { scopeParam });
                var del = lambda.Compile();
                return del(scope);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        static object EvaluateMember<TScope>(Expression ex, TScope scope)
        {
            var scopeParam = GetParameterExpression(ex);
            try
            {
                var lambda = Expression.Lambda<Func<TScope, object>>(
                    WrapExpression<object>(ex),
                    new ParameterExpression[] { scopeParam });
                var del = lambda.Compile();
                return del(scope);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        static ParameterExpression GetParameterExpression(Expression ex)
        {
            if (ex is MemberExpression)
            {
                var me = ex as MemberExpression;
                return GetParameterExpression(me.Expression);
            }
            else if (ex is BinaryExpression)
            {
                var be = ex as BinaryExpression;
                var left = GetParameterExpression(be.Left);
                var right = GetParameterExpression(be.Right);
                return left ?? right;
            }
            else if (ex is ParameterExpression)
            {
                return ex as ParameterExpression;
            }
            else if (ex is DynamicExpression)
            {
                var de = ex as DynamicExpression;

                foreach (var arg in de.Arguments)
                {
                    var param = GetParameterExpression(arg);
                    if (param != null) return param;
                }
            }
            return null;
        }

        static Expression WrapExpression<TResult>(Expression ex)
        {
            if (!typeof(TResult).IsAssignableFrom(ex.Type))
            {
                return Expression.Convert(ex, typeof(TResult));
            }
            return ex;
        }
    }
}
