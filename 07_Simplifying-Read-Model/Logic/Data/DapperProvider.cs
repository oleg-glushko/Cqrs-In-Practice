using Microsoft.Extensions.Configuration;
using System.Data;

namespace Logic.Data;

public class DapperProvider(IConfiguration configuration)
{
    private readonly string connectionString = configuration.GetConnectionString("CqrsInPracticeDb")
            ?? throw new InvalidOperationException("Connection string can't be null");

    public IDbConnection Connect()
        => new Microsoft.Data.SqlClient.SqlConnection(connectionString);
}
