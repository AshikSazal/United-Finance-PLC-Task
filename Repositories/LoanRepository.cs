using Loan_Procedure.DbHelper;
using Loan_Procedure.DTOs;
using Loan_Procedure.DTOs.Loan;
using Loan_Procedure.Models;
using Loan_Procedure.Repositories.Interfaces;
using Loan_Procedure.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace Loan_Procedure.Repositories
{
    public class LoanRepository: ILoanRepository
    {
        private readonly DbConnection _dbConnection;
        public LoanRepository(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public Response CreateLoan(Loan loan)
        {
            try
            {
                using var con = _dbConnection.CreateConnection();

                string query = @"INSERT INTO Loans
                        (CustomerId,Amount,LoanType,Tenor,Purpose,Status)
                        VALUES
                        (@CustomerId,@Amount,@LoanType,@Tenor,@Purpose,0)";

                using SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@CustomerId", loan.CustomerId);
                cmd.Parameters.AddWithValue("@Amount", loan.Amount);
                cmd.Parameters.AddWithValue("@LoanType", loan.LoanType);
                cmd.Parameters.AddWithValue("@Tenor", loan.Tenor);
                cmd.Parameters.AddWithValue("@Purpose", loan.Purpose);

                con.Open();
                cmd.ExecuteNonQuery();
                return Response.Ok("Loan added successfully.");
            }
            catch (Exception ex)
            {
                return Response.Fail("Something Went Wrong!!!");
            }
        }

        public PagedResult<LoanResponseDto> GetLoans(int? status, int? customerId, int page = 1, int pageSize = 10)
        {
            try
            {
                var result = new PagedResult<LoanResponseDto>();

                using var con = _dbConnection.CreateConnection();
                string query = @"
        SELECT COUNT(*) OVER() AS TotalRecords, l.*, c.Name AS CustomerName
        FROM Loans l
        INNER JOIN Customers c ON l.CustomerId = c.CustomerId
        WHERE (@Status IS NULL OR l.Status = @Status)
        AND (@CustomerId IS NULL OR l.CustomerId = @CustomerId)
        ORDER BY l.CreatedDate DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
    ";

                using var cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Status", status ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CustomerId", customerId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                cmd.Parameters.AddWithValue("@PageSize", pageSize);

                con.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.TotalRecords = Convert.ToInt32(reader["TotalRecords"]); // same for all rows
                    result.Items.Add(new LoanResponseDto
                    {
                        LoanId = Convert.ToInt32(reader["LoanId"]),
                        CustomerId = Convert.ToInt32(reader["CustomerId"]),
                        CustomerName = reader["CustomerName"]?.ToString() ?? string.Empty,
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        LoanType = reader["LoanType"]?.ToString() ?? string.Empty,
                        Tenor = Convert.ToInt32(reader["Tenor"]),
                        Status = Convert.ToByte(reader["Status"]),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while retrieving loans.", ex);
            }
        }

        public Response UpdateLoan(Loan loan)
        {
            try
            {
                using var con = _dbConnection.CreateConnection();

                string query = @"UPDATE Loans
                             SET CustomerId=@CustomerId,
                                 Amount=@Amount,
                                 LoanType=@LoanType,
                                 Tenor=@Tenor,
                                 Purpose=@Purpose
                             WHERE LoanId=@LoanId
                             AND Status <= 1";

                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@CustomerId", loan.CustomerId);
                cmd.Parameters.AddWithValue("@Amount", loan.Amount);
                cmd.Parameters.AddWithValue("@LoanType", loan.LoanType);
                cmd.Parameters.AddWithValue("@Tenor", loan.Tenor);
                cmd.Parameters.AddWithValue("@Purpose", loan.Purpose);
                cmd.Parameters.AddWithValue("@LoanId", loan.LoanId);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    return Response.Ok("Loan updated successfully.");
                else
                    return Response.Fail("Loan cannot be updated (might be already approved/rejected).");
            }
            catch (Exception ex)
            {
                return new Response { Status = false, Message = $"Error updating loan: {ex.Message}" };
            }
        }
        public Response UpdateStatus(int loanId, int status)
        {
            try
            {
                using var con = _dbConnection.CreateConnection();

                string query = "UPDATE Loans SET Status=@Status WHERE LoanId=@LoanId";

                using SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@LoanId", loanId);

                con.Open();
                cmd.ExecuteNonQuery();
                return Response.Ok("Loan Update Successfully");
            }
            catch (Exception ex)
            {
                return Response.Fail("Something Went Wrong!!!");
            }
        }
        public Loan GetLoan(int id)
        {
            try
            {
                using var con = _dbConnection.CreateConnection();

                string query = @"SELECT l.* FROM Loans l WHERE l.LoanId = @LoanId";

                using SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@LoanId", id);

                con.Open();
                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Loan
                    {
                        LoanId = (int)reader["LoanId"],
                        CustomerId = (int)reader["CustomerId"],
                        Amount = (decimal)reader["Amount"],
                        LoanType = reader["LoanType"].ToString(),
                        Tenor = (int)reader["Tenor"],
                        Purpose = reader["Purpose"].ToString(),
                        Status = Convert.ToByte(reader["Status"]),
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving loan.", ex);
            }
        }
    }
}
