using Dapper;
using NLog.Fluent;
using Oracle.ManagedDataAccess.Client;
using paymentSolution.Models;
using PartyPal.Models.Request;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Net;

namespace paymentSolution.Implementation
{
    public class DatabaseCalls : IDatabaseCalls
    {
        private readonly ILogger<DatabaseCalls> _logger;
        private readonly IConfiguration _config;
        private readonly string connString;
        public DatabaseCalls(ILogger<DatabaseCalls> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            connString = _config["ConnectionStrings:OraConnection"];
        }

        

        public IEnumerable<Users.ResponseData> GetAllUsers()
        {
            List<Users.ResponseData> usersList = new List<Users.ResponseData>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string sql = "SELECT * FROM AFFEvents.dbo.AFFEVENT_REGISTRATION";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                    Users.ResponseData user = new Users.ResponseData
                                    {
                                        FULLNAME = reader["FullName"].ToString(),
                                        EMAIL = reader["EMAIL"].ToString(),
                                        PHONENUMBER = reader["PHONENUMBER"].ToString(),
                                        GENDER = reader["GENDER"].ToString(),
                                        ADDRESS = reader["ADDRESS"].ToString(),
                                        EVENTNAME = reader["EVENTNAME"].ToString(),
                                        PAID = reader["PAID"].ToString(),
                                    
                                };
                                usersList.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"Error Occurred: {ex.Message}");
                return null;
            }

