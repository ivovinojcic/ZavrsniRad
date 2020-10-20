using VeterinarskaStanica.Model.Core;

namespace VeterinarskaStanica.Model.DatabaseConnector
{
    // Abstracting database entity so using can be made only dbContext object
    public class DbFactory : Disposable, IDbFactory
    {
        DataBaseConnection _dbContext;

        public DbFactory(DataBaseConnection context)
        {
            _dbContext = context;
        }
        
        public DataBaseConnection Init()
        {
            return _dbContext ?? (_dbContext = new DataBaseConnection());
        }

        protected override void DisposeCore()
        {
            if (_dbContext != null)
                _dbContext.Dispose();
        }
    }
}