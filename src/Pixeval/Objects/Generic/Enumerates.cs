﻿// Pixeval - A Strong, Fast and Flexible Pixiv Client
//  Copyright (C) 2019-2020 Dylech30th
// This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as
//  published by the Free Software Foundation, either version 3 of the
//  License, or (at your option) any later version.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Pixeval.Objects.Primitive;

namespace Pixeval.Objects.Generic
{
    public static class Enumerates
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        public static void AddRange<T>(this ICollection<T> dst, IEnumerable<T> src)
        {
            foreach (var t in src) dst.Add(t);
        }

        public static void AddSorted<T>(this IList<T> list, T item, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                list.Add(item);
                return;
            }

            var i = 0;
            while (i < list.Count && comparer.Compare(list[i], item) < 0) i++;

            list.Insert(i, item);
        }

        public static IImmutableSet<TR> ToImmutableSet<T, TR>(this IEnumerable<T> enumerable, Func<T, TR> function)
        {
            return enumerable == null ? new HashSet<TR>().ToImmutableHashSet() : enumerable.Select(function).ToImmutableHashSet();
        }

        public static bool EqualsIgnoreCase(this IEnumerable<string> src, IEnumerable<string> compare)
        {
            return src.All(x => Strings.IsNullOrEmpty(x) && compare.Any(i => i.EqualsIgnoreCase(x)));
        }

        public static IEnumerable<T> Peek<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            var peek = enumerable as T[] ?? enumerable.ToArray();
            foreach (var t in peek) action(t);

            return peek;
        }

        public static IEnumerable<T> NonNull<T>(this IEnumerable<T> source)
        {
            return source.Where(s => s != null);
        }
    }
}