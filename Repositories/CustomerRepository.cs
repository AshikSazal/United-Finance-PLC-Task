using Loan_Procedure.DbHelper;
using Loan_Procedure.DTOs;
using Loan_Procedure.DTOs.Customer;
using Loan_Procedure.Models;
using Loan_Procedure.Repositories.Interfaces;
using Loan_Procedure.Utils;
using Microsoft.Data.SqlClient;

namespace Loan_Procedure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DbConnection _dbConnection;
        public CustomerRepository(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public Response AddCustomer(Customer customer)
        {
            try
            {
                using var con = _dbConnection.CreateConnection();

                string query = @"INSERT INTO Customers(Name,NID,Mobile,Income) VALUES(@Name,@NID,@Mobile,@Income)";

                using SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@NID", customer.NID);
                cmd.Parameters.AddWithValue("@Mobile", customer.Mobile);
                cmd.Parameters.AddWithValue("@Income", customer.Income);

                con.Open();
                cmd.ExecuteNonQuery();

                return Response.Ok("Customer added successfully.");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE KEY constraint"))
                {
                    return Response.Fail("NID can't be duplicate");
                }
                return Response.Fail("Something Went Wrong!!!");
            }
        }
        public List<CustomerResponseDto> GetCustomers()
        {
            List<CustomerResponseDto> list = new();

            try
            {
                using var con = _dbConnection.CreateConnection();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Customers", con);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new CustomerResponseDto
                    {
                        CustomerId = Convert.ToInt32(reader["CustomerId"]),
                        Name = reader["Name"].ToString(),
                        NID = reader["NID"].ToString(),
                        Mobile = reader["Mobile"].ToString(),
                        Income = (decimal)reader["Income"]
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving customers.", ex);
            }

            return list;
        }

        public PagedResult<CustomerResponseDto> GetCustomers(int page = 1, int pageSize = 10)
        {
            var result = new PagedResult<CustomerResponseDto>();

            try
            {
                using var con = _dbConnection.CreateConnection();

                string query = @"
                    SELECT COUNT(*) OVER() AS TotalRecords, *
                    FROM Customers
                    ORDER BY Name
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                ";

                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                con.Open();

                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.TotalRecords = Convert.ToInt32(reader["TotalRecords"]);
                    result.Items.Add(new CustomerResponseDto
                    {
                        CustomerId = Convert.ToInt32(reader["CustomerId"]),
                        Name = reader["Name"]?.ToString() ?? string.Empty,
                        NID = reader["NID"]?.ToString() ?? string.Empty,
                        Mobile = reader["Mobile"]?.ToString() ?? string.Empty,
                        Income = reader["Income"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Income"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving customers.", ex);
            }

            return result;
        }

        public Response UpdateCustomer(Customer customer)
        {
            try
            {
                using var con = _dbConnection.CreateConnection();

                string query = @"UPDATE Customers
                    SET Name=@Name, NID=@NID, Mobile=@Mobile, Income=@Income
                    WHERE CustomerId=@CustomerId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@NID", customer.NID);
                cmd.Parameters.AddWithValue("@Mobile", customer.Mobile);
                cmd.Parameters.AddWithValue("@Income", customer.Income);
                cmd.Parameters.AddWithValue("@CustomerId", customer.CustomerId);

                con.Open();
                cmd.ExecuteNonQuery();
                return Response.Ok("Customer updated successfully.");
            }
            catch (Exception ex)
            {
                return Response.Fail("Something Went Wrong!!!");
            }
        }
        public Customer? GetCustomer(int customerId)
        {
            try
            {
                using var con = _dbConnection.CreateConnection();

                string query = @"SELECT CustomerId, Name, NID, Mobile, Income
                         FROM Customers
                         WHERE CustomerId = @CustomerId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CustomerId", customerId);

                con.Open();
                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Customer
                    {
                        CustomerId = Convert.ToInt32(reader["CustomerId"]),
                        Name = reader["Name"].ToString(),
                        NID = reader["NID"].ToString(),
                        Mobile = reader["Mobile"].ToString(),
                        Income = Convert.ToDecimal(reader["Income"])
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving the customer.", ex);
            }
        }
    }
}
