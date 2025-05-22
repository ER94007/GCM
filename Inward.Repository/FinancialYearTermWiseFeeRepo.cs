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
        public async Task<ResponseMessage> UpdateCheque(ChequeMaster ep)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ChequeMasterID", ep.ChequeMasterID);
                    queryParameters.Add("@chequeno", ep.chequeno);
                    queryParameters.Add("@remarks", ep.remarks);
                    queryParameters.Add("@Hostname", ep.hostname);
                    queryParameters.Add("@Ipaddress", ep.ipaddress);
                    queryParameters.Add("@TransactionId", ep.transactionid);
                    queryParameters.Add("@UserId", ep.userid);
                    var res = await conn.QueryFirstAsync<ResponseMessage>(StoreProcedures.UpdateCheque, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ResponseMessage> AddChequeNo(ChequeMaster ep)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@chequesdt", ep.dt.AsTableValuedParameter("gsm.ChequeNoListType"));
                    queryParameters.Add("@Hostname", ep.hostname);
                    queryParameters.Add("@Ipaddress", ep.ipaddress);
                    queryParameters.Add("@TransactionId", ep.transactionid);
                    queryParameters.Add("@UserId", ep.userid);
                    var res = await conn.QueryFirstAsync<ResponseMessage>(StoreProcedures.AddChequeNo, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ChequeMaster>> GetChequeDate()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    var res = await conn.QueryAsync<ChequeMaster>(StoreProcedures.GetChequeDate, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<ExpenseEntity>> GetExpenseData()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    var res = await conn.QueryAsync<ExpenseEntity>(StoreProcedures.GetExpenseData, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<long> GetBalanceData(long id1, long id2)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Subheadid", id1);
                    queryParameters.Add("@FinancialYearId", id2);
                    var res = await conn.QueryFirstAsync<long>(StoreProcedures.GetBalanceData, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ResponseMessage> AddExpense(ExpenseEntity ep)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@FinancialYearId", ep.FinancialYearId);
                    queryParameters.Add("@SubHeadId", ep.SubHeadId);
                    queryParameters.Add("@amount", ep.amount);
                    queryParameters.Add("@Remarks", ep.Remarks);
                    queryParameters.Add("@ExpenseType", ep.ExpenseType);
                    queryParameters.Add("@TransactionDate", ep.DateofExpense.ToString());
                    if (ep.ExpenseType == "Expense")
                    {
                        queryParameters.Add("@ChequeNo", ep.ChequeNo);
                    }
                    else
                    {
                        queryParameters.Add("@ChequeNo", 0);
                    }
                    queryParameters.Add("@Hostname", ep.hostname);
                    queryParameters.Add("@Ipaddress", ep.ipaddress);
                    queryParameters.Add("@TransactionId", ep.transactionid);
                    queryParameters.Add("@UserId", ep.userid);
                    var res = await conn.QueryFirstAsync<ResponseMessage>(StoreProcedures.AddExpense, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseMessage> CheckExpenseForCheque(long id)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ChequeMasterID", id);

                    var res = await conn.QueryFirstAsync<ResponseMessage>(StoreProcedures.CheckExpenseForCheque, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ChequeMaster> GetChequeDateById(long id)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@ChequeMasterId", id);

                    var res = await conn.QueryFirstAsync<ChequeMaster>(StoreProcedures.GetChequeDateById, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ResponseMessage> DeleteFinanceBalance(long id, long id2, string name1, string name2)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@FinancialYearBalanceId", id);
                    queryParameters.Add("@UserId", id2);
                    queryParameters.Add("@Hostname", name1);
                    queryParameters.Add("@Ipaddress", name2);
                    var res = await conn.QueryFirstAsync<ResponseMessage>(StoreProcedures.DeleteBalanceData, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ResponseMessage> UpdateFinanceBalance(FinanceBalanceEntity fn)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@FinancialYearBalanceId", fn.FinancialYearBalanceId);
                    queryParameters.Add("@FinancialYearId", fn.FinancialYearId);
                    queryParameters.Add("@SubheadId", fn.SubHeadId);
                    queryParameters.Add("@amount", fn.amount);
                    queryParameters.Add("@Hostname", fn.hostname);
                    queryParameters.Add("@Ipaddress", fn.ipaddress);
                    queryParameters.Add("@TransactionId", fn.transactionid);
                    queryParameters.Add("@UserId", fn.userid);
                    var res = await conn.QueryFirstAsync<ResponseMessage>(StoreProcedures.UpdateFinanceBalanceData, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<FinanceBalanceEntity> GetBalanceDataById(long id)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@FinancialYearBalanceId", id);
                    var res = await conn.QueryFirstOrDefaultAsync<FinanceBalanceEntity>(StoreProcedures.GetBalanceDataById, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<FinanceBalanceEntity>> GetFinanceBalanceData()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    var res = await conn.QueryAsync<FinanceBalanceEntity>(StoreProcedures.GetFinanceBalanceData, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ResponseMessage> AddFinanceYearBalance(FinanceBalanceEntity ft)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@FinancialYearId", ft.FinancialYearId);
                    queryParameters.Add("@Hostname", ft.hostname);
                    queryParameters.Add("@Ipaddress", ft.ipaddress);
                    queryParameters.Add("@TransactionId", ft.transactionid);
                    queryParameters.Add("@UserId", ft.userid);
                    //queryParameters.Add("@TermId", financialYearTermWiseFeeEntity.TermId);
                    queryParameters.Add("@dt", ft.fdt.AsTableValuedParameter("gsm.SubHeadAmountBalance"));
                    var res = await conn.QueryFirstAsync<ResponseMessage>(StoreProcedures.AddFinanceBalance, queryParameters, commandType: CommandType.StoredProcedure);
                    return res;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ResponseMessage> DeleteFinanceYearTerm(long id, long id2 , string name1, string name2)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@FinancialYearWiseTermWiseFeeDetailid", id);
                    queryParameters.Add("@UserId", id2);
                    queryParameters.Add("@Hostname", name1);
                    queryParameters.Add("@Ipaddress", name2);
                    
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
                    queryParameters.Add("@Hostname", fn.hostname);
                    queryParameters.Add("@Ipaddress", fn.ipaddress);
                    queryParameters.Add("@TransactionId", fn.transactionid);
                    queryParameters.Add("@UserId", fn.userid);

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
                    queryParameters.Add("@Hostname", financialYearTermWiseFeeEntity.hostname);
                    queryParameters.Add("@Ipaddress", financialYearTermWiseFeeEntity.ipaddress);
                    queryParameters.Add("@TransactionId", financialYearTermWiseFeeEntity.transactionid);
                    queryParameters.Add("@UserId", financialYearTermWiseFeeEntity.userid);

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
        public async Task<List<SelectListItem>> BindCheques()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    var res = await conn.QueryAsync<dynamic>(StoreProcedures.BindCheques, queryParameters, commandType: CommandType.StoredProcedure);
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
		public async Task<List<SelectListItem>> BindHeads()
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					var res = await conn.QueryAsync<dynamic>(StoreProcedures.BindHeads, queryParameters, commandType: CommandType.StoredProcedure);
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

	}
}
