using Dapper;
using GCM.Entity;
using Inward.Entity;
using Inward.Repository.Abstraction;
using Microsoft.AspNetCore.Mvc.Rendering;
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
		public async Task<ResponseMessage> DeleteSubhead(long subheadid)
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					queryParameters.Add("@SubheadId", subheadid);
					return await conn.QueryFirstOrDefaultAsync<ResponseMessage>(StoreProcedures.DeleteSubhead, queryParameters, commandType: CommandType.StoredProcedure);

				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<SubHeadEntity> GetSubheadById(long id)
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					queryParameters.Add("@SubheadId", id);
					var res = await conn.QueryFirstOrDefaultAsync<SubHeadEntity>(StoreProcedures.GetSubheadById, queryParameters, commandType: CommandType.StoredProcedure);
					return res;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<IEnumerable<SubHeadEntity>> GetSubHeadList()
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					var res = await conn.QueryAsync<SubHeadEntity>(StoreProcedures.GetSubheadList, queryParameters, commandType: CommandType.StoredProcedure);
					return res;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<ResponseMessage> AddUpdateSubhead(SubHeadEntity sh)
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					queryParameters.Add("@SubHeadName", sh.subheadname);
					queryParameters.Add("@SubHeadType", sh.subheadtype);
					queryParameters.Add("@SubHeadId", sh.subheadid);
					queryParameters.Add("@HeadMasterId", sh.HeadMasterId);
					queryParameters.Add("@UserId", sh.UserId);
					return await conn.QueryFirstOrDefaultAsync<ResponseMessage>(StoreProcedures.AddUpdateSubhead, queryParameters, commandType: CommandType.StoredProcedure);

				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<Student> GetStudentByid(long studentid)
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					queryParameters.Add("@studentid", studentid);
					var res = await conn.QueryFirstOrDefaultAsync<Student>(StoreProcedures.GetStudentById, queryParameters, commandType: CommandType.StoredProcedure);
					return res;
				}
			}
			catch (Exception)
			{

				throw;
			}
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
		public async Task<List<SelectListItem>> BindGender()
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					var res = await conn.QueryAsync<dynamic>(StoreProcedures.BindGender, queryParameters, commandType: CommandType.StoredProcedure);
					var genderList = res.Select(item => new SelectListItem
					{
						Value = Convert.ToString(item.Value),  // Extract 'parameterid'
						Text = item.Text // Extract 'parametername'
					}).ToList();
					return genderList;
				}
			}
			catch (Exception ex)
			{

				throw ex;

			}
		}

		public async Task<List<SelectListItem>> BindCategory()
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					var res = await conn.QueryAsync<dynamic>(StoreProcedures.BindCategory, queryParameters, commandType: CommandType.StoredProcedure);
					var categoryList = res.Select(item => new SelectListItem
					{
						Value = Convert.ToString(item.Value),  // Extract 'parameterid'
						Text = item.Text // Extract 'parametername'
					}).ToList();
					return categoryList;
				}
			}
			catch (Exception ex)
			{

				throw ex;

			}
		}
		public async Task<ResponseMessage> DeleteStudent(long studentid)
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					queryParameters.Add("@studentid", studentid);
					return await conn.QueryFirstOrDefaultAsync<ResponseMessage>(StoreProcedures.DeleteStudent, queryParameters, commandType: CommandType.StoredProcedure);

				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<ResponseMessage> AddUpdateStudent(Student st)
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					queryParameters.Add("@studentid", st.studentid);
					queryParameters.Add("@studentname", st.name);
					queryParameters.Add("@mobileno", st.mobileno);
					queryParameters.Add("@email", st.email);
					queryParameters.Add("@genderid", st.genderid);
					queryParameters.Add("@categoryid", st.categoryid);
					queryParameters.Add("@applicationno", st.applicationno);
					queryParameters.Add("@enrollmentno", st.enrolmentno);
					queryParameters.Add("@FinancialYearId", st.FinancialYearId);  
					queryParameters.Add("@termid", st.termid);  
					queryParameters.Add("@UserId", st.userid);  
					return await conn.QueryFirstOrDefaultAsync<ResponseMessage>(StoreProcedures.AddUpdateStudent, queryParameters, commandType: CommandType.StoredProcedure);

				}
			}
			catch (Exception ex)
			{

				throw ex;

			}
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
			}
			catch (Exception ex)
			{

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
			}
			catch (Exception ex)
			{

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
			catch (Exception ex)
			{

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

					return await conn.QueryFirstOrDefaultAsync<ResponseMessage>(StoreProcedures.InsertInwardDetail, queryParameters, commandType: CommandType.StoredProcedure);

				}
			}
			catch (Exception ex)
			{

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
			}
			catch (Exception ex)
			{

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
			}
			catch (Exception ex)
			{

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
			}
			catch (Exception ex)
			{

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
			}
			catch (Exception ex)
			{

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
			}
			catch (Exception ex)
			{

				throw ex;

			}


		}
		public async Task<IEnumerable<StudentFeeDetailReport>> GetStudentFeeDetailReport(int YearId, int TermId)
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					queryParameters.Add("@YearId", YearId);
					queryParameters.Add("@TermId", TermId);

					var res = await conn.QueryAsync<StudentFeeDetailReport>(StoreProcedures.GetStudentFeeDetailReport, queryParameters, commandType: CommandType.StoredProcedure);
					return res;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<IEnumerable<HeadMasterEntity>> GetHeadList()
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					var res = await conn.QueryAsync<HeadMasterEntity>(StoreProcedures.GetHeadList, queryParameters, commandType: CommandType.StoredProcedure);
					return res;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<ResponseMessage> Deletehead(long subheadid)
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					queryParameters.Add("@HeadMasterId", subheadid);
					return await conn.QueryFirstOrDefaultAsync<ResponseMessage>(StoreProcedures.Deletehead, queryParameters, commandType: CommandType.StoredProcedure);

				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<HeadMasterEntity> GetheadById(long id)
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					queryParameters.Add("@HeadMasterId", id);
					var res = await conn.QueryFirstOrDefaultAsync<HeadMasterEntity>(StoreProcedures.GetheadById, queryParameters, commandType: CommandType.StoredProcedure);
					return res;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<ResponseMessage> AddUpdatehead(HeadMasterEntity sh)
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					queryParameters.Add("@HeadName", sh.HeadName);
					queryParameters.Add("@HeadMasterId", sh.HeadMasterId);
					queryParameters.Add("@IsStudentRelated", sh.IsStudentRelated);
					queryParameters.Add("@IsCombined", sh.IsCombined);
					queryParameters.Add("@UserId", sh.UserId);
					return await conn.QueryFirstOrDefaultAsync<ResponseMessage>(StoreProcedures.AddUpdateHead, queryParameters, commandType: CommandType.StoredProcedure);

				}
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<IEnumerable<FeeCollectionDetailReport>> GetFeeCollectionDetailReport(string fromdate, string todate)
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					queryParameters.Add("@fromdate", fromdate);
					queryParameters.Add("@todate", todate);

					var res = await conn.QueryAsync<FeeCollectionDetailReport>(StoreProcedures.GetFeeCollectionDetailReport, queryParameters, commandType: CommandType.StoredProcedure);
					return res;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public async Task<List<MenuViewModel>> GetMenusByUserIdAsync(int userid)
		{
			try
			{
				using (var conn = GetConnection())
				{
					var queryParameters = new DynamicParameters();
					queryParameters.Add("@UserId", userid);



					var result = await conn.QueryAsync<MenuViewModel>(
				StoreProcedures.GetMenusByUserId,
				queryParameters,
				commandType: CommandType.StoredProcedure
			);

					return result.ToList();
				}
			}
			catch (Exception)
			{

				throw;
			}
		}

        public async Task<IEnumerable<StudentUpdate>> GetStudentForUpdate(int YearId, int TermId)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@YearId", YearId);
                    queryParameters.Add("@TermId", TermId);

                    var res = await conn.QueryAsync<StudentUpdate>(StoreProcedures.GetStudentsByYearAndSemester, queryParameters, commandType: CommandType.StoredProcedure);
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
