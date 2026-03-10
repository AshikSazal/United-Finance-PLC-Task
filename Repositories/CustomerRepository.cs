using Loan_Procedure.DbHelper;
using Loan_Procedure.DTOs.Customer;
using Loan_Procedure.Models;
using Loan_Procedure.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Loan_Procedure.Utils;

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
                // You can log the exception here if needed
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
        public Response UpdateCustomer(Customer customer)
        {
            try
            {
                using var con = _dbConnection.CreateConnection();

                string query = @"UPDATE Customers
                         SET Name=@Name,
                             NID=@NID,
                             Mobile=@Mobile,
                             Income=@Income
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
        public Customer GetCustomer(int customerId)
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
