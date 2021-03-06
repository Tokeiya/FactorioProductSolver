﻿/*
 * Copyright (C) 2012 - 2015 Takahisa Watanabe <linerlock@outlook.com> All rights reserved.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Parseq
{
	public static partial class Seq
	{
		public static void ForEach<T>(
			this IDelayed<ISeq<T>> delayedSeq,
			Action<T> action)
		{
			foreach (var item in delayedSeq.Force().AsEnumerable())
				action(item);
		}

		public static bool Any<T>(
			this IDelayed<ISeq<T>> delayedSeq,
			Func<T, bool> predicate)
		{
			return delayedSeq.Force().Case(
				() => false,
				pair =>
					predicate(pair.Item0) || pair.Item1.Any(predicate));
		}

		public static bool Any<T>(
			this IDelayed<ISeq<T>> delayedSeq)
		{
			return delayedSeq.Force().Case(
				() => false,
				_ => true);
		}

		public static int Count<T>(this IDelayed<ISeq<T>> delayedSeq)
		{
			return delayedSeq.Force()
				.AsEnumerable().Count();
		}

		public static T1 Foldl<T0, T1>(this IDelayed<ISeq<T0>> delayedSeq, T1 init, Func<T1, T0, T1> func)
		{
			return delayedSeq.Force()
				.Case(
					() =>
						init,
					pair => pair.Item1.Foldl(func(init, pair.Item0), func));
		}

		public static T1 Foldr<T0, T1>(this IDelayed<ISeq<T0>> delayedSeq, T1 init, Func<T0, T1, T1> func)
		{
			return delayedSeq.Force()
				.Case(
					() =>
						init,
					pair =>
						func(pair.Item0, pair.Item1.Foldr(init, func)));
		}

		public static IDelayed<ISeq<T1>> Unfold<T0, T1>(T0 init, Func<T0, IOption<IPair<T1, T0>>> func)
		{
			return Delayed.Return(() =>
				func(init).Case(
					() =>
						Empty<T1>()
							.Force(),
					pair =>
						Cons(pair.Item0, Delayed.Return(() => Unfold(pair.Item1, func).Force()))
							.Force()));
		}

		public static IDelayed<ISeq<T>> Singleton<T>(T value)
		{
			return Cons(value, Empty<T>());
		}

		public static IDelayed<ISeq<T>> Concat<T>(
			this IDelayed<ISeq<T>> delayedSeq0,
			IDelayed<ISeq<T>> delayedSeq1)
		{
			return delayedSeq0.FlatMap(seq0 =>
				seq0.Case(
					() => delayedSeq1,
					pair =>
						Cons(pair.Item0, Delayed.Return(() => pair.Item1.Concat(delayedSeq1).Force()))));
		}

		public static IDelayed<ISeq<T1>> Map<T0, T1>(
			this IDelayed<ISeq<T0>> delayedSeq,
			Func<T0, T1> func)
		{
			return delayedSeq.Select(func);
		}

		public static IDelayed<ISeq<T1>> FlatMap<T0, T1>(
			this IDelayed<ISeq<T0>> delayedSeq,
			Func<T0, IDelayed<ISeq<T1>>> func)
		{
			return delayedSeq.SelectMany(func);
		}

		public static IDelayed<ISeq<T>> Filter<T>(
			this IDelayed<ISeq<T>> delayedSeq,
			Func<T, bool> predicate)
		{
			return delayedSeq.Filter(predicate);
		}

		public static IEnumerable<T> AsEnumerable<T>(
			this IDelayed<ISeq<T>> delayedSeq)
		{
			foreach (var item in delayedSeq.Select(seq => seq.AsEnumerable()).Force())
				yield return item;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class SeqExtensions
	{
		public static IDelayed<ISeq<T>> Where<T>(
			this IDelayed<ISeq<T>> delayedSeq,
			Func<T, bool> predicate)
		{
			return delayedSeq.FlatMap(seq =>
				seq.Case(
					() => Seq.Empty<T>(),
					pair => predicate(pair.Item0)
						? Seq.Cons(pair.Item0, Delayed.Return(() => pair.Item1.Where(predicate).Force()))
						: Delayed.Return(() => pair.Item1.Where(predicate).Force())));
		}

		public static IDelayed<ISeq<T1>> Select<T0, T1>(
			this IDelayed<ISeq<T0>> delayedSeq,
			Func<T0, T1> selector)
		{
			return delayedSeq.FlatMap(seq =>
				seq.Case(
					() =>
						Seq.Empty<T1>(),
					pair =>
						Seq.Cons(selector(pair.Item0), Delayed.Return(() => pair.Item1.Select(selector)).Force())));
		}

		public static IDelayed<ISeq<T1>> SelectMany<T0, T1>(
			this IDelayed<ISeq<T0>> delayedSeq,
			Func<T0, IDelayed<ISeq<T1>>> selector)
		{
			return delayedSeq.FlatMap(seq =>
				seq.Case(
					() =>
						Seq.Empty<T1>(),
					pair =>
						Seq.Concat(selector(pair.Item0),
							Delayed.Return(() => pair.Item1.SelectMany(selector).Force()))));
		}

		public static IDelayed<ISeq<T2>> SelectMany<T0, T1, T2>(
			this IDelayed<ISeq<T0>> delayedSeq,
			Func<T0, IDelayed<ISeq<T1>>> selector,
			Func<T0, T1, T2> projector)
		{
			return delayedSeq.SelectMany(value0 => selector(value0).Select(value1 => projector(value0, value1)));
		}
	}
}