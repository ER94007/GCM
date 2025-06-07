using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inward.Entity;
using Inward.Repository.Abstraction;
using Inward.Repository;
using GCM.Repository.Abstraction;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;

namespace GCM.Repository
{
    public class StudentRepo : BaseRepository<BaseDataTableEntity>, IStudent
    {
        public IConfiguration appConfig;

        public StudentRepo(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }

        //public async Task<ResponseMessage> AddStudent(DataTable studentTable)
        //{
        //    try
        //    {
        //        using (var conn = GetConnection())
        //        {
        //            var queryParameters = new DynamicParameters();

        //            // Add the DataTable as a TVP parameter (SQL Server's table type)
        //            queryParameters.Add("@Students", studentTable.AsTableValuedParameter("dbo.StudentTableType"));

        //            // Call the stored procedure
        //            return await conn.QueryFirstOrDefaultAsync<ResponseMessage>(StoreProcedures.AddStudentExcel, queryParameters,commandType: CommandType.StoredProcedure);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception and rethrow it
        //        throw new Exception("Error adding students", ex);
        //    }
        //}

        public async Task<ResponseMessage> AddStudent(DataTable studentTable, long yearid, long semid, long userid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();

                    // Add the DataTable as a TVP parameter (SQL Server's table type)
                    queryParameters.Add("@Students", studentTable.AsTableValuedParameter("dbo.StudentTableType"));
                    queryParameters.Add("@FinancialYearid", yearid);
                    queryParameters.Add("@semid", semid);
                    queryParameters.Add("@userid", userid);

                    // Define the output parameter for result (1 or 0)
                    queryParameters.Add("@ResultId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    // Call the stored procedure
                    await conn.ExecuteAsync("dbo.AddStudentExcel", queryParameters, commandType: CommandType.StoredProcedure);

                    // Map the result to ResponseMessage
                    var result = new ResponseMessage
                    {
                        Id = queryParameters.Get<int>("@ResultId") // Get ResultId (1 for success, 0 for failure)
                    };

                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow it
                throw new Exception("Error adding students", ex);
            }
        }

        public async Task<ResponseMessage> UpdateEnrollment (DataTable updatestudentTable, long userid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();

                    // Add the DataTable as a TVP parameter (SQL Server's table type)
                    queryParameters.Add("@StudentUpdates", updatestudentTable.AsTableValuedParameter("StudentUpdateTableType"));
                    queryParameters.Add("@UserId", userid);

                    // Define the output parameter for result (1 or 0)
                    queryParameters.Add("@ResultId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    // Call the stored procedure
                    await conn.ExecuteAsync("UpdateStudentEnrollmentNumbers", queryParameters, commandType: CommandType.StoredProcedure);

                    // Map the result to ResponseMessage
                    var result = new ResponseMessage
                    {
                        Id = queryParameters.Get<int>("@ResultId") // Get ResultId (1 for success, 0 for failure)
                    };

                    return result;
                }
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow it
                throw new Exception("Error adding students", ex);
            }
        }
    }
}
    