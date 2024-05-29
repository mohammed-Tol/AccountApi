using AccountsApiService.DataAccess;
using AccountsApiService.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Text;
using System.Xml;

namespace AccountsApiService.Infrastructure
{
    public class AccountsService : BaseDataAccess, IRepository<Account, long>
    {
        public AccountsService(IConfiguration config) : base(config)
        {

        }

        public long CreateAccount(Account entity)
        {
            var sqlText1 = "sp_AddAccount";   //stored procedure name
            try
            {
                var result = ExecuteNonQuery(sqlText1, commandType: CommandType.StoredProcedure, "@AccountID",
                    new SqlParameter("@Balance", entity.balance),
                    new SqlParameter("@HasCheckBook", entity.hasCheque),
                    new SqlParameter("@WdQuota", entity.wd_quota),
                    new SqlParameter("@DpQuota", entity.dp_quota),
                    new SqlParameter("@IsActive", entity.isActive),
                    new SqlParameter("@CustId", entity.customerID),
                    new SqlParameter("@TypeId", entity.type_id),
                    new SqlParameter("@BranchId", entity.branchID),
                    new SqlParameter("@AccountID", entity.accountID) { Direction=ParameterDirection.Output }
                );
                return result;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return 0;
        }

        public void DeleteAccountByAccountId(long Accid)
        {
            var sqlText1 = "sp_DeleteAccountSafely";   //stored procedure name
            try
            {
                ExecuteNonQuery(sqlText1, commandType: CommandType.StoredProcedure, new SqlParameter("@AccountID", Accid)
                );
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }

        public Account GetAccountByAccountID(long Accid)
        {
            Account account = new Account();
            var sqlText1 = "sp_GetCustomerAccount";   //stored procedure name
            try
            {
                using (SqlDataReader reader = ExecuteReader(sqlText1, CommandType.StoredProcedure,
            new SqlParameter("@AccountId", Accid)))
                {
                    // Check if the reader has any rows
                    if (reader.Read())
                    {
                        // Populate account object
                        account.accountID = (long)reader["AccountId"];
                        account.balance = (decimal)reader["Balance"];
                        account.hasCheque = (bool)reader["HasCheckBook"];
                        account.wd_quota = (int)reader["WdQuota"];
                        account.dp_quota = (int)reader["DpQuota"];
                        account.isActive = (bool)reader["IsActive"];
                        account.customerID = (int)reader["CustId"];
                        account.type_id = (int)reader["TypeId"];
                        account.branchID = (string)reader["BranchId"];
                    }
                }
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return account;
        }

        public IEnumerable<Account> GetAllAccountsByCustomerID(int CustomerID)
        {
            List<Account> accountsList = new List<Account>();
            var sqlText1 = "sp_GetCustomerAccounts";   //stored procedure name
            try
            {
                using (SqlDataReader reader = ExecuteReader(sqlText1, CommandType.StoredProcedure, new SqlParameter("@CustomerId", CustomerID)))
                {
                    // Check if the reader has any rows
                    if (reader.HasRows)
                    {
                        // Read each row and populate Account objects
                        while (reader.Read())
                        {
                            Account account = new Account();
                            account.accountID = (long)reader["AccountId"];
                            account.balance = (decimal)reader["Balance"];
                            account.hasCheque = (bool)reader["HasCheckBook"];
                            account.wd_quota = (int)reader["WdQuota"];
                            account.dp_quota = (int)reader["DpQuota"];
                            account.isActive = (bool)reader["IsActive"];
                            account.customerID = (int)reader["CustId"];
                            account.type_id = (int)reader["TypeId"];
                            account.branchID = (string)reader["BranchId"];

                            accountsList.Add(account);
                        }
                    }
                }
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
            return accountsList;
        }
        public void ApplyForChequeBook(long Accid)
        {
            var sqlText1 = "sp_ApplyForCheckBook";   //stored procedure name
            try
            {
                ExecuteNonQuery(sqlText1, commandType: CommandType.StoredProcedure, new SqlParameter("@accID", Accid)
                );
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}
