using Empresaurio.TeamOrganizer.Domain.CustomEntities;
using Empresaurio.TeamOrganizer.Domain.Entities;
using Empresaurio.TeamOrganizer.Infrastructure.Data;
using Empresaurio.TeamOrganizer.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Empresaurio.TeamOrganizer.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _ConnectionString;
        public AccountRepository(DataAccess dataAccess)
        {
            _ConnectionString = dataAccess.ConnectionSQL;
        }

        public async Task<UserCredentials> AuthenticateAsync(Credentials credentials)
        {
            UserCredentials user = null;

            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("uspUserLogin", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 30).Value = credentials.EmailAddress;
                    command.Parameters.Add("@Password", SqlDbType.VarChar, 20).Value = credentials.Password;

                    SqlDataReader dataReader = await command.ExecuteReaderAsync();

                    if (dataReader != null) 
                    {
                        while (dataReader.Read())
                        {
                            user = new UserCredentials()
                            {
                                FirstName = dataReader.GetString(dataReader.GetOrdinal("FirstName")),
                                LastName = dataReader.GetString(dataReader.GetOrdinal("LastName")),
                                EmailAddress = dataReader.GetString(dataReader.GetOrdinal("EmailAddress")),
                                PhotoUrl = dataReader.GetString(dataReader.GetOrdinal("Url"))
                            };
                        }
                        dataReader.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return user;
        }

        public async Task RegisterAsync(User user)
        {
            using (SqlConnection connection = new SqlConnection(_ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("uspUserRegister", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@EmailAddress", SqlDbType.VarChar, 30).Value = user.EmailAddress;
                    command.Parameters.Add("@Password", SqlDbType.VarChar, 20).Value = user.Password;
                    command.Parameters.Add("@PhotoUrl", SqlDbType.VarChar, 255).Value = user.PhotoUrl;
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar, 20).Value = user.FirstName;
                    command.Parameters.Add("@MiddleName", SqlDbType.VarChar, 20).Value = user.MiddleName;
                    command.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = user.LastName;
                    command.Parameters.Add("@DateOfBirth", SqlDbType.Date).Value = user.DateOfBirth;
                    command.Parameters.Add("@Telephone", SqlDbType.Char, 9).Value = user.Telephone;

                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

    }
}
