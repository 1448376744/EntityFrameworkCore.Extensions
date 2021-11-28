﻿using Dapper;
using EntityFrameworkCore.Extensions.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace EntityFrameworkCore.Extensions.Query
{
    public class DapperQueryProvider
    {
        private readonly DatabaseFacade _database;

        public DapperQueryProvider(DatabaseFacade database)
        {
            _database = database;
        }
        
        public IDbContextTransaction? CurrentTransaction => _database.CurrentTransaction;

        public IDbContextTransaction BeginTransactionScope(IsolationLevel? isolationLevel)
        {
            if (_database.CurrentTransaction != null)
            {
                throw new InvalidOperationException("Nested transactions are not allowed");
            }
            if (isolationLevel != null)
            {
                return _database.BeginTransaction(isolationLevel.Value);
            }
            else
            {
                return _database.BeginTransaction();
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionScopeAsync(IsolationLevel? isolationLevel)
        {
            if (_database.CurrentTransaction != null)
            {
                throw new InvalidOperationException("Nested transactions are not allowed");
            }
            if (isolationLevel != null)
            {
                return await _database.BeginTransactionAsync(isolationLevel.Value);
            }
            else
            {
                return await _database.BeginTransactionAsync();
            }
        }

        public int Execute(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var transaction = CurrentTransaction?.GetDbTransaction();
            return GetDbConnection().Execute(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<int> ExecuteAsync(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var transaction = CurrentTransaction?.GetDbTransaction();
            return GetDbConnection().ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public T ExecuteScalar<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var transaction = CurrentTransaction?.GetDbTransaction();
            return GetDbConnection().ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var transaction = CurrentTransaction?.GetDbTransaction();
            return GetDbConnection().ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public IEnumerable<dynamic> Query(string sql, object? param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var transaction = CurrentTransaction?.GetDbTransaction();
            return GetDbConnection().Query(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public Task<IEnumerable<dynamic>> QueryAsync(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var transaction = CurrentTransaction?.GetDbTransaction();
            return GetDbConnection().QueryAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public IEnumerable<T> Query<T>(string sql, object? param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
        {
            var transaction = CurrentTransaction?.GetDbTransaction();
            return GetDbConnection().Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var transaction = CurrentTransaction?.GetDbTransaction();
            return GetDbConnection().QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        private IDbConnection GetDbConnection()
        {
            return _database.GetDbConnection();
        }
    }
}
