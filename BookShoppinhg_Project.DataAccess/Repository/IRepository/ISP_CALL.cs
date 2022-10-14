﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppinhg_Project.DataAccess.Repository.IRepository
{
   public interface ISP_CALL:IDisposable
    {
        T Single<T>(string ProcedureName, DynamicParameters param = null);
        T OneRecord<T>(string ProcedureName, DynamicParameters param = null);
        void Execute(string ProcedureName, DynamicParameters param = null);
        IEnumerable<T> List<T>(string ProcedureName, DynamicParameters param = null);
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string ProcedureName, DynamicParameters param = null);
    }
}
