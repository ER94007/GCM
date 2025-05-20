using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GCM.Repository.Abstraction;
using GCM.Services.Abstraction;
using Inward.Entity;
using Inward.Repository.Abstraction;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace GCM.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudent _StudentRepo;
        public StudentService(IStudent studentRepo)
        {
            _StudentRepo = studentRepo ?? throw new ArgumentNullException(nameof(studentRepo));
        }
        public async Task<ResponseMessage> AddStudent(DataTable studentTable, long yearid, long semid, long userid)
        {
           // return await _StudentRepo.AddStudent(studentTable);
            var result = await _StudentRepo.AddStudent(studentTable, yearid,semid,userid);

            // Return the result to the caller
            return result;
        }
        
    }
}
