﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Linq;
using NHibernate.Cfg;
using NHibernate.DomainModel.Northwind.Entities;
using NHibernate.SqlCommand;
using NUnit.Framework;

namespace NHibernate.Test.Criteria.ReadonlyTests
{
	using System.Threading.Tasks;
	[TestFixture]
	public class QueryOverCacheableTestsAsync : CriteriaNorthwindReadonlyTestCase
	{
		//Just for discoverability
		private class CriteriaCacheableTest{}
		
		protected override void Configure(Configuration config)
		{
			config.SetProperty(Environment.UseQueryCache, "true");
			config.SetProperty(Environment.GenerateStatistics, "true");
			base.Configure(config);
		}

		[Test]
		public async Task QueryIsCacheableAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			await (db.Customers.Cacheable().Take(1).ListAsync());
			await (db.Customers.Cacheable().Take(1).ListAsync());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(1), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task QueryIsCacheable2Async()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			await (db.Customers.Cacheable().Take(1).ListAsync());
			await (db.Customers.Take(1).ListAsync());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(0), "Unexpected cache hit count");
		}

		[Test]
		public async Task QueryIsCacheableWithRegionAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());
			await (Sfi.EvictQueriesAsync("test"));
			await (Sfi.EvictQueriesAsync("other"));

			await (db.Customers.Cacheable().Take(1).CacheRegion("test").ListAsync());
			await (db.Customers.Cacheable().Take(1).CacheRegion("test").ListAsync());
			await (db.Customers.Cacheable().Take(1).CacheRegion("other").ListAsync());

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(2), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(2), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task CanBeCombinedWithFetchAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			await (db.Customers
			.Cacheable()
			.ListAsync());

			await (db.Orders
				.Cacheable()
				.Take(1)
				.ListAsync());

			await (db.Customers
				.Fetch(SelectMode.Fetch, x => x.Orders)
				.Cacheable()
				.Take(1)
				.ListAsync());

			await (db.Orders
				.Fetch(SelectMode.Fetch, x => x.OrderLines)
				.Cacheable()
				.Take(1)
				.ListAsync());

			await (db.Customers
				.Fetch(SelectMode.Fetch, x => x.Address)
				.Where(x => x.CustomerId == "VINET")
				.Cacheable()
				.SingleOrDefaultAsync());

			var customer = await (db.Customers
				.Fetch(SelectMode.Fetch, x => x.Address)
				.Where(x => x.CustomerId == "VINET")
				.Cacheable()
				.SingleOrDefaultAsync());

			Assert.That(NHibernateUtil.IsInitialized(customer.Address), Is.True, "Expected the fetched Address to be initialized");
			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(5), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(5), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task FetchIsCacheableAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			var order = (await (db.Orders
					.Fetch(
						SelectMode.Fetch,
						x => x.Customer,
						x => x.OrderLines,
						x => x.OrderLines.First().Product,
						x => x.OrderLines.First().Product.OrderLines)
					.Where(x => x.OrderId == 10248)
					.Cacheable()
					.ListAsync()))
					.First();

			AssertFetchedOrder(order);

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(1), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(1), "Unexpected cache miss count");

			Sfi.Statistics.Clear();
			Session.Clear();

			order = (await (db.Orders
					.Fetch(
						SelectMode.Fetch,
						x => x.Customer,
						x => x.OrderLines,
						x => x.OrderLines.First().Product,
						x => x.OrderLines.First().Product.OrderLines)
					.Where(x => x.OrderId == 10248)
					.Cacheable()
					.ListAsync()))
					.First();

			AssertFetchedOrder(order);

			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(0), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(0), "Unexpected cache miss count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task FetchIsCacheableForJoinAliasAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());

			Customer customer = null;
			OrderLine orderLines = null;
			Product product = null;
			OrderLine prOrderLines = null;

			var order = (await (db.Orders
					.JoinAlias(x => x.Customer, () => customer)
					.JoinAlias(x => x.OrderLines, () => orderLines, JoinType.LeftOuterJoin)
					.JoinAlias(() => orderLines.Product, () => product)
					.JoinAlias(() => product.OrderLines, () => prOrderLines, JoinType.LeftOuterJoin)
					.Where(x => x.OrderId == 10248)
					.Cacheable()
					.ListAsync()))
					.First();

			AssertFetchedOrder(order);

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(1), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(1), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(1), "Unexpected cache miss count");

			Sfi.Statistics.Clear();
			Session.Clear();

			order = (await (db.Orders
					.JoinAlias(x => x.Customer, () => customer)
					.JoinAlias(x => x.OrderLines, () => orderLines, JoinType.LeftOuterJoin)
					.JoinAlias(() => orderLines.Product, () => product)
					.JoinAlias(() => product.OrderLines, () => prOrderLines, JoinType.LeftOuterJoin)
					.Where(x => x.OrderId == 10248)
					.Cacheable()
					.ListAsync()))
					.First();

			AssertFetchedOrder(order);

			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(0), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(0), "Unexpected cache miss count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(1), "Unexpected cache hit count");
		}

		[Test]
		public async Task FutureFetchIsCacheableAsync()
		{
			Sfi.Statistics.Clear();
			await (Sfi.EvictQueriesAsync());
			var multiQueries = Sfi.ConnectionProvider.Driver.SupportsMultipleQueries;

			db.Orders
			.Fetch(SelectMode.Fetch, x => x.Customer)
			.Where(x => x.OrderId == 10248)
			.Cacheable()
			.Future();

			var order = db.Orders
					.Fetch(
						SelectMode.Fetch,
						x => x.OrderLines,
						x => x.OrderLines.First().Product,
						x => x.OrderLines.First().Product.OrderLines)
					.Where(x => x.OrderId == 10248)
					.Cacheable()
					.Future()
					.ToList()
					.First();

			AssertFetchedOrder(order);

			Assert.That(Sfi.Statistics.QueryExecutionCount, Is.EqualTo(multiQueries ? 1 : 2), "Unexpected execution count");
			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(2), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(2), "Unexpected cache miss count");

			Sfi.Statistics.Clear();
			Session.Clear();

			db.Orders
			.Fetch(SelectMode.Fetch, x => x.Customer)
			.Where(x => x.OrderId == 10248)
			.Cacheable()
			.Future();

			order = db.Orders
					.Fetch(
						SelectMode.Fetch,
						x => x.OrderLines,
						x => x.OrderLines.First().Product,
						x => x.OrderLines.First().Product.OrderLines)
					.Where(x => x.OrderId == 10248)
					.Cacheable()
					.Future()
					.ToList()
					.First();
			
			AssertFetchedOrder(order);

			Assert.That(Sfi.Statistics.QueryCachePutCount, Is.EqualTo(0), "Unexpected cache put count");
			Assert.That(Sfi.Statistics.QueryCacheMissCount, Is.EqualTo(0), "Unexpected cache miss count");
			Assert.That(Sfi.Statistics.QueryCacheHitCount, Is.EqualTo(2), "Unexpected cache hit count");
		}

		private static void AssertFetchedOrder(Order order)
		{
			Assert.That(order.Customer, Is.Not.Null, "Expected the fetched Customer to be not null");
			Assert.That(NHibernateUtil.IsInitialized(order.Customer), Is.True, "Expected the fetched Customer to be initialized");
			Assert.That(NHibernateUtil.IsInitialized(order.OrderLines), Is.True, "Expected the fetched  OrderLines to be initialized");
			Assert.That(order.OrderLines, Has.Count.EqualTo(3), "Expected the fetched OrderLines to have 3 items");
			var orderLine = order.OrderLines.First();
			Assert.That(orderLine.Product, Is.Not.Null, "Expected the fetched Product to be not null");
			Assert.That(NHibernateUtil.IsInitialized(orderLine.Product), Is.True, "Expected the fetched Product to be initialized");
			Assert.That(NHibernateUtil.IsInitialized(orderLine.Product.OrderLines), Is.True, "Expected the fetched OrderLines to be initialized");
		}
	}
}
