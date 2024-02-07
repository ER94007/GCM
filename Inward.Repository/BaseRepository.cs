using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inward.Entity;
using Inward.Repository.Abstraction;

namespace Inward.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseDataTableEntity
    {
        IConfiguration appConfig;
        public BaseRepository(IConfiguration config)
        {
            appConfig = config;
        }

        protected IDbConnection GetConnection()
        {
            return new SqlConnection(appConfig.GetConnectionString("Inward"));
        }
    }
}
