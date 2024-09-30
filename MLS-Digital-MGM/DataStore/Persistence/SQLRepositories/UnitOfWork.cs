using System;
using System.Threading.Tasks;
using DataStore.Data;
using DataStore.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataStore.Persistence.SQLRepositories
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed;
        private bool _isTransactionActive = false;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (!_isTransactionActive)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
                _isTransactionActive = true;
            }
            return _transaction;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CommitAsync()
        {
            if (!_isTransactionActive)
            {
                return await _context.SaveChangesAsync();
            }

            try
            {
                int result = await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
                return result;
            }
            finally
            {
                await DisposeTransactionAsync();
                _isTransactionActive = false;
            }
        }

        public async Task RollbackAsync()
        {
            if (_isTransactionActive)
            {
                await _transaction.RollbackAsync();
                await DisposeTransactionAsync();
                _isTransactionActive = false;
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!_disposed)
            {
                if (_transaction != null)
                {
                    await DisposeTransactionAsync();
                }
                await _context.DisposeAsync();
                _disposed = true;
            }
        }
    }
}
