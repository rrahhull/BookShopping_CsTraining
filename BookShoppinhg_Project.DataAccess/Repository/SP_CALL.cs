using BookShopping_Project.DataAccess.Data;
using BookShoppinhg_Project.DataAccess.Repository.IRepository;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppinhg_Project.DataAccess.Repository
{
    public class SP_CALL:ISP_CALL
    {
        private readonly ApplicationDbContext _context;
        private static string ConnectionString = "";
        public SP_CALL(ApplicationDbContext context)
        {
            _context = context;
            ConnectionString = _context.Database.GetDbConnection().ConnectionString;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Execute(string ProcedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                sqlCon.Execute(ProcedureName, param, commandType: CommandType.StoredProcedure);
            }

        }

        public IEnumerable<T> List<T>(string ProcedureName, DynamicParameters param = null)
        {
            using(SqlConnection sqlCon=new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                return sqlCon.Query<T>(ProcedureName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string ProcedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon=new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                var Result = SqlMapper.QueryMultiple(sqlCon, ProcedureName, param, commandType: CommandType.StoredProcedure);
                var Item1 = Result.Read<T1>();
                var Item2 = Result.Read<T2>();
                if (Item1 != null && Item2 != null)
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(Item1, Item2);
            }
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }

        public T OneRecord<T>(string ProcedureName, DynamicParameters param = null)
        {
            using(SqlConnection sqlCon =new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                var Value = sqlCon.Query<T>(ProcedureName, param, commandType: CommandType.StoredProcedure);
                return Value.FirstOrDefault();
            }
        }

        public T Single<T>(string ProcedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                return sqlCon.ExecuteScalar<T>(ProcedureName,param,commandType:CommandType.StoredProcedure);
            }
        }
    }
}
