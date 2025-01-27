using System.Data;

namespace Infrastructure.Data.Connection
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
