using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inward.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCM.Services.Abstraction
{
    public interface IStudentService
    {
        Task<ResponseMessage> AddStudent(DataTable studentTable, long yearid);
    }
}
