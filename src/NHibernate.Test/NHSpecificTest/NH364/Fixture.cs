using System;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH364
{
	[TestFixture]
	public class Fixture : BugTestCase
	{
		public override string BugNumber
		{
			get { return "NH364"; }
		}

		[Test]
		public void IdBagIdentity()
		{
			using (ISession s = OpenSession())
			{
				Category cat1 = new Category();
				cat1.Name = "Cat 1";

				Link link1 = new Link();
				link1.Name = "Link 1";
				link1.Categories.Add(cat1);

				s.Save(cat1);
				s.Save(link1);
				s.Flush();

				s.Delete("from Link");
				s.Delete("from Category");
				s.Flush();
			}
		}

		Product product1;
		Product product2;
		Product product3;
		Invoice inv;

		private void IdBagWithCompositeElementThatContainsAManyToOne_Setup()
		{
			using (ISession s = OpenSession())
			{
				product1 = new Product("Star Wars DVD");
				product2 = new Product("100TB Hard Drive");
				product3 = new Product("Something else");

				s.Save(product1);
				s.Save(product2);
				s.Save(product3);

				inv = new Invoice();
				inv.Number = "123";
				InvoiceItem item = new InvoiceItem();
				inv.Items.Add(new InvoiceItem(product1, 1));
				inv.Items.Add(new InvoiceItem(product2, 1));

				s.Save(inv);

				s.Flush();
			}
		}

		private void IdBagWithCompositeElementThatContainsAManyToOne_CleanUp()
		{
			using (ISession s = OpenSession())
			{
				s.Delete("from Invoice");
				s.Delete("from Product");
				s.Flush();
			}
		}

		[Test]
		public void IdBagWithCompositeElementThatContainsAManyToOne_Insert()
		{
			IdBagWithCompositeElementThatContainsAManyToOne_Setup();
			using (ISession s = OpenSession())
			{
				Invoice invLoaded = s.Get(typeof(Invoice), inv.Id) as Invoice;
				Assert.AreEqual(2, invLoaded.Items.Count, "Expected 2 things in the invoice");
				s.Clear();
			}
			IdBagWithCompositeElementThatContainsAManyToOne_CleanUp();
		}

		[Test]
		public void IdBagWithCompositeElementThatContainsAManyToOne_Update()
		{
			IdBagWithCompositeElementThatContainsAManyToOne_Setup();
			using (ISession s = OpenSession())
			{
				Invoice invToUpdate = s.Get(typeof(Invoice), inv.Id) as Invoice;
				((InvoiceItem)invToUpdate.Items[0]).Quantity = 10; // update information of an element
				invToUpdate.Items.Add(new InvoiceItem(product3, 1)); // update the idbag collection
				s.Flush();
				s.Clear();
			}
			using (ISession s = OpenSession())
			{
				Invoice invLoaded = s.Get(typeof(Invoice), inv.Id) as Invoice;
				Assert.AreEqual(10m, ((InvoiceItem)invLoaded.Items[0]).Quantity);
				Assert.AreEqual(3, invLoaded.Items.Count, "The collection should have a new item");
				s.Clear();
			}
			IdBagWithCompositeElementThatContainsAManyToOne_CleanUp();
		}

		[Test]
		public void IdBagWithCompositeElementThatContainsAManyToOne_Delete()
		{
			IdBagWithCompositeElementThatContainsAManyToOne_Setup();
			using (ISession s = OpenSession())
			{
				Invoice invToUpdate = s.Get(typeof(Invoice), inv.Id) as Invoice;
				invToUpdate.Items.RemoveAt(0);
				s.Flush();
				s.Clear();
			}
			using (ISession s = OpenSession())
			{
				Invoice invLoaded = s.Get(typeof(Invoice), inv.Id) as Invoice;
				Assert.AreEqual(1, invLoaded.Items.Count, "The collection should only have one item");
				s.Clear();
			}
			IdBagWithCompositeElementThatContainsAManyToOne_CleanUp();
		}
	}
}