using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GCM.Entity;
using GCM.Repository.Abstraction;
using Inward.Entity;
using Inward.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GCM.Repository
{
    public class StudentFeeCollectionRepo : BaseRepository<BaseDataTableEntity>, IStudentFeeCollectionRepo
    {
        public IConfiguration appConfig;
        public StudentFeeCollectionRepo(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<IEnumerable<Student>> GetStudentList()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    var res = await conn.QueryAsync<Student>(StoreProcedures.GetStudents, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<SelectListItem>> BindStudents(long id)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@FinancialyearId", id);
                    var res = await conn.QueryAsync<dynamic>(StoreProcedures.BindStudents, queryParameters, commandType: CommandType.StoredProcedure);
                    var termList = res.Select(item => new SelectListItem
                    {
                        Value = Convert.ToString(item.Value),  // Extract 'parameterid'
                        Text = item.Text // Extract 'parametername'
                    }).ToList();
                    return termList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<SelectListItem>> BindTerm()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    var res = await conn.QueryAsync<dynamic>(StoreProcedures.BindTerm, queryParameters, commandType: CommandType.StoredProcedure);
                    var termList = res.Select(item => new SelectListItem
                    {
                        Value = Convert.ToString(item.Value),  // Extract 'parameterid'
                        Text = item.Text // Extract 'parametername'
                    }).ToList();
                    return termList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<SelectListItem>> BindSubhead()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    var res = await conn.QueryAsync<dynamic>(StoreProcedures.BindSubhead, queryParameters, commandType: CommandType.StoredProcedure);
                    var subheadList = res.Select(item => new SelectListItem
                    {
                        Value = Convert.ToString(item.Value),  // Extract 'parameterid'
                        Text = item.Text // Extract 'parametername'
                    }).ToList();
                    return subheadList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<SelectListItem>> BindFinancialYear()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    var res = await conn.QueryAsync<dynamic>(StoreProcedures.BindYear, queryParameters, commandType: CommandType.StoredProcedure);
                    var yearList = res.Select(item => new SelectListItem
                    {
                        Value = Convert.ToString(item.Value),  // Extract 'parameterid'
                        Text = item.Text // Extract 'parametername'
                    }).ToList();
                    return yearList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<StudentFeeCollection>> FeeDetails(int termId, int financialYearId, int studentId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@TermId", termId, DbType.Int32);
                    queryParameters.Add("@FinancialYearId", financialYearId, DbType.Int32);
                    queryParameters.Add("@StudentId", studentId, DbType.Int32);

                    var res = await conn.QueryAsync<StudentFeeCollection>(
                        StoreProcedures.GetFeeDetails,queryParameters,commandType: CommandType.StoredProcedure
                    );

                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseMessage> AddFeeCollection(StudentFeeCollection model,decimal amount)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@StudentId", model.StudentId);
                    queryParameters.Add("@FinancialYearId", model.FinancialYearId);
                    queryParameters.Add("@TermId", model.TermId);
                    queryParameters.Add("@TotalAmount", amount);
                    queryParameters.Add("@FeeMode", model.FormType);
                    queryParameters.Add("@CreatedBy", 1);
					var json = JsonConvert.SerializeObject(model.feesdetails);

					Console.WriteLine(json); // or log it

					queryParameters.Add("@FeeDetails", JsonConvert.SerializeObject(model.feesdetails));
					var res = await conn.QueryFirstOrDefaultAsync<ResponseMessage>(StoreProcedures.AddStudentFeeCollection, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;

				}
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }

        public async Task<IEnumerable<StudentFeeCollection>> GetStudentFeeCollectionList()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    var res = await conn.QueryAsync<StudentFeeCollection>(StoreProcedures.GetStudentFeeCollection, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<StudentFeeCollection>> GetReport_studentFeeMaster(long studentid, long id2, long id3, string RecieptNo)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@studentid", studentid);
                    queryParameters.Add("@FinancialYearId", id2);
                    queryParameters.Add("@TermId", id3);
                    queryParameters.Add("@RecieptNo", RecieptNo);
                    var res = await conn.QueryAsync<StudentFeeCollection>(StoreProcedures.GetReport_studentFeeMaster, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
