using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inward.Entity;
using Inward.Repository.Abstraction;

namespace GCM.Repository.Abstraction
{
    public interface IStudent : IBaseRepository<BaseDataTableEntity>
    {
        Task<ResponseMessage> AddStudent(DataTable studentTable, long yearid,long semid,long userid);
    }
}
