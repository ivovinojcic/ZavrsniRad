using System.Threading.Tasks;

namespace VeterinarskaStanica.Model.DatabaseConnector
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
