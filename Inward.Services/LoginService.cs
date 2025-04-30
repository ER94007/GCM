using GCM.Entity;
using Inward.Entity;
using Inward.Repository.Abstraction;
using Inward.Services.Abstraction;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inward.Services
{
	public class LoginService : ILoginService
	{
		private readonly ILoginRepo _LoginRepo;
		public LoginService(ILoginRepo userLoginRepo)
		{
			_LoginRepo = userLoginRepo ?? throw new ArgumentNullException(nameof(userLoginRepo));
		}
		public async Task<ResponseMessage> DeleteSubhead(long id)
		{
			return await _LoginRepo.DeleteSubhead(id);
		}
		public async Task<ResponseMessage> AddUpdateSubhead(SubHeadEntity subHead)
		{
			return await _LoginRepo.AddUpdateSubhead(subHead);

		}
		public async Task<SubHeadEntity> GetSubheadById(long id)
		{
			return await _LoginRepo.GetSubheadById(id);
		}
		public async Task<IEnumerable<SubHeadEntity>> GetSubHeadList()
		{
			return await _LoginRepo.GetSubHeadList();
		}


		public async Task<ResponseMessage> DeleteStudent(long studentid)
		{
			return await _LoginRepo.DeleteStudent(studentid);
		}
		public async Task<Student> GetStudentByid(long studentid)
		{
			return await _LoginRepo.GetStudentByid(studentid);
		}
		public async Task<ResponseMessage> AddUpdateStudent(Student st)
		{
			return await _LoginRepo.AddUpdateStudent(st);
		}

		public async Task<IEnumerable<Student>> GetStudentList()
		{
			return await _LoginRepo.GetStudentList();
		}
		public async Task<List<SelectListItem>> BindGender()
		{
			return await _LoginRepo.BindGender();
		}
		public async Task<List<SelectListItem>> BindCategory()
		{
			return await _LoginRepo.BindCategory();
		}
		public async Task<UserMaster> AuthenticateUser(UserMaster userMaster)
		{
			return await _LoginRepo.AuthenticateUser(userMaster);
		}
		public async Task<FarmerEntity> GetFarmerDetailsById(string farmerid)
		{
			return await _LoginRepo.GetFarmerDetailsById(farmerid);
		}
		public async Task<InwardEntity> GetInwardsById(string InwardId)
		{
			return await _LoginRepo.GetInwardsById(InwardId);
		}

		public async Task<ResponseMessage> InsertInwardDetail(InwardEntity inwardEntity, DataTable dtGradeData)
		{
			return await _LoginRepo.InsertInwardDetail(inwardEntity, dtGradeData);
		}


		public async Task<List<FarmerEntity>> FillFarmer()
		{
			return await _LoginRepo.FillFarmer();
		}
		public async Task<List<FarmerEntity>> FillGrade()
		{
			return await _LoginRepo.FillGrade();
		}
		public async Task<List<InwardEntity>> GetInwardList()
		{
			return await _LoginRepo.GetInwardList();
		}
		public async Task<InwardEntity> GetLastInwardNo()
		{
			return await _LoginRepo.GetLastInwardNo();
		}
		public async Task<List<FarmerEntity>> FillUnit(string gradeId)
		{
			return await _LoginRepo.FillUnit(gradeId);
		}
		public async Task<IEnumerable<StudentFeeDetailReport>> GetStudentFeeDetailReport(int YearId, int TermId)
		{
			return await _LoginRepo.GetStudentFeeDetailReport(YearId, TermId);
		}
		public async Task<IEnumerable<HeadMasterEntity>> GetHeadList()
		{
			return await _LoginRepo.GetHeadList();
		}
		public async Task<ResponseMessage> Deletehead(long id)
		{
			return await _LoginRepo.Deletehead(id);
		}

		public async Task<HeadMasterEntity> GetheadById(long id)
		{
			return await _LoginRepo.GetheadById(id);
		}

		public async Task<ResponseMessage> AddUpdatehead(HeadMasterEntity Head)
		{
			return await _LoginRepo.AddUpdatehead(Head);
		}
	}
}
