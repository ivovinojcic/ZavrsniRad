using System;
using VeterinarskaStanica.Model.Core;

namespace VeterinarskaStanica.Model.DatabaseConnector
{
   public interface IDbFactory : IDisposable
    {
        DataBaseConnection Init();
        DataBaseConnection InitThreadSafe();
    }
}
