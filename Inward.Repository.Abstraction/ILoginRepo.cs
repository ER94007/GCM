using GCM.Entity;
using Inward.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inward.Repository.Abstraction
{
    public interface ILoginRepo : IBaseRepository<BaseDataTableEntity>
    {
        Task<ResponseMessage> DeleteSubhead(long id);
        Task<SubHeadEntity> GetSubheadById(long id);
        Task<IEnumerable<SubHeadEntity>> GetSubHeadList();
        Task<ResponseMessage> AddUpdateSubhead(SubHeadEntity subHead);
        Task<ResponseMessage> DeleteStudent(long studentid);
        Task<Student> GetStudentByid(long studentid);
        Task<IEnumerable<Student>> GetStudentList();
        Task<List<SelectListItem>> BindGender();
        Task<List<SelectListItem>> BindCategory();
        Task<ResponseMessage> AddUpdateStudent(Student st);
        Task<UserMaster> AuthenticateUser(UserMaster userMaster);
        Task<FarmerEntity> GetFarmerDetailsById(string farmerId);
        Task<InwardEntity> GetInwardsById(string InwardId);

        Task<ResponseMessage> InsertInwardDetail(InwardEntity inwardEntity, DataTable dtGradeData);

        Task<List<FarmerEntity>> FillFarmer();
        Task<List<FarmerEntity>> FillGrade();
        Task<List<InwardEntity>> GetInwardList();
        Task<InwardEntity> GetLastInwardNo();
        Task<List<FarmerEntity>> FillUnit(string gradeid);

        Task<IEnumerable<StudentFeeDetailReport>> GetStudentFeeDetailReport(int YearId, int TermId);
		Task<IEnumerable<HeadMasterEntity>> GetHeadList();

		Task<ResponseMessage> Deletehead(long id);
		Task<HeadMasterEntity> GetheadById(long id);
		Task<ResponseMessage> AddUpdatehead(HeadMasterEntity Head);
        Task<IEnumerable<FeeCollectionDetailReport>> GetFeeCollectionDetailReport(string fromdate,string todate);
		Task<List<MenuViewModel>> GetMenusByUserIdAsync(int userid);
		Task<IEnumerable<StudentUpdate>> GetStudentForUpdate(int YearId, int TermId);

	}
}
