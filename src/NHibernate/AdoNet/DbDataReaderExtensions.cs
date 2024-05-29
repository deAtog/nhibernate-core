using System;
using System.Data.Common;

namespace NHibernate.AdoNet
{
	internal static class DbDataReaderExtensions
	{
		public static bool TryGetBoolean(this DbDataReader rs, int ordinal, out bool value)
		{
			try
			{
				value = rs.GetBoolean(ordinal);
				return true;
			}
			catch (InvalidCastException)
			{
				value = default;
				return false;
			}
		}

		public static bool TryGetByte(this DbDataReader rs, int ordinal, out byte value)
		{
			try
			{
				value = rs.GetByte(ordinal);
				return true;
			}
			catch (InvalidCastException)
			{
				value = default;
				return false;
			}
		}

		public static bool TryGetChar(this DbDataReader rs, int ordinal, out char value)
		{
			try
			{
				value = rs.GetChar(ordinal);
				return true;
			}
			catch (InvalidCastException)
			{
				value = default;
				return false;
			}
		}

		public static bool TryGetDecimal(this DbDataReader rs, int ordinal, out decimal value)
		{
			try
			{
				value = rs.GetDecimal(ordinal);
				return true;
			}
			catch (InvalidCastException)
			{
				value = default;
				return false;
			}
		}

		public static bool TryGetDouble(this DbDataReader rs, int ordinal, out double value)
		{
			try
			{
				value = rs.GetDouble(ordinal);
				return true;
			}
			catch (InvalidCastException)
			{
				value = default;
				return false;
			}
		}

		public static bool TryGetDateTime(this DbDataReader rs, int ordinal, out DateTime value)
		{
			try
			{
				value = rs.GetDateTime(ordinal);
				return true;
			}
			catch (InvalidCastException)
			{
				value = default;
				return false;
			}
		}
		public static bool TryGetFloat(this DbDataReader rs, int ordinal, out float value)
		{
			try
			{
				value = rs.GetFloat(ordinal);
				return true;
			}
			catch (InvalidCastException)
			{
				value = default;
				return false;
			}
		}
		public static bool TryGetGuid(this DbDataReader rs, int ordinal, out Guid value)
		{
			try
			{
				value = rs.GetGuid(ordinal);
				return true;
			}
			catch (InvalidCastException)
			{
				value = default;
				return false;
			}
		}

		public static bool TryGetUInt16(this DbDataReader rs, int ordinal, out ushort value)
		{
			var dbValue = rs[ordinal];

			if (dbValue is ushort)
			{
				value = (ushort) dbValue;
				return true;
			}

			value = default;
			return false;
		}

		public static bool TryGetInt16(this DbDataReader rs, int ordinal, out short value)
		{
			try
			{
				value = rs.GetInt16(ordinal);
				return true;
			}
			catch (InvalidCastException)
			{
				value = default;
				return false;
			}
		}
		public static bool TryGetInt32(this DbDataReader rs, int ordinal, out int value)
		{
			try
			{
				value = rs.GetInt32(ordinal);
				return true;
			}
			catch (InvalidCastException)
			{
				value = default;
				return false;
			}
		}

		public static bool TryGetUInt32(this DbDataReader rs, int ordinal, out uint value)
		{
			var dbValue = rs[ordinal];

			if (dbValue is uint)
			{
				value = (uint) dbValue;
				return true;
			}

			value = default;
			return false;
		}

		public static bool TryGetInt64(this DbDataReader rs, int ordinal, out long value)
		{
			try
			{
				value = rs.GetInt64(ordinal);
				return true;
			}
			catch (InvalidCastException)
			{
				value = default;
				return false;
			}
		}

		public static bool TryGetUInt64(this DbDataReader rs, int ordinal, out ulong value)
		{
			var dbValue = rs[ordinal];

			if (dbValue is ulong)
			{
				value = (ulong) dbValue;
				return true;
			}

			value = default;
			return false;
		}

		public static bool TryGetSByte(this DbDataReader rs, int ordinal, out sbyte value)
		{
			var dbValue = rs[ordinal];

			if (dbValue is sbyte)
			{
				value = (sbyte) rs[ordinal];
				return true;
			}

			value = default;
			return false;
		}

		public static bool TryGetString(this DbDataReader rs, int ordinal, out string value)
		{
			try
			{
				value = rs.GetString(ordinal);
				return true;
			}
			catch (InvalidCastException)
			{
				value = default;
				return false;
			}
		}

		public static bool TryGetTimeSpan(this DbDataReader rs, int ordinal, out TimeSpan value)
		{
			var dbValue = rs[ordinal];

			if (dbValue is TimeSpan)
			{
				value = (TimeSpan) dbValue;
				return true;
			}

			value = default;
			return false;
		}
	}
}