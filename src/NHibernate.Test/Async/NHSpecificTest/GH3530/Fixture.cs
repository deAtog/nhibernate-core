﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using NHibernate.SqlTypes;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.GH3530
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		private CultureInfo initialCulture;

		[OneTimeSetUp]
		public void FixtureSetup()
		{
			initialCulture = CurrentCulture;
		}

		[OneTimeTearDown]
		public void FixtureTearDown()
		{
			CurrentCulture = initialCulture;
		}

		protected override void OnTearDown()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.CreateQuery("delete from System.Object").ExecuteUpdate();

				transaction.Commit();
			}
		}

		protected override void CreateSchema()
		{
			CreateTable("Integer");
			CreateTable("DateTime");
			CreateTable("Double");
			CreateTable("Decimal");
		}

		private void CreateTable(string name)
		{
			var sb = new StringBuilder();
			var guidType = Dialect.GetTypeName(SqlTypeFactory.Guid);
			var stringType = Dialect.GetTypeName(SqlTypeFactory.GetAnsiString(255));

			var catalog = GetQuotedDefaultCatalog();
			var schema = GetQuotedDefaultSchema();
			var table = GetQualifiedName(catalog, schema, $"{name}Entity");

			sb.Append($"{Dialect.CreateTableString} {table} (");

			// Generate columns
			sb.Append($"Id {guidType}, ");
			sb.Append($"DataValue {stringType}");

			// Add the primary key contraint for the identity column
			sb.Append($", {Dialect.PrimaryKeyString} ( Id )");
			sb.Append(")");

			using (var cn = Sfi.ConnectionProvider.GetConnection())
			{
				try
				{
					using (var cmd = cn.CreateCommand())
					{
						cmd.CommandText = sb.ToString();
						cmd.ExecuteNonQuery();
					}
				}
				finally
				{
					Sfi.ConnectionProvider.CloseConnection(cn);
				}
			}
		}

		private string GetQuotedDefaultCatalog()
		{
			var t = cfg.GetType();
			var getQuotedDefaultCatalog = t.GetMethod("GetQuotedDefaultCatalog", BindingFlags.Instance | BindingFlags.NonPublic);

			return (string) getQuotedDefaultCatalog.Invoke(cfg, [Dialect]);
		}

		private string GetQuotedDefaultSchema()
		{
			var t = cfg.GetType();
			var getQuotedDefaultSchema = t.GetMethod("GetQuotedDefaultSchema", BindingFlags.Instance | BindingFlags.NonPublic);

			return (string) getQuotedDefaultSchema.Invoke(cfg, [Dialect]);
		}

		private string GetQualifiedName(string catalog, string schema, string name)
		{
			return Dialect.Qualify(catalog, schema, name);
		}

		private async Task PerformTestAsync<T, U>(CultureInfo from, CultureInfo to, T expectedValue, Action<T, T> assert, CancellationToken cancellationToken = default(CancellationToken))
			where T : struct
			where U : Entity<T>, new()
		{
			object id;

			CurrentCulture = from;
			using (var session = OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var entity = new U()
				{
					DataValue = expectedValue
				};

				id = await (session.SaveAsync(entity, cancellationToken));
				await (tx.CommitAsync(cancellationToken));
			}

			CurrentCulture = to;
			using (var session = OpenSession())
			using (var tx = session.BeginTransaction())
			{
				var entity = await (session.GetAsync<U>(id, cancellationToken));

				assert(expectedValue, entity.DataValue);
			}
		}

		[Test, TestCaseSource(nameof(GetTestCases))]
		public async Task TestDateTimeAsync(CultureInfo from, CultureInfo to)
		{
			DateTime leapDay = new DateTime(2024, 2, 29, new GregorianCalendar(GregorianCalendarTypes.USEnglish));

			await (PerformTestAsync<DateTime, DateTimeEntity>(from, to, leapDay, (expected, actual) => Assert.AreEqual(expected, actual)));
		}

		[Test, TestCaseSource(nameof(GetTestCases))]
		public async Task TestDecimalAsync(CultureInfo from, CultureInfo to)
		{
			decimal decimalValue = 12.3m;

			await (PerformTestAsync<decimal, DecimalEntity>(from, to, decimalValue, (expected, actual) => Assert.AreEqual(expected, actual)));
		}

		[Test, TestCaseSource(nameof(GetTestCases))]
		public async Task TestDoubleAsync(CultureInfo from, CultureInfo to)
		{
			double doubleValue = 12.3d;

			await (PerformTestAsync<double, DoubleEntity>(from, to, doubleValue, 
				(expected, actual) => Assert.True(Math.Abs(expected - actual) < double.Epsilon, $"Expected: {expected}\nBut was: {actual}\n")
			));
		}

		[Test, TestCaseSource(nameof(GetTestCases))]

		public async Task TestIntegerAsync(CultureInfo from, CultureInfo to)
		{
			int integerValue = 123;

			await (PerformTestAsync<int, IntegerEntity>(from, to, integerValue, (expected, actual) => Assert.AreEqual(expected, actual)));
		}

		private CultureInfo CurrentCulture
		{
			get
			{
				return CultureInfo.CurrentCulture;
			}
			set
			{
				CultureInfo.CurrentCulture = value;
			}
		}

		public static object[][] GetTestCases()
		{
			return [
				[new CultureInfo("en-US"), new CultureInfo("de-DE")],
				[new CultureInfo("en-US"), new CultureInfo("ar-SA", false)],
				[new CultureInfo("en-US"), new CultureInfo("th-TH", false)],
			];
		}
	}
}
