using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCM.Entity;
using Inward.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCM.Services.Abstraction
{
    public interface IStudentFeeCollectionService
    {
        Task<IEnumerable<Student>> GetStudentList();
        Task<List<SelectListItem>> BindFinancialYear();
        Task<List<SelectListItem>> BindStudents(long id);
        Task<List<SelectListItem>> BindTerm();
        Task<List<SelectListItem>> BindSubhead();
        Task<IEnumerable<StudentFeeCollection>> FeeDetails(int termId, int financialYearId, int studentid);
        Task<ResponseMessage> AddFeeCollection(StudentFeeCollection model,decimal fees);
        Task<IEnumerable<StudentFeeCollection>> GetStudentFeeCollectionList();
        Task<IEnumerable<StudentFeeCollection>> GetReport_studentFeeMaster(long studentid, long id2, long id3,string recieptno);
    }
}
