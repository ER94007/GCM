using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inward.Entity;
using Inward.Repository.Abstraction;
using Inward.Repository;
using GCM.Repository.Abstraction;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;

namespace GCM.Repository
{
    public class StudentRepo : BaseRepository<BaseDataTableEntity>, IStudent
    {
        public IConfiguration appConfig;

        public StudentRepo(IConfiguration config) : base(config)
        {
            appConfig = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<ResponseMessage> AddStudent(DataTable studentTable)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@studentTable", studentTable);
                    //queryParameters.Add("@studentname", st.name);
                    //queryParameters.Add("@mobileno", st.mobileno);
                    //queryParameters.Add("@email", st.email);
                    //queryParameters.Add("@genderid", st.genderid);
                    //queryParameters.Add("@categoryid", st.categoryid);
                    //queryParameters.Add("@applicationno", st.applicationno);
                    //queryParameters.Add("@enrollmentno", st.enrolmentno);
                    return await conn.QueryFirstOrDefaultAsync<ResponseMessage>(StoreProcedures.AddUpdateStudent, queryParameters, commandType: CommandType.StoredProcedure);

                }
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }
    }
}
    