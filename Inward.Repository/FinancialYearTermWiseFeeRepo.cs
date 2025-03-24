using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inward.Entity;
using Inward.Repository.Abstraction;
using Inward.Repository;
using Microsoft.Extensions.Configuration;
using GCM.Repository.Abstraction;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dapper;
using System.Data;
using GCM.Entity;

namespace GCM.Repository
{
    public class FinancialYearTermWiseFeeRepo : BaseRepository<BaseDataTableEntity>, IFinancialYearTermWiseFeeRepo
    {
        public IConfiguration appConfig;

        public FinancialYearTermWiseFeeRepo(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<ResponseMessage> DeleteFinanceYearTerm(long id)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@FinancialYearWiseTermWiseFeeDetailid", id);
                    
                    var res = await conn.QueryFirstAsync<ResponseMessage>(StoreProcedures.DeleteFinanceData, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ResponseMessage> UpdateFinanceData(FinancialYearTermWiseFeeEntity fn)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@FinancialYearWiseTermWiseFeeDetailid", fn.FinancialYearWiseTermWiseFeeDetailid);
                    queryParameters.Add("@FinancialYearId", fn.FinancialYearId);
                    queryParameters.Add("@TermId", fn.TermId);
                    queryParameters.Add("@SubHeadId", fn.SubHeadId);
                    queryParameters.Add("@MaleFee", fn.malefee);
                    queryParameters.Add("@FemaleFee", fn.femalefee);
                    
                    var res = await conn.QueryFirstAsync<ResponseMessage>(StoreProcedures.UpdateFinanceData, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<FinancialYearTermWiseFeeEntity> GetFinanceDataById(long id)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@FinancialYearWiseTermWiseFeeDetailid", id);
                    var res = await conn.QueryFirstOrDefaultAsync<FinancialYearTermWiseFeeEntity>(StoreProcedures.GetFinancialDataById, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<FinancialYearTermWiseFeeEntity>> GetFinanceData()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    var res = await conn.QueryAsync<FinancialYearTermWiseFeeEntity>(StoreProcedures.GetFinanceData, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ResponseMessage> AddFinancialYearTermFee(FinancialYearTermWiseFeeEntity financialYearTermWiseFeeEntity)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@FinancialYearId", financialYearTermWiseFeeEntity.FinancialYearId);
                    //queryParameters.Add("@TermId", financialYearTermWiseFeeEntity.TermId);
                    queryParameters.Add("@dt", financialYearTermWiseFeeEntity.dt.AsTableValuedParameter("dbo.FinancialYearTermWiseFeeType"));
                    var res = await conn.QueryFirstAsync<ResponseMessage>(StoreProcedures.AddFinancialYearTermFee, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
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
        public async Task<List<SelectListItem>> BindYear()
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

    }
}
