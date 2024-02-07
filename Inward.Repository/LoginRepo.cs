using Dapper;
using Inward.Entity;
using Inward.Repository.Abstraction;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inward.Repository
{
    public class LoginRepo : BaseRepository<BaseDataTableEntity>, ILoginRepo
    {
        public IConfiguration appConfig;

        public LoginRepo(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<UserMaster> AuthenticateUser(UserMaster userMaster)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@UserName", userMaster.UserName);
                    queryParameters.Add("@Password", userMaster.Password);
                    return await conn.QueryFirstOrDefaultAsync<UserMaster>(StoreProcedures.UserAuthenticationByCredentials, queryParameters, commandType: CommandType.StoredProcedure);
                }
            }catch (Exception ex) {

                throw ex;
            
            }
     
        }

        public async Task<FarmerEntity> GetFarmerDetailsById(string farmerid)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@farmerid", farmerid);
                    return await conn.QueryFirstOrDefaultAsync<FarmerEntity>(StoreProcedures.GetFarmerDetailsById, queryParameters, commandType: CommandType.StoredProcedure);
                }
            }catch (Exception ex) {

                throw ex;
            
            }
     
        }    
        
        public async Task<InwardEntity> GetInwardsById(string InwardId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    InwardEntity travelRequestDetails = new InwardEntity();
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@InwardId", InwardId);

                    var result = (await conn.QueryAsync<InwardEntity>(StoreProcedures.GetInwardsById, queryParameters, commandType: CommandType.StoredProcedure)).ToList();
                    if (result != null && result.Count() > 0)
                    {
                        travelRequestDetails.InwardId = result.Select(x => x.InwardId).FirstOrDefault();
                        travelRequestDetails.InwardNo = result.Select(x => x.InwardNo).FirstOrDefault();
                        travelRequestDetails.InwardDate = result.Select(x => x.InwardDate).FirstOrDefault();
                      
                        //travelRequestDetails.GradeDetails = result.Where(m => m.Reg_TravelRequestTypeId == "1")
                        //    .Select(m => new GradeDetail
                        //    {
                        //        GradeName = m.gradenam,
                        //        TotalWeight = m.TotalWeight,
                        //        AuctionRate = m.AuctionRate,
                        //        VendorCode = m.VendorCode,
                        //        UnitName = m.UnitName,
                        //        TotalCarat = m.TotalCarat,
                        //        VendorRate = m.VendorRate,
                        //        NetAmount = m.NetAmount,
                        //    }).ToList();

    }
                    else
                    {
                        return null;
                    }
                    return travelRequestDetails;
                }
            }
            catch (Exception ex) {

                throw ex;
            
            }
     
        }

        public async Task<ResponseMessage> InsertInwardDetail(InwardEntity inwardEntity, DataTable dtGradeData)
        {
            try
            {

                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    if (dtGradeData != null && dtGradeData.Rows.Count > 0)
                    {
                        dtGradeData.AcceptChanges();
                        queryParameters.Add("@InsertGrade", dtGradeData.AsTableValuedParameter("dtGradeDetailType"));
                    }
            
                    //queryParameters.Add("@InwardId", inwardEntity.InwardId);
                    queryParameters.Add("@Inward_No", inwardEntity.InwardNo);
                    queryParameters.Add("@Inward_Date", inwardEntity.InwardDate);
                    queryParameters.Add("@Farmer_Id", inwardEntity.Farmer_Id);
                    queryParameters.Add("@UserId", inwardEntity.UserId);

                    return  await conn.QueryFirstOrDefaultAsync<ResponseMessage>(StoreProcedures.InsertInwardDetail, queryParameters, commandType: CommandType.StoredProcedure);
                     
                }
            }
            catch (Exception ex) {

                throw ex;
            
            }
     
        }

        public async Task<List<FarmerEntity>> FillFarmer()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
        
                    var result = await conn.QueryAsync<FarmerEntity>(StoreProcedures.FillFarmer, queryParameters, commandType: CommandType.StoredProcedure);

                    return result.ToList();
                }
            }catch (Exception ex) {

                throw ex;
            
            }
     
        }
        public async Task<List<FarmerEntity>> FillGrade()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
        
                    var result = await conn.QueryAsync<FarmerEntity>(StoreProcedures.FillGrade, queryParameters, commandType: CommandType.StoredProcedure);

                    return result.ToList();
                }
            }catch (Exception ex) {

                throw ex;
            
            }
     
        }
        public async Task<List<InwardEntity>> GetInwardList()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
        
                    var result = await conn.QueryAsync<InwardEntity>(StoreProcedures.GetInwardList, queryParameters, commandType: CommandType.StoredProcedure);

                    return result.ToList();
                }
            }catch (Exception ex) {

                throw ex;
            
            }
     
        }
        public async Task<InwardEntity> GetLastInwardNo()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
        
                    return await conn.QueryFirstAsync<InwardEntity>(StoreProcedures.GetLastInwardNo, queryParameters, commandType: CommandType.StoredProcedure);

                }
            }catch (Exception ex) {

                throw ex;
            
            }
     
        }
        public async Task<List<FarmerEntity>> FillUnit(string gradeId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Grade_Id", gradeId);

                    var result = await conn.QueryAsync<FarmerEntity>(StoreProcedures.FillUnit, queryParameters, commandType: CommandType.StoredProcedure);

                    return result.ToList();
                }
            }catch (Exception ex) {

                throw ex;
            
            }
     
        }

    }
}