            return usersList;
        }



        public async Task<List<Users>> GetFemaleUsers()
        {
            List<Users> usersList = new List<Users>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string sql = $"SELECT * FROM AFFEvents.dbo.AFFEVENT_REGISTRATION where GENDER = 'FEMALE'";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Users user = new Users
                                {
                                    response_data = new Users.ResponseData
                                    {
                                        FULLNAME = reader["FullName"].ToString(),
                                        EMAIL = reader["EMAIL"].ToString(),
                                        PHONENUMBER = reader["PHONENUMBER"].ToString(),
                                        GENDER = reader["GENDER"].ToString(),
                                        ADDRESS = reader["ADDRESS"].ToString(),
                                        EVENTNAME = reader["EVENTNAME"].ToString(),
                                        PAID = reader["PAID"].ToString()
                                    }
                                };

                                usersList.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"Error Occurred: {ex.Message}");
                return null;
            }

            return usersList;
        }


        public Users GetUserByEmail(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM AFFEvents.dbo.AFFEVENT_REGISTRATION WHERE EMAIL = @email";
                    SqlCommand command = new SqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@email", email);

                    SqlDataReader reader = command.ExecuteReader();

                    Users user = null;
                    if (reader.Read())
                    {
                        user = new Users
                        {
                            response_data = new Users.ResponseData
                            {
                                FULLNAME = reader["FullName"].ToString(),
                                EMAIL = reader["EMAIL"].ToString(),
                                PHONENUMBER = reader["PHONENUMBER"].ToString(),
                                GENDER = reader["GENDER"].ToString(),
                                ADDRESS = reader["ADDRESS"].ToString(),
                                EVENTNAME = reader["EVENTNAME"].ToString(),
                                PAID = reader["PAID"].ToString(),
                            }
                        };
                    }

                    reader.Close();

                    return user;
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"Error Occured:: {ex.Message}");
                return null;
            }
        }


        public Users CheckWithUniqueId(string Email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string sql = $"SELECT * FROM AFFEvents.dbo.AFFEVENT_REGISTRATION where EMAIL = '{Email}'";
                    SqlCommand command = new SqlCommand(sql, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    Users user = null;
                    if (reader.Read())
                    {
                        user = new Users
                        {
                            response_data = new Users.ResponseData
                            {
                                FULLNAME = reader["FullName"].ToString(),
                                EMAIL = reader["EMAIL"].ToString(),
                                PHONENUMBER = reader["PHONENUMBER"].ToString(),
                                UNIQUEID = reader["UNIQUEID"].ToString(),
                                GENDER = reader["GENDER"].ToString(),
                                ADDRESS = reader["ADDRESS"].ToString(),
                                EVENTNAME = reader["EVENTNAME"].ToString(),
                                PAID = reader["PAID"].ToString(),
                            }
                        };
                    }

                    reader.Close();

                    return user;
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"Error Occured:: {ex.Message}");
                return null;
            }
        }

        public Users CheckDuplicateTransref(string transRef)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string sql = $"SELECT * FROM AFFEvents.dbo.AFFEVENT_REGISTRATION where TRANSACTION_REFERENCE = '{transRef}'";
                    SqlCommand command = new SqlCommand(sql, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    Users user = null;
                    if (reader.Read())
                    {
                        user = new Users
                        {
                            response_data = new Users.ResponseData
                            {
                                FULLNAME = reader["FullName"].ToString(),
                                EMAIL = reader["EMAIL"].ToString(),
                                PHONENUMBER = reader["PHONENUMBER"].ToString(),
                                GENDER = reader["GENDER"].ToString(),
                                ADDRESS = reader["ADDRESS"].ToString(),
                                EVENTNAME = reader["EVENTNAME"].ToString(),
                                PAID = reader["PAID"].ToString(),
                            }
                        };
                    }

                    reader.Close();

                    return user;
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"Error Occured:: {ex.Message}");
                return null;
            }
        }


        public Users CheckTransExists(string transRef)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string sql = $"SELECT * FROM AFFEvents.dbo.AFFEVENT_REGISTRATION where REQUEST_ID = '{transRef}'";
                    SqlCommand command = new SqlCommand(sql, conn);

                    SqlDataReader reader = command.ExecuteReader();

                    Users user = null;
                    if (reader.Read())
                    {
                        user = new Users
                        {
                            response_data = new Users.ResponseData
                            {
                                FULLNAME = reader["FullName"].ToString(),
                                EMAIL = reader["EMAIL"].ToString(),
                                PHONENUMBER = reader["PHONENUMBER"].ToString(),
                                GENDER = reader["GENDER"].ToString(),
                                ADDRESS = reader["ADDRESS"].ToString(),
                                EVENTNAME = reader["EVENTNAME"].ToString(),
                                PAID = reader["PAID"].ToString(),
                            }
                        };
                    }

                    reader.Close();

                    return user;
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"Error Occured:: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> InsertNewUser(InsertUserReq req)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    SqlCommand command = new SqlCommand("AFFEvents.dbo.SP_INSERT_REGISTRANT", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FULLNAME", req.FULLNAME);
                    command.Parameters.AddWithValue("@EMAIL", req.EMAIL);
                    command.Parameters.AddWithValue("@PHONENUMBER", req.PHONENUMBER);
                    command.Parameters.AddWithValue("@ADDRESS", req.ADDRESS);
                    command.Parameters.AddWithValue("@GENDER", req.GENDER);
                    command.Parameters.AddWithValue("@EVENTNAME", req.EVENTNAME);
                    command.Parameters.AddWithValue("@UNIQUEID", req.UNIQUEID);

                    await command.ExecuteNonQueryAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"Error Occured:: {ex.Message}");
                return false;
            }
        }

        public bool UpdateRequestId(string accountNo, string requestId, string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string sql = "UPDATE AFFEvents.dbo.AFFEVENT_REGISTRATION SET VIRTUAL_ACC_NUMBER = @accountNo, REQUEST_ID = @requestId WHERE EMAIL = @email";
                    SqlCommand command = new SqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@accountNo", accountNo);
                    command.Parameters.AddWithValue("@requestId", requestId);
                    command.Parameters.AddWithValue("@email", email);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"Error Occured:: {ex.Message}");
                return false;
            }
        }

        public bool UpdateTransRef(string transRef, string sessionId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string sql = "UPDATE AFFEvents.dbo.AFFEVENT_REGISTRATION SET TRANSACTION_REFERENCE = @transRef, SESSION_ID = @sessionId, PAID = 'Y' WHERE REQUEST_ID = @transRef";
                    SqlCommand command = new SqlCommand(sql, conn);

                    command.Parameters.AddWithValue("@transRef", transRef);
                    command.Parameters.AddWithValue("@sessionId", sessionId);

                    int rowsAffected = command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"Error Occured:: {ex.Message}");
                return false;
            }
        }

        public AuthenticationResp CheckIfAuthorized(string channelCode, string authorization, string operationName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM AFFEvents.dbo.AFFEVENT_ChannelsAllowed WHERE channel_code = @channelCode AND auth_key = @authorization AND method_reqd = @operationName AND channel_allowed = 'Y'";
                    SqlCommand command = new SqlCommand(sql, conn);
                    command.Parameters.AddWithValue("@channelCode", channelCode);
                    command.Parameters.AddWithValue("@authorization", authorization);
                    command.Parameters.AddWithValue("@operationName", operationName);

                    SqlDataReader reader = command.ExecuteReader();

                    AuthenticationResp authorized = null;
                    if (reader.Read())
                    {
                        authorized = new AuthenticationResp
                        {
                            CHANNEL_CODE = reader["CHANNEL_CODE"].ToString(),
                            CHANNEL_ALLOWED = reader["CHANNEL_ALLOWED"].ToString(),
                        };
                    }

                    reader.Close();

                    return authorized;
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"Error Occured:: {ex.Message}");
                return null;
            }
        }


    }
}
