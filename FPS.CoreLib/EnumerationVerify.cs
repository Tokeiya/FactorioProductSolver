using System;
using System.Collections.Generic;

namespace FPS.CoreLib
{
	public static class EnumerationVerify
	{
		public static bool Verify<T>(this T value) where T : Enum
		{
			if (!typeof(T).IsEnum) throw new InvalidOperationException($"{nameof(T)} is not Enum.");
			return Cache<T>.Verify(value);
		}

		private static class Cache<TEnum>
		{
			private static readonly HashSet<TEnum> Set;

			static Cache()
			{
				Set = new HashSet<TEnum>((TEnum[]) Enum.GetValues(typeof(TEnum)));
			}

			public static bool Verify(TEnum value)
			{
				return Set.Contains(value);
			}
		}
	}
}