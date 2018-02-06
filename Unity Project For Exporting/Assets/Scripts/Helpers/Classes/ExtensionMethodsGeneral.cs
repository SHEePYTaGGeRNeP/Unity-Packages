using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if (!UNITY_WINRT)
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
#endif
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
public static class ExtensionMethodsGeneral
{
    private static readonly System.Random _random = new System.Random();

    /// <summary>
    /// If the given <paramref name="type"/> is an array or some other collection
    /// comprised of 0 or more instances of a "subtype", get that type
    /// </summary>
    /// <param name="type">the source type</param>
    /// <returns></returns>
    public static Type GetEnumeratedType(this Type type)
    {
        // provided by Array
        var elType = type.GetElementType();
        if (null != elType) return elType;

        // otherwise provided by collection
        var elTypes = type.GetGenericArguments();
        if (elTypes.Length > 0) return elTypes[0];

        // otherwise is not an 'enumerated' type
        return null;
    }

    
    public static string ToRacePositionText(this int pos)
    {
        switch (pos)
        {
            case 1: return pos + "st";
            case 2: return pos + "nd";
            case 3: return pos + "rd";
            default: return pos + "th";
        }
    }

    public static string GetFullName(this Enum myEnum)
    {
        return String.Format("{0}.{1}", myEnum.GetType().Name, myEnum.ToString());
    }

    /// <summary>
    /// Returns the number of nodes between the startNode and the targetNode
    /// Uses .Next and .Previous.
    /// </summary>
    public static int CountNodesTo<T>(this LinkedList<T> list, LinkedListNode<T> startNode,
        LinkedListNode<T> targetNode)
    {
        int count = 0;
        LinkedListNode<T> curNode = startNode;
        for (int loop = 0; loop < list.Count; loop++)
        {
            if (curNode == null)
                curNode = list.First;
            if (curNode == targetNode)
                break;
            curNode = curNode.Next;
            count++;
        }
        bool found = curNode == targetNode;
        int resultNext = count;
        count = 0;
        curNode = startNode;
        for (int loop = 0; loop < list.Count; loop++)
        {
            if (curNode == null)
                curNode = list.Last;
            if (curNode == targetNode)
                break;
            curNode = curNode.Previous;
            count++;
        }
        if (!found)
            found = curNode == targetNode;
        if (!found)
            throw new Exception("targetNode not found in LinkedList");
        return Math.Min(count, resultNext);
    }

    /// <summary>
    /// Because of floating point inaccuracies we use a small margin.
    /// </summary>
    /// <param name="f">this</param>
    /// <param name="to">Float to compare it to</param>
    /// <param name="margin">Margin for which to check.</param>
    /// <returns></returns>
    public static bool AboutEqualTo(this float f, float to, float margin = 1e-5f)
    {
        float result = Math.Abs(f - to);        
        return result < margin;
    }
    /// <summary>
    /// Because of floating point inaccuracies we use a small margin.
    /// </summary>
    /// <param name="f">this</param>
    /// <param name="to">Float to compare it to</param>
    /// <param name="margin">Margin for which to check.</param>
    /// <returns></returns>
    public static bool AboutEqualToOrMoreThan(this float f, float to, float margin = 1e-5f)
    {
        if (f > to) return true;        
        return f.AboutEqualTo(to, margin);
    }

    public static bool IsNullOrEmpty<T>(this IList<T> list)
    {
        return list == null || list.Count == 0;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        if (list == null) return;
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int index = _random.Next(0, n + 1);
            T value = list[index];
            list[index] = list[n];
            list[n] = value;
        }
    }

    public static T RandomItem<T>(this IList<T> list)
    {
        return list[_random.Next(0, list.Count)];
    }


#if (!UNITY_WINRT)
    /// <summary>
    /// Class must be marked [Serializable].
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T DeepClone<T>(T obj)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T) formatter.Deserialize(ms);
        }
    }
