namespace SR5Builder.Helpers
{
    using DataModels;
    using ExpressionEvaluator;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using System.Reflection;


    public static class ExpressionUtils
    {
        public static Dictionary<INotifyPropertyChanged, HashSet<string>> FindMembers<TScope, TResult>(CompiledExpression<TResult> ce, TScope scope)
        {
            var set = findMembersTraverse(ce.Expression, scope);
            var dict = new Dictionary<INotifyPropertyChanged, HashSet<string>>();

            foreach (var pair in set)
            {
                if (dict.ContainsKey(pair.Item1))
                {
                    dict[pair.Item1].Add(pair.Item2);
                }
                else
                {
                    var subset = new HashSet<string>();
                    subset.Add(pair.Item2);
                    dict.Add(pair.Item1, subset);
                }
            }

            return dict;
        }

        private static HashSet<Tuple<INotifyPropertyChanged, string>> findMembersTraverse<TScope>(
            Expression ex, TScope scope)
        {

            if (ex is MemberExpression)
            {
                var me = ex as MemberExpression;
                string memberName = null;
                INotifyPropertyChanged container = null;

                if (me.Member.MemberType == MemberTypes.Property)
                {
                    var pi = me.Member as PropertyInfo;
                    if (typeof(LeveledTrait).IsAssignableFrom(pi.PropertyType))
                    {
                        var outerContainer = EvaluateContainer(me.Expression, scope);
                        container = pi.GetValue(outerContainer) as INotifyPropertyChanged;
                        memberName = "AugmentedRating";
                    }
                    else
                    {
                        memberName = me.Member.Name;
                        container = EvaluateContainer(me.Expression, scope);
                    }
                }

                if (container != null)
                {
                    return new HashSet<Tuple<INotifyPropertyChanged, string>>(
                        new Tuple<INotifyPropertyChanged, string>[]
                            { Tuple.Create(container, memberName) }
                        );
                }
            }
            else if (ex is BinaryExpression)
            {
                var be = ex as BinaryExpression;
                var left = findMembersTraverse(be.Left, scope);
                var right = findMembersTraverse(be.Right, scope);
                left.UnionWith(right);
                return left;
            }

            return new HashSet<Tuple<INotifyPropertyChanged, string>>();
        }

        static INotifyPropertyChanged EvaluateContainer<TScope>(Expression ex, TScope scope)
        {
            var scopeParam = GetParameterExpression(ex);
            try
            {
                var lambda = Expression.Lambda<Func<TScope, INotifyPropertyChanged>>(ex, new ParameterExpression[] { scopeParam });
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
            else return null;
        }
    }
}
