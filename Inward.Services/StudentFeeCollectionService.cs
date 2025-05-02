using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCM.Entity;
using GCM.Repository.Abstraction;
using GCM.Services.Abstraction;
using Inward.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GCM.Services
{
    public class StudentFeeCollectionService : IStudentFeeCollectionService
    {
         private readonly IStudentFeeCollectionRepo _studentFeeCollectionRepo;
        public StudentFeeCollectionService(IStudentFeeCollectionRepo studentFeeCollectionRepo)
        {
            _studentFeeCollectionRepo = studentFeeCollectionRepo ?? throw new ArgumentNullException(nameof(studentFeeCollectionRepo));
        }

        public async Task<IEnumerable<Student>> GetStudentList()
        {
            return await _studentFeeCollectionRepo.GetStudentList();
        }
        public async Task<List<SelectListItem>> BindStudents(long id)
        {
            return await _studentFeeCollectionRepo.BindStudents(id);
        }
        public async Task<List<SelectListItem>> BindFinancialYear()
        {
            return await _studentFeeCollectionRepo.BindFinancialYear();
        }
        public async Task<List<SelectListItem>> BindTerm()
        {
            return await _studentFeeCollectionRepo.BindTerm();
        }
        public async Task<List<SelectListItem>> BindReciept(long id)
        {
            return await _studentFeeCollectionRepo.BindReciept(id);
        }
        public async Task<List<SelectListItem>> BindSubhead()
        {
            return await _studentFeeCollectionRepo.BindSubhead();
        }
        public async Task<IEnumerable<StudentFeeCollection>> FeeDetails(int termId, int financialYearId, int studentid)
        {
            return await _studentFeeCollectionRepo.FeeDetails(termId,financialYearId,studentid);
        }
        public async Task<ResponseMessage> AddFeeCollection(StudentFeeCollection model,decimal fees)
        {
            return await _studentFeeCollectionRepo.AddFeeCollection(model,fees);
        }
        public async Task<IEnumerable<StudentFeeCollection>> GetStudentFeeCollectionList()
        {
            return await _studentFeeCollectionRepo.GetStudentFeeCollectionList();
        }
        public async Task<IEnumerable<StudentFeeCollection>> GetReport_studentFeeMaster(long studentid, long id2, long id3, string RecieptNo)
        {
            return await _studentFeeCollectionRepo.GetReport_studentFeeMaster(studentid,id2, id3,RecieptNo);
        }
    }
}
