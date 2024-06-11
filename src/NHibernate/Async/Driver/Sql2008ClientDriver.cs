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
using System.Data.Common;
using NHibernate.Util;
using NHibernate.AdoNet;

namespace NHibernate.Driver
{
	using System.Threading.Tasks;
	using System.Threading;
	public partial class Sql2008ClientDriver : SqlClientDriver
	{

		#if NETFX
		#else

		#endif

		public override async Task<DbDataReader> ExecuteReaderAsync(DbCommand command, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			var reader = await (command.ExecuteReaderAsync(cancellationToken)).ConfigureAwait(false);

			return new NoCharDbDataReader(reader);
		}
	}
}
