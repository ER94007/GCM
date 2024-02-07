using Inward.Entity;
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
        Task<UserMaster> AuthenticateUser(UserMaster userMaster);
        Task<FarmerEntity> GetFarmerDetailsById(string farmerId);
        Task<InwardEntity> GetInwardsById(string InwardId);

        Task<ResponseMessage> InsertInwardDetail(InwardEntity inwardEntity, DataTable dtGradeData);

        Task<List<FarmerEntity>> FillFarmer();
        Task<List<FarmerEntity>> FillGrade();
        Task<List<InwardEntity>> GetInwardList();
        Task<InwardEntity> GetLastInwardNo();
        Task<List<FarmerEntity>> FillUnit(string gradeid);

    }
}
