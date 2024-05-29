using System;
using System.Data.Common;
using System.Xml;
using NHibernate.AdoNet;
using NHibernate.Engine;
using NHibernate.SqlTypes;

namespace NHibernate.Type
{
	[Serializable]
	public partial class XmlDocType : MutableType
	{
		public XmlDocType()
			: base(new XmlSqlType())
		{
		}

		public XmlDocType(SqlType sqlType) : base(sqlType)
		{
		}

		public override string Name
		{
			get { return "XmlDoc"; }
		}

		public override System.Type ReturnedClass
		{
			get { return typeof (XmlDocument); }
		}

		/// <inheritdoc />
		public override void Set(DbCommand cmd, object value, int index, ISessionImplementor session)
		{
			cmd.Parameters[index].Value = GetStringRepresentation(value);
		}

		/// <inheritdoc />
		public override object Get(DbDataReader rs, int index, ISessionImplementor session)
		{
			if (!rs.TryGetString(index, out var dbValue))
			{
				var locale = session.Factory.Settings.Locale;

				// according to documentation, GetValue should return a string, at least for MsSQL
				// hopefully all DataProvider has the same behaviour
				dbValue = Convert.ToString(rs.GetValue(index), locale);
			}

			// 6.0 TODO: inline the call.
#pragma warning disable 618
			return FromStringValue(dbValue);
#pragma warning restore 618
		}

		/// <inheritdoc />
		public override string ToLoggableString(object value, ISessionFactoryImplementor factory)
		{
			return (value == null) ? null :
				// 6.0 TODO: inline this call.
#pragma warning disable 618
				ToString(value);
#pragma warning restore 618
		}

		// Since 5.2
		[Obsolete("This method has no more usages and will be removed in a future version. Override ToLoggableString instead.")]
		public override string ToString(object val)
		{
			return GetStringRepresentation(val);
		}

		// Since 5.2
		[Obsolete("This method has no more usages and will be removed in a future version.")]
		public override object FromStringValue(string xml)
		{
			return ParseStringRepresentation(xml);
		}

		public override object DeepCopyNotNull(object value)
		{
			var original = (XmlDocument) value;
			var copy = new XmlDocument();
			copy.LoadXml(original.OuterXml);
			return copy;
		}

		public override bool IsEqual(object x, object y)
		{
			if (x == null && y == null)
			{
				return true;
			}
			if (x == null || y == null)
			{
				return false;
			}
			return ((XmlDocument) x).OuterXml == ((XmlDocument) y).OuterXml;
		}

		/// <inheritdoc />
		public override object Assemble(object cached, ISessionImplementor session, object owner)
		{
			return ParseStringRepresentation(cached as string);
		}

		/// <inheritdoc />
		public override object Disassemble(object value, ISessionImplementor session, object owner)
		{
			return GetStringRepresentation(value);
		}

		private static string GetStringRepresentation(object value)
		{
			return ((XmlDocument) value)?.OuterXml;
		}

		private static object ParseStringRepresentation(string value)
		{
			if (value == null)
				return null;

			var xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(value);
			return xmlDocument;
		}
	}
}
