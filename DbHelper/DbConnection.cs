using Microsoft.Data.SqlClient;

namespace Loan_Procedure.DbHelper
{
    public class DbConnection
    {
        private readonly IConfiguration _configuration;
        public DbConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
