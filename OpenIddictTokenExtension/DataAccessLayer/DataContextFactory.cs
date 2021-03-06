using OpenIddictTokenExtension.DataAccessLayer.Interfaces;
using System;

namespace OpenIddictTokenExtension.DataAccessLayer
{
    public class DataContextFactory : IDataContextFactory, IDisposable
    {
        private readonly IDataContext _context;

        public DataContextFactory(IdentityDbContext dbContext)
        {
            _context = dbContext;
        }

        public IDataContext GetDataContext()
        {
            return _context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
