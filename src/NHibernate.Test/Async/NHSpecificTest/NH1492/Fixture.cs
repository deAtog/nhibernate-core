﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections.Generic;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1492
{
	using System.Threading.Tasks;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		[Test]
		public async Task RetrieveEntitiesAsync()
		{
			Entity eDel = new Entity(1, "DeletedEntity");
			eDel.Deleted = "Y";

			Entity eGood = new Entity(2, "GoodEntity");
			eGood.Childs.Add(new ChildEntity(eGood, "GoodEntityChild"));

			// Make "Deleted" entity persistent
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				await (s.SaveAsync(eDel));
				await (s.SaveAsync(eGood));
				await (t.CommitAsync());
			}

			// Retrive (check if the entity was well persisted)
			IList<ChildEntity> childs;
			using (ISession s = OpenSession())
			{
				s.EnableFilter("excludeDeletedRows").SetParameter("deleted", "Y");

				IQuery q = s.CreateQuery("FROM ChildEntity c WHERE c.Parent.Code = :parentCode").SetParameter("parentCode", 2);
				childs=	await (q.ListAsync<ChildEntity>());
			}
			Assert.AreEqual(1, childs.Count);

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				await (s.DeleteAsync("from Entity"));
				await (t.CommitAsync());
			}
		}
	}
}