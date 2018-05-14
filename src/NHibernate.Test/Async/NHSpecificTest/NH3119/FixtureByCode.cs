﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate.Bytecode;
using NHibernate.Bytecode.Lightweight;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Properties;
using NUnit.Framework;
using NHibernate.Linq;

namespace NHibernate.Test.NHSpecificTest.NH3119
{
	using System.Threading.Tasks;
	[TestFixture]
	public class ByCodeFixtureAsync : TestCaseMappingByCode
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Name);
				rc.Component(x => x.Component, c =>
				{
					c.Property(x => x.Value, pmapper => pmapper.Column("`Value`"));
				});
			});

			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		private static IBytecodeProvider _backupByteCodeProvider;

		protected override void OnSetUp()
		{
			_backupByteCodeProvider = Environment.BytecodeProvider;

			if (!Environment.UseReflectionOptimizer)
			{
				Assert.Ignore("Test only works with reflection optimization enabled");
			}

			using (ISession session = OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				var e1 = new Entity { Name = "Name", Component = new Component { Value = "Value" } };
				session.Save(e1);

				session.Flush();
				transaction.Commit();
			}

			// Change refelection optimizer and recreate the configuration and factory
			Environment.BytecodeProvider = new TestBytecodeProviderImpl();
			Configure();
			RebuildSessionFactory();
		}

		protected override void OnTearDown()
		{
			Environment.BytecodeProvider = _backupByteCodeProvider;

			using (ISession session = OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				session.Delete("from System.Object");

				session.Flush();
				transaction.Commit();
			}
		}

		[Test]
		public async Task PocoComponentTuplizer_Instantiate_UsesReflectonOptimizerAsync()
		{
			using (ISession freshSession = OpenSession())
			using (freshSession.BeginTransaction())
			{
				ComponentTestReflectionOptimizer.IsCalledForComponent = false;
				await (freshSession.Query<Entity>().SingleAsync());
				Assert.That(ComponentTestReflectionOptimizer.IsCalledForComponent, Is.True);
			}
		}

		[Test]
		public async Task PocoComponentTuplizerOfDeserializedConfiguration_Instantiate_UsesReflectonOptimizerAsync()
		{
			TestsContext.AssumeSystemTypeIsSerializable();

			MemoryStream configMemoryStream = new MemoryStream();
			BinaryFormatter writer = new BinaryFormatter();
			writer.Serialize(configMemoryStream, cfg);

			configMemoryStream.Seek(0, SeekOrigin.Begin);
			BinaryFormatter reader = new BinaryFormatter();
			Configuration deserializedConfig = (Configuration)reader.Deserialize(configMemoryStream);
			ISessionFactory factoryFromDeserializedConfig = deserializedConfig.BuildSessionFactory();

			using (ISession deserializedSession = factoryFromDeserializedConfig.OpenSession())
			using (deserializedSession.BeginTransaction())
			{
				ComponentTestReflectionOptimizer.IsCalledForComponent = false;
				await (deserializedSession.Query<Entity>().SingleAsync());
				Assert.That(ComponentTestReflectionOptimizer.IsCalledForComponent, Is.True);
			}
		}
	}
}