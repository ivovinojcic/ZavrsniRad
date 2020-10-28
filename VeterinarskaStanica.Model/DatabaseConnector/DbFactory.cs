using Microsoft.EntityFrameworkCore;
using VeterinarskaStanica.Model.Core;

namespace VeterinarskaStanica.Model.DatabaseConnector
{
    // Abstracting database entity so using can be made only dbContext object
    public class DbFactory : Disposable, IDbFactory
    {
        //Core
        DataBaseConnection _dbContext;

        //ThreadSafe
        private readonly DbContextOptions<DataBaseConnection> _dbContextOptions;

        public DbFactory(DataBaseConnection context, DbContextOptions<DataBaseConnection> dbContextOptions)
        {
            _dbContext = context;
            _dbContextOptions = dbContextOptions;
        }
        
        public DataBaseConnection Init()
        {
            return _dbContext ?? (_dbContext = new DataBaseConnection());
        }

        public DataBaseConnection InitThreadSafe()
        {
            return new DataBaseConnection(_dbContextOptions);
        }

        protected override void DisposeCore()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}