#endif

    /// <summary>
    /// Custom Foreach in ExtensionMethods.cs
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source == null) throw new ArgumentNullException("source");
        if (action == null) throw new ArgumentNullException("action");

        foreach (T item in source)
        {
            action(item);
        }
    }

    /// <summary>
    /// Clears the contents of the string builder. This method exists in .Net 4.0 but not in 2.0
    /// </summary>
    /// <param name="value">
    /// The <see cref="StringBuilder"/> to clear.
    /// </param>
    public static void ClearContents(this StringBuilder value)
    {
        value.Length = 0;
        value.Capacity = 0;
    }

    #region MoreLINQ

    #region License and Terms

    // MoreLINQ - Extensions to LINQ to Objects
    // Copyright (c) 2008-2011 Jonathan Skeet. All rights reserved.
    // 
    // Licensed under the Apache License, Version 2.0 (the "License");
    // you may not use this file except in compliance with the License.
    // You may obtain a copy of the License at
    // 
    //     http://www.apache.org/licenses/LICENSE-2.0
    // 
    // Unless required by applicable law or agreed to in writing, software
    // distributed under the License is distributed on an "AS IS" BASIS,
    // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    // See the License for the specific language governing permissions and
    // limitations under the License.

    #endregion

    /// <summary>
    /// Returns the minimal element of the given sequence, based on
    /// the given projection.
    /// </summary>
    /// <remarks>
    /// If more than one element has the minimal projected value, the first
    /// one encountered will be returned. This overload uses the default comparer
    /// for the projected type. This operator uses immediate execution, but
    /// only buffers a single result (the current minimal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <returns>The minimal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector)
    {
        return source.MinBy(selector, Comparer<TKey>.Default);
    }

    /// <summary>
    /// Returns the minimal element of the given sequence, based on
    /// the given projection and the specified comparer for projected values.
    /// </summary>
    /// <remarks>
    /// If more than one element has the minimal projected value, the first
    /// one encountered will be returned. This overload uses the default comparer
    /// for the projected type. This operator uses immediate execution, but
    /// only buffers a single result (the current minimal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <param name="comparer">Comparer to use to compare projected values</param>
    /// <returns>The minimal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
    /// or <paramref name="comparer"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector, IComparer<TKey> comparer)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        if (selector == null)
            throw new ArgumentNullException("selector");
        if (comparer == null)
            throw new ArgumentNullException("comparer");
        using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
        {
            if (!sourceIterator.MoveNext())
            {
                throw new InvalidOperationException("Sequence was empty");
            }
            TSource min = sourceIterator.Current;
            TKey minKey = selector(min);
            while (sourceIterator.MoveNext())
            {
                TSource candidate = sourceIterator.Current;
                TKey candidateProjected = selector(candidate);
                if (comparer.Compare(candidateProjected, minKey) >= 0) continue;
                min = candidate;
                minKey = candidateProjected;
            }
            return min;
        }
    }

    /// <summary>
    /// Returns the maximimal element of the given sequence, based on the given projection.
    /// </summary>
    /// <remarks>
    /// If more than one element has the maximal projected value, the first
    /// one encountered will be returned. This overload uses the default comparer
    /// for the projected type. This operator uses immediate execution, but
    /// only buffers a single result (the current maximal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <returns>The maximal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector)
    {
        return source.MaxBy(selector, Comparer<TKey>.Default);
    }

    /// <summary>
    /// Returns the maximal element of the given sequence, based on
    /// the given projection and the specified comparer for projected values.
    /// </summary>
    /// <remarks>
    /// If more than one element has the maximal projected value, the first
    /// one encountered will be returned. This overload uses the default comparer
    /// for the projected type. This operator uses immediate execution, but
    /// only buffers a single result (the current maximal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <param name="comparer">Comparer to use to compare projected values</param>
    /// <returns>The maximal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
    /// or <paramref name="comparer"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector, IComparer<TKey> comparer)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        if (selector == null)
            throw new ArgumentNullException("selector");
        if (comparer == null)
            throw new ArgumentNullException("comparer");
        using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
        {
            if (!sourceIterator.MoveNext())
            {
                throw new InvalidOperationException("Sequence was empty");
            }
            TSource max = sourceIterator.Current;
            TKey maxKey = selector(max);
            while (sourceIterator.MoveNext())
            {
                TSource candidate = sourceIterator.Current;
                TKey candidateProjected = selector(candidate);
                if (comparer.Compare(candidateProjected, maxKey) <= 0) continue;
                max = candidate;
                maxKey = candidateProjected;
            }
            return max;
        }
    }


    public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
    {
        if (val.CompareTo(min) < 0) return min;
        if (val.CompareTo(max) > 0) return max;
        return val;
    }

    #endregion

    /// <summary>
    /// This method exists in .Net 4.0 but not in 2.0
    /// </summary>
    public static bool IsNullEmptyOrWhitespace(this string s)
    {
        return s == null || s.All(char.IsWhiteSpace);
    }

    #region CheckCharacters


    /// <summary>
    /// Returns if the string length contains equal or more than value
    /// </summary>
    /// <param name="s"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool HasMinCharacters(this string s, int value)
    {
        return s.Length >= value;
    }

    /// <summary>
    /// Returns if the string length contains equal or less than value
    /// </summary>
    /// <param name="s"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool HasMaxCharacters(this string s, int value)
    {
        return s.Length <= value;
    }

    /// <summary>
    /// Returns if the string length contains between minValue and maxValue
    /// </summary>
    /// <param name="s"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public static bool HasMinAndMaxCharacters(this string s, int minValue, int maxValue)
    {
        return s.HasMinCharacters(minValue) && s.HasMaxCharacters(maxValue);
    }

    #endregion

    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    /// <summary>
    /// Returns the elements of a collection to a string using a seperator for each element.
    /// </summary>
    public static string ToStringCollection<T>(this IEnumerable<T> something, string seperator)
    {
        StringBuilder builder = new StringBuilder();
        foreach (T t in something)
            builder.Append(t + seperator);
            builder.Remove(builder.Length - seperator.Length, seperator.Length);
        return builder.ToString();
    }

}